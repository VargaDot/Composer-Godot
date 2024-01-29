# Composer

A lightweight scene manager library for [Godot](https://godotengine.org/), written in C#, with a mere 20 KB footprint. It boasts robust efficiency and cross-language scripting capabilities, supporting both 2D and 3D environments.\
This library excels in loading scenes [asynchronously](https://en.wikipedia.org/wiki/Asynchrony_(computer_programming)), facilitating the implementation of dynamic loading screens, smooth transitions and other features.

## 🧾 Prerequisites

- [Godot](https://godotengine.org/) 4.2 (.NET version)
- [.NET](https://docs.godotengine.org/en/stable/tutorials/scripting/c_sharp/c_sharp_basics.html#prerequisites) SDK 

## 🛠️ Installation

- Download the `Composer` folder as .ZIP or from the releases tab
- Extract it into your project folder
- [Autoload](https://docs.godotengine.org/en/stable/tutorials/scripting/singletons_autoload.html) `Composer.cs` and if you plan to use it from `.gd` files, autoload `ComposerGD.cs` as well

## ⚙️ Usage
Disclaimer: If you're utilizing ComposerGD, replace `Composer.` to `ComposerGD.` in the provided C# code snippets unless a GDScript example is presented.
<details>
<summary><strong>Scene Creation</strong></summary>

**Method 1:**\
First, add a reference name and path to the *Manifest*.
```
Composer.AddScene("MyScene", "res://path/to/MyScene")
```

Then create it.
```
Composer.CreateScene("MyScene")
```
\
**Method 2:**\
We can add a scene and create it instantly using *SceneSettings*, without needing to call `CreateScene`.

C#:
```
Composer.AddScene("MyScene", "res://path/to/MyScene", new(){
    InstantCreate = true,
})
```

GDScript:
```
ComposerGD.AddScene("MyScene", "res://path/to/MyScene", {
    "instant_create":true,
})
```

**Method 3:**\
We can add a scene with the export method.

**Method x:**\
We can add scenes using packed resources.

**Method x:**\
We can add scenes with "InstantCreate = true" in bulk.

**Method x:**\
We can create scenes in bulk.

</details>

<details>
<summary><strong>Scene Handling</strong></summary>

**Assigning Parents:**\
By default, scenes will be instantiated as children of `/root`, you can assign a custom parent with the SceneParent setting.
if the SceneParent is null, Composer will fallback to `/root`.

**Replacing Scenes:**\
To replace a scene with another one, we use the `ReplaceScene` Method.
```
Composer.ReplaceScene("SceneToReplace", "NewScene")
```

**Reloading Scenes:**\
To reload a scene, use the `ReloadScene` Method.
```
Composer.ReloadScene("SceneToReload")
```

**Run Scenes**\
Use `EnableScene` to run a scene, useful for making it run in the background. 
```
Composer.EnableScene("MyScene")
```

**Stop Scenes:**\
Stop or Pause scenes using `DisableScene`
```
Composer.DisableScene("MyScene")
```

</details>

<details>
<summary>Scene Removal</summary>

    
    
</details>
