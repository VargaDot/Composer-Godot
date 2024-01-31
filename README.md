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
⚠️ Disclaimer: If you're utilizing ComposerGD, replace `Composer.` to `ComposerGD.` in the provided C# code snippets unless a GDScript example is presented.
<details>
<summary><strong>🔨 Scene Creation</strong></summary>

**Method 1:**
+ First, add a reference name and path to the *Manifest*.
```
Composer.AddScene("MyScene", "res://path/to/MyScene")
```

+ Then create it.
```
Composer.CreateScene("MyScene")
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

+ You can also set a custom parent

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

+ You can also disable autoloading the PackedScene resource with `InstantLoad` parameter set to false.

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
<summary><strong>🏗️ Scene Handling</strong></summary>

**Get Scene:**
+ Returns the `Scene` class based on the InternalName of the scene. Useful for making direct interactions with the instance.
```
var scene = Composer.GetScene("MyScene")
```

**Replacing Scenes:**
+ To replace a scene with another one, we use the `ReplaceScene` Method.
```
Composer.ReplaceScene("MyScene", "NewScene")
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
<summary><strong>📝 Assigning Custom Parents</strong></summary>

+ By default, scenes will be instantiated as children of `/root`. However, you can assign a custom parent through a multitude of ways.

**Method 1:**
+ With SceneSettings

C#:
```
Composer.AddScene(//Other parameters, new(){
    SceneParent = parent,
})
```
\
GDScript:
```
ComposerGD.AddScene(//Other parameters, {
    "scene_parent":parent
})
```
+ If SceneParent is null, Composer will fallback to `/root`.

**Method 2:**
+ Through CreateScene with an optional parameter.
```
Composer.CreateScene("MyScene", parent)
```

**Method 3:**
+ Using the ReplaceScene Method.
```
Composer.ReplaceScene(//Other parameters, newParent)
```

</details>

<details>
<summary><strong>🔥 Scene Removal</strong></summary>

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
<summary><strong>🚥 Signals</strong></summary>

⚠️ Warning: If you are using ComposerGD, connect the signals from ComposerGD.

**SceneBeganLoaded**
+ Emitted when scene has began its loading of Resource. Fires with a sceneName parameter which is always the `InternalName` of the scene.
```
SceneBeganLoaded(string sceneName)
```

**SceneLoaded**
+ Emitted when scene has finished its loading of Resource. Fires with a sceneName parameter which is always the `InternalName` of the scene.
```
SceneLoaded(string sceneName)
```

**SceneAllLoaded**
+ Emitted when every scene that has existed in the queue has been loaded.
```
SceneAllLoaded()
```

**SceneLoadingProcessUpdated**
+ Emitted with every process tick when the scene is being loaded. Fires with a sceneName parameter which is always the `InternalName` of the scene and also the progress parameter which represents the percentage of loading.
```
SceneLoadingProcessUpdated(string sceneName, float progress)
```

**SceneCreated**
+ Emitted when scene has finished creating via CreateScene(). Fires with a sceneName parameter which is always the `InternalName` of the scene.
```
SceneCreated(string sceneName)
```

**SceneEnabled**
+ Emitted when scene has been enabled via EnableScene(). Fires with a sceneName parameter which is always the `InternalName` of the scene.
```
SceneEnabled(string sceneName)
```

**SceneDisabled**
+ Emitted when scene has been disabled via DisableScene(). Fires with a sceneName parameter which is always the `InternalName` of the scene.
```
SceneDisabled(string sceneName)
```

**SceneRemoved**
+ Emitted when scene has been removed via RemoveScene(). Fires with a sceneName parameter which is always the `InternalName` of the scene.
```
SceneRemoved(string sceneName)
```

**SceneDisposed**
+ Emitted when scene has been disposed via DisposeScene(). Fires with a sceneName parameter which is always the `InternalName` of the scene.
```
SceneDisposed(string sceneName)
```

</details>

## Demonstrations
Check out [Composer-Demos](https://github.com/VargaDot/Composer-Demos) for examples of how Composer can be implemented.
