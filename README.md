# Unity-Indoor_AR_Navigation
It's Indoor AR Navigation made with Unity.

-*Youtube presentation video Link : *   
-*Youtube Demo video Link : *

<img width = 28% src=https://user-images.githubusercontent.com/76057758/167531642-8973d3b5-0b47-42cd-91b4-135f9f42614c.gif> <img width = 28% src=https://user-images.githubusercontent.com/76057758/167301658-aaa9c6f0-b034-4742-ac03-2b12700e1014.gif>
<img width = 28% src=https://user-images.githubusercontent.com/76057758/167550653-a379fefa-5741-45c4-a88b-62e81d760265.gif>


The interior of the large building is very spacious and there are many roads, so it’s not easy to get to the desired location. The building may have a map, but you can’t memorize each one.

So, to relieve this inconvenience, we made an indoor AR navigation system. It can be used anywhere 3D modeling is prepared!

- guide the user on the way   
- AR environment for easy and visual help   
- providing additional information

## **System Flow :**
<img src=https://user-images.githubusercontent.com/76057758/172294345-f0fd2ae5-db00-48c1-ac8f-e0e2b0bf347c.png>

## **Environment :**
Unity `2021.1.5f1`   
ARCore XR `4.2.2`   
AR Foundation `4.2.2`   
AT+ Explore `1.4.9f3`   
SketchUp   

## **Requirements and supported device :**
Our project is using ARcore package to show AR environment within Unity. That's why you need to use a device that supports ARCore.

### **Android)**   
Android 8.1 (API 27) or later is required.   
- The rear (world) camera is emulated as a virtual scene.
- Front (selfie) cameras are not supported.

### **iOS)**   
The current project is based on Android, but the Unity project settings allow you to add an iOS experience.   
The device must support ARKit. Lowest possible deployment target is iOS 12.0, but latest iOS 14.0 is strongly recommended.

## **How to use it? :**

Recognize maps modeled with QR codes.| 2D minimap showing the user's current location.  
:-------------------------:|:-------------------------:
<img src="https://user-images.githubusercontent.com/76037656/167076052-72fedd6c-7156-4e60-856d-44fe12ac4dfd.png" width="180px"></img>  |  <img src="https://user-images.githubusercontent.com/76037656/167076131-77ec5ce0-5d99-4eaa-a386-683fc10af319.PNG" width="180px"></img>
**UI indicating the remaining distance to the next point.**  |  **Allows users to search for destinations they want.**
<img src="https://user-images.githubusercontent.com/76037656/167076134-9ff25658-95a8-4326-9d03-ddc111f56d67.PNG" width="180px" /></img>  |  <img src="https://user-images.githubusercontent.com/76037656/167076138-715c54db-b397-4eb6-b3ac-55e82d415f6a.png" width="180px"></img>

* When the QR code is recognized, the 3D modeled map is brought into the AR environment.

* After that, when the user clicks on the desired destination, the shortest distance is obtained by using the navigation algorithm to the destination.

* The shortest distance obtained above includes an intermediate point, and the nearest intermediate point from the user is referred to as the next arrival point.

* Here, the arrow indicating the direction, the distance remaining to the next arrival point, and the 2D mini-map make it easier for users to navigate.

* When you arrive at the arrival point, set the middle point to the arrival point and update the direction of the arrow, the remaining distance, and the mini map.

* The goal of this project is to make it easier for users to find their way to the desired destination in this way.


## **How Navigation work :**

3D modeling | Dijkstra Algorithm  
:-------------------------:|:-------------------------:
<img width=70% src=https://user-images.githubusercontent.com/43882631/172041501-192f2588-e22b-4e92-a0b9-abdc0ff9b0dd.png>  |  <img src="https://user-images.githubusercontent.com/43882631/167574411-c274843d-fce0-4b63-a961-af20c8858641.gif"></img>
**Algorithm making list of points to destination**  |  **Arrow**
<img src="https://user-images.githubusercontent.com/43882631/172042101-22d92899-2b02-49a4-b91e-3f3c455968ad.png"></img>  |  <img width=75% src="https://user-images.githubusercontent.com/43882631/172040964-6a2c1385-4673-47ba-8da3-145104879ee2.gif"></img>

* The AR navigation we produced does not use the GPS function. We planned a program to check the movement through the virtual 3D map and the user’s camera and show the objects placed in the AR environment.

* Start the search based on the destination and explore the shortest path. Route exploration uses the Dijktra algorithm to contain information about the next point at each point, so it can be adjusted immediately if user moves differnt route from the existing path.

* The arrow always point to the next location by using 'lookat' function in unity.

<p align="center">
  <img src=https://user-images.githubusercontent.com/43882631/172041303-8077d8ab-3d2e-4c11-a939-e4803b91f5d5.gif> 
</p>

<!---
## **Features :**

### `Collider`
The collision function was used to recognize that the user reached the crosspoint or arrival point.

### `Image Tracking`
The camera recognizes the predetermined image, creates and designates a location and an object.

### `Laycast`
Shooting invisible rays to determine what objects are hit by the rays and then post-processing them.

### `LineRenderer`
The line renderer draws lines that connect each using an array of two or more points in the 3D space.
The project draws a line by recognizing the user's location on the map and the following points.

### `LOD Group`
LOD, Level Of Detail, is one of the optimaization technologies created to reduce the load of the 
system due to the nature of the project that needs to be implemented in real time.
Within the project, distant objects were used to make them invisible to the user.

### `Unity UI`

### `2D Sprite`
--->

## **Member:**
* [Kwon Soon Wan] @RootOfGroot / AI-Software Gachon UNIV.
* [Jeon Tak] @JEONTAK / AI-Software Gachon UNIV.
* [Hwang Seong Min] @tnfpck0306 / AI-Software Gachon UNIV.
