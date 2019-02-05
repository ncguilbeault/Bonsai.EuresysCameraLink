using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenCV.Net;
using System.Threading;
using System.Reactive.Linq;
using System.Reactive.Disposables;
using System.Threading.Tasks;
using System.Drawing.Design;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using System.ComponentModel;
using System.Diagnostics;
using Euresys.MultiCam;

public enum BoardTopologyTypes
{ 
    MONO = 0, 
    MONO_DECA = 1,
    MONO_SLOW = 2,
    MONO_OPT1 = 3,
    MONO_DECA_OPT1 = 4
}

namespace Bonsai.EuresysCameraLink
{
    //[DefaultProperty("CamFile")]
    // Create a description for the Euresys Frame Grabber Bonsai node
    [Description("Produces a sequence of images acquired from a camera connected to the Euresys Grablink Camera Link Card (Euresys Inc.).")]
    //[Editor("Bonsai.Vision.Design.FileCaptureEditor, Bonsai.Vision.Design", typeof(ComponentEditor))]
    public class EuresysFrameGrabber : Source<IplImage>
    {

        // Generate IplImage for Source Node
        public override IObservable<IplImage> Generate()
        {
            return source;
        }

        // Create a description for acquiring the cam file
        [Editor("Bonsai.Design.OpenFileNameEditor, Bonsai.Design", typeof(UITypeEditor))]
        [Description("The name of the cam file to use. The cam file specifies all of the camera parameters.")]
        public string CamFile { get; set; }

        // Create a description for the board typology
        [Description("The type of board typology for camera and board configuration.")]
        public BoardTopologyTypes BoardTypology { get; set; }

        private IObservable<IplImage> source;
        private readonly object captureLock = new object();
        private IObserver<IplImage> global_observer;
        private uint channel;
        private MC.CALLBACK multiCamCallback;

        private void multiCamProcessingCallback(ref MC.SIGNALINFO signalInfo)
        {
            if (signalInfo.Signal == MC.SIG_SURFACE_PROCESSING)
            {
                int width;
                int height;
                IntPtr bufferAddress;
                uint currentChannel = signalInfo.Instance;
                uint currentSurface = signalInfo.SignalInfo;
                IplImage output;
                unsafe
                {
                    MC.GetParam(currentChannel, "ImageSizeX", out width);
                    MC.GetParam(currentChannel, "ImageSizeY", out height);
                    MC.GetParam(currentSurface, "SurfaceAddr:0", out bufferAddress);
                    output = new IplImage(new Size(width, height), IplDepth.U8, 1, bufferAddress).Clone();
                }
                global_observer.OnNext(output);
            }
        }

        public EuresysFrameGrabber()
        {
            source = Observable.Create<IplImage>((observer, cancellationToken) =>
            {
                return Task.Factory.StartNew(() =>
                {
                    lock (captureLock)
                    {
                        //string camFilePath = CamFile;
                        string camFile = System.IO.Path.GetFileNameWithoutExtension(CamFile);
                        var boardTypology = BoardTypology;
                        global_observer = observer;
                        MC.OpenDriver();
                        MC.SetParam(MC.BOARD + 0, "BoardTopology", boardTypology.ToString());
                        MC.Create("CHANNEL", out channel);
                        MC.SetParam(channel, "DriverIndex", 0);
                        MC.SetParam(channel, "Connector", "M");
                        MC.SetParam(channel, "CamFile", camFile);
                        multiCamCallback = new MC.CALLBACK(multiCamProcessingCallback);
                        MC.RegisterCallback(channel, multiCamCallback, channel);
                        try
                        {
                            MC.SetParam(channel, MC.SignalEnable + MC.SIG_SURFACE_PROCESSING, "ON");
                            MC.SetParam(channel, "ChannelState", "ACTIVE");
                            while (!cancellationToken.IsCancellationRequested)
                            {
                                // Do nothing
                            }
                            MC.SetParam(channel, MC.SignalEnable + MC.SIG_SURFACE_PROCESSING, "FREE");
                        }
                        finally
                        {
                            MC.Delete(channel);
                            MC.CloseDriver();
                        }
                    }
                },
                cancellationToken,
                TaskCreationOptions.LongRunning,
                TaskScheduler.Default);
            })
            .PublishReconnectable()
            .RefCount();
        }
    }
}
