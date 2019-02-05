# Bonsai.EuresysCameraLink
[Bonsai](https://bonsai-rx.org/) library containing frame grabber module for acquiring images from cameras connected to a [Euresys GrabLink Card](https://www.euresys.com/Products/Frame-Grabbers/Grablink-series).

## Installation
First, you need to ensure you have established communication between the Euresys card and your camera. You need to ensure that you can access the camera correctly using the software provided by Euresys and your camera provider.
Once the connection between the card and the camera has been established, open up the Bonsai Package Manager, go to settings, and then add the .nuget folder path found in the Bonsai.EuresysCameraLink repository to the list of available package sources.
After adding the folder to the path, noew look for the package under online packages and install the Bonsai.EuresysCameraLink package to Bonsai.

## Usage
After the path to the NuGet package has been added to Bonsai, a ***EuresysFrameGrabber*** source node will become available to add to your workflows. 
If everything has been installed correctly, you will now be able to acquire frames from your camera within Bonsai.

### Notes:
I originally developed this package for the [Euresys GrabLink FUll XR](https://www.euresys.com/Products/Frame-Grabbers/Grablink-series/Grablink-Full-XR) and the [Sentech STC-CMB200PCL](http://www.sentechamerica.com/En/Cameras/CameraLink/STC-CMB200PCL).
Since I have only been able to test this package with this particular board and camera, I cannot ensure that this package will work with other board / camera configurations. However, I encourage you to try it out anyways.
