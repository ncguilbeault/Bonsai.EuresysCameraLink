# Bonsai.EuresysCameraLink
[Bonsai](https://bonsai-rx.org/) library containing frame grabber module for acquiring images from cameras connected to a [Euresys GrabLink Card](https://www.euresys.com/Products/Frame-Grabbers/Grablink-series).

## Installation
First, you need to ensure you have established communication between the Euresys card and your camera. Make sure you can access the camera correctly using the software provided by Euresys and your camera provider.
Once the connection between the card and the camera has been established, open up the Bonsai Package Manager, go to settings, and then add the .nuget folder path found in the Bonsai.EuresysCameraLink repository to the list of available package sources.
After adding the folder path to the list of available package sources, look for the EuresysCameraLink package in online packages and click to install the package.

## Usage
After installing the EuresysCameraLink Bonsai package, a **EuresysFrameGrabber** source node will become available to add to your workflows. 
If everything has been installed correctly, you will now be able to acquire frames from your camera within Bonsai.

### Notes:
This was originally developed for the [Euresys GrabLink Full XR](https://www.euresys.com/Products/Frame-Grabbers/Grablink-series/Grablink-Full-XR) and the [Sentech STC-CMB200PCL](http://www.sentechamerica.com/En/Cameras/CameraLink/STC-CMB200PCL).
Since I have not tested other camera and board configurations, I cannot ensure that this package will work with your board and camera setup. However, I encourage you to try it out anyways to see if it works.
