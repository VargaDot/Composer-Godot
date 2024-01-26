using Godot;
using System;

namespace ComposerLib
{
    public partial class Composer : Node
    {
        [Signal]
        public delegate void SceneBeganLoadedEventHandler(string sceneName);

        [Signal]
        public delegate void SceneLoadedEventHandler(string sceneName);

        [Signal]
        public delegate void ScenesAllLoadedEventHandler();

        [Signal]
        public delegate void SceneLoadingProcessUpdatedEventHandler(string sceneName, float progress);

        [Signal]
        public delegate void SceneCreatedEventHandler(string sceneName);

        [Signal]
        public delegate void SceneEnabledEventHandler(string sceneName);

        [Signal]
        public delegate void SceneDisabledEventHandler(string sceneName);

        [Signal]
        public delegate void SceneRemovedEventHandler(string sceneName);

        [Signal]
        public delegate void SceneDisposedEventHandler(string sceneName);


        public Godot.Collections.Dictionary<string, Scene> Scenes = new();
        internal ComposerGD ComposerGD {get; set;} = null;
        private readonly Loader Loader = new();

        public override void _EnterTree()
        {
            AddChild(Loader, true);
            Loader.LoaderStarted += OnSceneBeganLoading;
            Loader.LoaderLoadingUpdated += OnLoadingUpdated;
            Loader.LoaderAllFinished += OnLoadingAllFinished;
        }

        public Scene GetScene(string name)
        {
            if (!Scenes.TryGetValue(name, out Scene scene))
            {
                GD.PrintErr($"GetScene error: Scene {name} doesn't exist in memory.");
                return null;
            }

            return scene;
        }

        public void AddScene(string name, string path, SceneSettings settings = null)
        {
            if (!CheckIfExists(name)) return;

            settings ??= new();

            var scene = new Scene(name, path, settings);
            scene.FinishedLoading += OnSceneLoaded;
            scene.FinishedCreating += OnSceneCreated;

            Scenes.Add(name, scene);
        }

        public void AddScene(string name, PackedScene resource, string path = "", SceneSettings settings = null)
        {
            if (!CheckIfExists(name)) return;

            settings ??= new();

            var scene = new Scene(name, resource, path, settings);
            scene.FinishedLoading += OnSceneLoaded;
            scene.FinishedCreating += OnSceneCreated;

            Scenes.Add(name, scene);
        }

        public void AddScene(Scene scene)
        {
            if (!CheckIfExists(scene.InternalName)) return;

            scene.FinishedLoading += OnSceneLoaded;
            scene.FinishedCreating += OnSceneCreated;

            Scenes.Add(scene.InternalName, scene);
        }

        public void AddScenes(Godot.Collections.Array<Scene> scenes)
        {
            foreach (Scene scene in scenes)
            {
                AddScene(scene);
            }
        }

        public void LoadScene(string name)
        {
            var scene = GetScene(name);
            if (!CheckForNull(scene, "LoadScene")) return;

            scene.Load();
        }

        public void CreateScene(string name, Node newParent = null)
        {
            var scene = GetScene(name);
            if (!CheckForNull(scene, "CreateScene")) return;

            if (newParent != null)
                scene.Settings.SceneParent = newParent;

            scene.Create();
        }

        public void ReplaceScene(string sceneToRemove, string sceneToAdd, Node newParent = null)
        {
            if (newParent != null)
                GetScene(sceneToAdd).Settings.SceneParent = newParent;

            RemoveScene(sceneToRemove);
            CreateScene(sceneToAdd);
        }

        public async void ReloadScene(string name)
        {
            var scene = GetScene(name);
            if (!CheckForNull(scene, "ReloadScene")) return;

            RemoveScene(name);

            await ToSignal(this, SignalName.SceneRemoved);

            CreateScene(name);
        }

        public void EnableScene(string name)
        {
            var scene = GetScene(name);
            if (!CheckForNull(scene, "EnableScene")) return;

            scene.Enable();
            EmitSignal(SignalName.SceneEnabled, name);
            ComposerGD?.EmitSignal(ComposerGD.SignalName.SceneEnabled, name);
        }

        public void DisableScene(string name)
        {
            var scene = GetScene(name);
            if (!CheckForNull(scene,"DisableScene")) return;

            scene.Disable();
            EmitSignal(SignalName.SceneDisabled, name);
            ComposerGD?.EmitSignal(ComposerGD.SignalName.SceneDisabled, name);
        }

        public void RemoveScene(string name)
        {
            var scene = GetScene(name);
            if (!CheckForNull(scene,"RemoveScene")) return;

            scene.Remove();
            EmitSignal(SignalName.SceneRemoved, name);
            ComposerGD?.EmitSignal(ComposerGD.SignalName.SceneRemoved, name);
        }

        public void DisposeScene(string name)
        {
            var scene = GetScene(name);
            if (!CheckForNull(scene,"DisposeScene")) return;

            scene.FinishedLoading -= OnSceneLoaded;
            scene.FinishedCreating -= OnSceneCreated;
            scene.Dispose();
            Scenes.Remove(name);

            EmitSignal(SignalName.SceneDisposed, name);
            ComposerGD?.EmitSignal(ComposerGD.SignalName.SceneDisposed, name);
        }

        private bool CheckIfExists(string name)
        {
            if (Scenes.ContainsKey(name))
            {
                GD.PrintErr($"AddScene error: Scene {name} already exists in memory.");
                return false;
            }

            return true;
        }

        private bool CheckForNull(Scene scene, string functionName)
        {
            if (scene == null)
            {
                GD.PrintErr($"{functionName} error: Scene {scene?.InternalName} doesn't exist in memory.");
                return false;
            }

            return true;
        }

        private void OnSceneCreated(string sceneName)
        {
            EmitSignal(SignalName.SceneCreated, sceneName);
            ComposerGD?.EmitSignal(ComposerGD.SignalName.SceneCreated, sceneName);
        }

        private void OnSceneBeganLoading(Scene scene)
        {
            EmitSignal(SignalName.SceneBeganLoaded, scene.InternalName);
            ComposerGD?.EmitSignal(ComposerGD.SignalName.SceneBeganLoading, scene.InternalName);
        }

        private void OnLoadingUpdated(Scene scene, float progress)
        {
            EmitSignal(SignalName.SceneLoadingProcessUpdated, scene.InternalName, progress);
            ComposerGD?.EmitSignal(ComposerGD.SignalName.SceneLoadingProcessUpdated, scene.InternalName, progress);
        }

        private void OnLoadingAllFinished()
        {
            EmitSignal(SignalName.ScenesAllLoaded);
            ComposerGD?.EmitSignal(ComposerGD.SignalName.ScenesAllLoaded);
        }

        private void OnSceneLoaded(string sceneName)
        {
            EmitSignal(SignalName.SceneLoaded, sceneName);
            ComposerGD?.EmitSignal(ComposerGD.SignalName.SceneLoaded, sceneName);
        }
    }
}

