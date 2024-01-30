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

**Method 1:**
+ First, add a reference name and path to the *Manifest*.
```
Composer.AddScene("MyScene", "res://path/to/MyScene")
```

+ Then create it.
```
Composer.CreateScene("MyScene")
```

+ You can also directly specify a parent for the scene in CreateScene with an optional parameter:
```
Composer.CreateScene("MyScene", parent)
```
\
**Method 2:**
+ You can add a scene and create it instantly using *SceneSettings*, without needing to call `CreateScene`.

C#:
```
Composer.AddScene("MyScene", "res://path/to/MyScene", new(){
    InstantCreate = true,
})
```
\
GDScript:
```
ComposerGD.AddScene("MyScene", "res://path/to/MyScene", {
    "instant_create":true,
})
```

+ With SceneSettings, you can also disable autoloading the PackedScene resource with `InstantLoad` parameter set to false.

C#:
```
Composer.AddScene("MyScene", "res://path/to/MyScene", new(){
    InstantLoad = false,
})
```
\
GDScript:
```
ComposerGD.AddScene("MyScene", "res://path/to/MyScene", {
    "instant_load":false,
})
```

+ To later load a scene, you can then call `LoadScene` method:
```
Composer.LoadScene("MyScene")
```

**Method 3:**
+ You can add a prexisting scene that has been either created by making an instance of the *Scene* class or created directly in the editor itself, using Resources.
```
var scene = new Scene("MyScene","res://path/to/MyScene")

Composer.AddScene(scene)
```

+ If you have multiple scenes already created, you can also use the `AddScenes` method. It takes an Array of *Scene* instances.
```
Composer.AddScenes(new (){
    scene1, scene2, scene3, ...
})
```

There are also specific methods called `LoadScenes` and `CreateScenes` for loading/creating multiple scenes at once. They too only take one parameter, this being an Array of *Scene* instances.

</details>

<details>
<summary><strong>Scene Handling</strong></summary>

**Get Scene:**
+ Returns the `Scene` class based on the InternalName of the scene. Useful for making direct interactions with the instance.
```
var scene = Composer.GetScene("MyScene")
```

**Assigning Parents:**
+ By default, scenes will be instantiated as children of `/root`, you can assign a custom parent with the SceneParent setting.
if the SceneParent is null, Composer will fallback to `/root`.

**Replacing Scenes:**
+ To replace a scene with another one, we use the `ReplaceScene` Method. You can also specify an optional new parent for the replacement scene (default is null meaning use the current parent).
```
Composer.ReplaceScene("MyScene", "NewScene", newParent)
```

**Reloading Scenes:**
+ To reload a scene, use the `ReloadScene` Method.
```
Composer.ReloadScene("MyScene")
```

**Run Scenes**
+ Use `EnableScene` to run a scene, useful for unpausing. 
```
Composer.EnableScene("MyScene")
```

**Stop Scenes:**
+ Use `EnableScene` to stop a scene, useful for pausing. 
```
Composer.DisableScene("MyScene")
```

</details>

<details>
<summary><strong>Scene Removal</strong></summary>

**Unload Scene:**
+ Removes the scene resource.
```
Composer.UnloadScene("MyScene")
```

**Remove Scene from tree:**
+ Removes the instance from the tree.
```
Composer.RemoveScene("MyScene")
```

**Dispose of Scene from memory:**
+ Removes the instance, resource and InternalName.
```
Composer.DisposeScene("MyScene")
```

</details>

<details>
<summary><strong>Signals</strong></summary>

</details>

## Demonstrations
Check out [Composer-Demos](https://github.com/VargaDot/Composer-Demos) for examples of how Composer can be implemented.
