# Inverse-Kinematics (Unity, C#)

## Author

**Morgane DERO** Master 1 Game Programmer at Isart Digital

## Description 

The aim of this project is to implement **one** of the three main Inverse Kinematics algorithms (**CyclicCoordinateDescent**, **Jacobian** or **FABRIK**).   
This is a solo academic project exploring **bone chain manipulation**, often used in **game animation** and **robotics**.

>Inverse Kinematics (IK) lets you move the end of a joint chain (**like a hand or foot**), and automatically calculates how all joints should rotate to reach that position.   

>It's widely used to create **natural, realistic movements** with **minimal effort**.


## Architecture

```

Assets
├── Materials
│   └── Target.mat
├── Meshes
│   ├── hypo-carno-the-isle (folder)
│   └── RawModel.prefab
├── Scenes
│   └── TEST.unity
└── Scripts
    ├── Algorithms
    │   ├── AlgoChoice.cs
    │   ├── CyclicCoordinateDescentAlgorithm.cs
    │   ├── FabrikAlgorithm.cs
    │   └── JacobianAlgorithm.cs    
    ├── Lib
    │   └── MathLib.cs    
    └── Managers
        ├── JointManager.cs
        ├── SpawnManager.cs
        └── TargetSpawner.cs    

```

## Getting Started

### To run this project on your machine :    
1. Clone the repository
2. Open Unity (version 6)
3. In Unity, open the scene > Assets/Scenes/TEST.unity

## How to Use 

Once in the scene, you'll find the **IKManager** GameObject.   

This object gives access to the main scripts controlling your experience : 


### Spawn Manager

- If you use the **Raw Model** prefab, make sure the **Is Raw Model** option is checked.
- If unchecked, you can spawn a new model based on your **Segment** settings.

### Algo Choice

- Select the IK algorithm you wish to test : **CCD**, **Jacobian** or **FABRIK**.
- Change the iteration type to : 
  - **AUTO** for automatic iterations.
  - **CLICK_TO_ACTIVATE** to step through each iteration manually.
- During Play mode, you can **Reset** the model to its initial position and rotation any time.

## Code

### JointManager

This script **manages joint rotations**    
> For example, in the human body, arm movement is naturally restricted by joint limits.

- Vector3 clampMin : public
- Vector3 clampMax : public

### MathLib

This script is my personal mathematic tool box.

### Cyclic Coordinate Descent Algorithm

(Coming soon: explanation, screenshots and usage tips!)

### Fabrik Algorithm

(Coming soon: explanation, screenshots and usage tips!)

### Jacobian Algorithm

(Coming soon: explanation, screenshots and usage tips!)

## Organisation 

### Tools & Workflow used

- **Google Calendar** to **plan my work time** due to the lack of hours at school. 
- **Unity 6** as the **Game Engine**.
- **Sticky Notes** (windows) for **issues/tasks to do**.
- **Rider** as my IDE in **C#**

## Interesting Links
- **CCD understanding given by our teacher** :    
https://rodolphe-vaillant.fr/entry/114/cyclic-coordonate-descent-inverse-kynematic-ccd-ik   

- **CCD Beginner-friendly documentation** :      
https://pixeleuphoria.com/blog/index.php/2023/01/21/ik-solver-from-scratch-in-unity/
