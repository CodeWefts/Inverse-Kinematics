# Inverse-Kinematics (Unity, C#)

## Author

> **Morgane DERO** - Master 1 Game Programmer at Isart Digital

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

## Usage and Code Overview

Once in the scene, you'll find the **IKManager** GameObject.   

This object gives access to the main scripts controlling your experience : 


### Spawn Manager

- Generates joint and bone hierarchies procedurally or reads them from an existing model.
- If you use the **Raw Model** prefab, make sure the **Is Raw Model** option is checked. It reads a hierarchy from a model.
- If unchecked, you can spawn a procedural skeleton using your given **segment** value, and gameobjects references.

Each joint receives :
- A JointManager component to store rotation clamps.
- A color gradient for visualization (only with a procedural spawn)
- A parent-child structure defining joint and bone relations.

The class also stores :
- **joints** , **bones** , **initialRotations** lists for use by IK solvers.
- A flag **ite** for debugging step-based CCD iterations.

![SpawnScreens](Assets/Screen_Shots/Spawn_IK1.png)
![SpawnScreens](Assets/Screen_Shots/SpawnIK.png)

### Algo Choice

The central controller that manages solver selection and iteration behavior.
- Allows switching between **CCD** and **FABRIK** algorithms through the **Algorithm** enum.
- Defines iteration type : **AUTO** (continuous solving every frame) or **CLICK_TO_ACTIVATE** (manual iteration).
- Automatically attaches and initializes algorithm components as needed.
- Includes : 
  - **Reset** function to restore initial joint rotations.
  - **IsCloseToTarget()** to determine convergence within a threshold.

### JointManager

This script **manages joint rotation constraints** (clamp values)    
> For example, in the human body, arm movement is naturally restricted by joint limits.

- Vector3 clampMin : public
- Vector3 clampMax : public

These are used by MathLib.QuaternionLib.ClampRotation() to ensure physically plausible motion.

### MathLib

Provides essential mathematical tools for both CCD and FABRIK algorithms.

Features:

- **DotProduct**, **NormalizeAngle**, and **RotationBetween** functions for geometric calculations.

- Nested **QuaternionLib** class implementing:

  - **FromToRotation()** – builds a quaternion from two direction vectors.

  - **ApplyRotation()** – combines two quaternions (custom multiplication).

  - **ClampRotation()** – limits Euler angles of a Transform based on min/max values.

  - **Normalize()** – ensures quaternions remain unit-length.

This library abstracts low-level quaternion math, making it easier to maintain and extend.

### Cyclic Coordinate Descent Algorithm

Implements **CCD** (**Cyclic Coordinate Descent**), a joint-by-joint iterative rotation solver.

Core features:

- Each iteration rotates one joint (**_pivot**) to align the end-effector with the target.

- Uses **MathLib.RotationBetween()** to compute the necessary quaternion rotation.

- Applies rotation using **QuaternionLib.ApplyRotation()** and clamps it with **ClampRotation()**.

- Iteratively processes joints in reverse order until reaching the base joint.

Supports a debug mode through **Iteration()**, allowing per-step visualization controlled by **SpawnManager.ite**.

### Fabrik Algorithm

Implements **FABRIK** (**Forward And Backward Reaching Inverse Kinematics**), a geometric solver operating on joint positions.

Algorithm stages:

1. **Initialization**: Computes segment lengths between joints.

2. **Backward Reaching**: Starts from the end-effector toward the base, aligning each joint towards the target.

3. **Forward Reaching**: Moves joints from base to end-effector, maintaining fixed segment lengths.

4. **Rotation update**: Rotates each joint to align with the repositioned bones.

Key parameters:

- **threshold**: distance to target before stopping.

- **max iterations**: capped at 10 by default for stability.


## How It All Fits Together

1. **SpawnManager** creates a robotic arm or bone chain.

2. Each joint gets a **JointManager** defining its reachable rotation range.

3. **AlgoChoice** initializes the chosen solver (CCD or FABRIK).

4. Depending on **TypeOfIteration**, either:

   - Solvers run automatically each frame (**AUTO**), or

   - One step is executed manually (**CLICK_TO_ACTIVATE**).

5. **MathLib** ensures accurate vector math, quaternion rotations, and clamped results.

## Future Improvements and what else I wanted to do

- Add **per-joint** weighting for smoother CCD motion.
- Visualize **rotation limits** in editor gizmos.
- Include **runtime/in-game target tracking** (moving targets)
- Add **constraints** to the FABRIK algorithm.
- Add **collisions**.

## Organisation 

### Tools & Workflow used

> This section summarizes the tools and workflow used to complete the project efficiently.

- **Google Calendar** to **plan my work time** due to the lack of hours at school. 
- **Unity 6** as the **Game Engine**.
- **Sticky Notes** (windows) for **issues/tasks to do**.
- **Rider** as my IDE in **C#**
- **GitHub** for commits and version tracking

## Interesting Links
- **CCD understanding given by our teacher** :    
https://rodolphe-vaillant.fr/entry/114/cyclic-coordonate-descent-inverse-kynematic-ccd-ik   

- **CCD Beginner-friendly documentation** :      
https://pixeleuphoria.com/blog/index.php/2023/01/21/ik-solver-from-scratch-in-unity/
