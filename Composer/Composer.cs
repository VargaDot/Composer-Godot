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
        private readonly CreateSettings DefaultCreateSettings = new(){SceneParent = ((SceneTree)Engine.GetMainLoop()).Root};
        private readonly Loader Loader = new();

        public override void _EnterTree()
        {
            AddChild(Loader,true);
            Loader.LoaderStarted += OnSceneBeganLoading;
            Loader.LoaderLoadingUpdated += OnLoadingUpdated;
        }

        public Scene GetScene(string name)
        {
            if (!Scenes.ContainsKey(name))
            {
                GD.PrintErr($"GetScene error: Scene {name} doesn't exist in memory.");
                return null;
            }

            return Scenes[name];
        }

        public void AddScene(string name, string path, AddSettings settings = null)
        {
            if (Scenes.ContainsKey(name))
            {
                GD.PrintErr($"AddScene error: Scene {name} already exists in memory.");
                return;
            }

            var scene = new Scene(name, path);
            scene.FinishedLoading += OnSceneLoaded;
            scene.FinishedCreating += OnSceneCreated;

            VerifyPreAddSettings(name, settings);

            Scenes.Add(name,scene);

            VerifyPostAddSettings(name, settings);
        }

        public async void LoadScene(string name, LoadSettings settings = null)
        {
            if (!Scenes.ContainsKey(name))
            {
                GD.PrintErr($"LoadScene error: Scene {name} doesn't exist in memory.");
                return;
            }

            VerifyPreLoadSettings(name, settings);

            var scene = Scenes[name];
            scene.Load();

            await ToSignal(scene,Scene.SignalName.FinishedLoading);

            VerifyPostLoadSettings(name, settings);
        }

        public async void CreateScene(string name, CreateSettings settings = null)
        {
            settings ??= DefaultCreateSettings;

            if (!Scenes.ContainsKey(name))
            {
                GD.PrintErr($"CreateScene error: Scene {name} doesn't exist in memory.");
                return;
            }

            VerifyPreCreateSettings(name, settings);

            var scene = Scenes[name];
            scene.Create(settings.SceneParent);

            await ToSignal(scene, Scene.SignalName.FinishedCreating);

            VerifyPostCreateSettings(name, settings);
        }

        public void ReplaceScene(string sceneToRemove, string sceneToAdd, Node parent)
        {
            RemoveScene(sceneToRemove);
            CreateScene(sceneToAdd, new CreateSettings{
                SceneParent = parent
            });
        }

        public void EnableScene(string name)
        {
            if (!Scenes.ContainsKey(name))
            {
                GD.PrintErr($"EnableScene error: Scene {name} doesn't exist in memory.");
                return;
            }

            Scenes[name].Enable();
            EmitSignal(SignalName.SceneEnabled, name);
            ComposerGD?.EmitSignal(ComposerGD.SignalName.SceneEnabled, name);
        }

        public void DisableScene(string name)
        {
            if (!Scenes.ContainsKey(name))
            {
                GD.PrintErr($"EnableScene error: Scene {name} doesn't exist in memory.");
                return;
            }

            Scenes[name].Disable();
            EmitSignal(SignalName.SceneDisabled, name);
            ComposerGD?.EmitSignal(ComposerGD.SignalName.SceneDisabled, name);
        }

        public void RemoveScene(string name)
        {
            if (!Scenes.ContainsKey(name))
            {
                GD.PrintErr($"RemoveScene error: Scene {name} doesn't exist in memory.");
                return;
            }

            Scenes[name].Remove();
            EmitSignal(SignalName.SceneRemoved, name);
            ComposerGD?.EmitSignal(ComposerGD.SignalName.SceneRemoved, name);
        }

        public void DisposeScene(string name)
        {
            if (!Scenes.ContainsKey(name))
            {
                GD.PrintErr($"DisposeScene error: Scene {name} doesn't exist in memory.");
                return;
            }

            var scene = Scenes[name];
            scene.FinishedLoading -= OnSceneLoaded;
            scene.FinishedCreating -= OnSceneCreated;
            scene.Dispose();
            Scenes.Remove(name);

            EmitSignal(SignalName.SceneDisposed, name);
            ComposerGD?.EmitSignal(ComposerGD.SignalName.SceneDisposed, name);
        }

        private void VerifyPreAddSettings(string name, AddSettings settings)
        {

        }

        private void VerifyPostAddSettings(string name, AddSettings settings)
        {
            if (settings.InstantLoad)
            {
                LoadScene(name,new LoadSettings{
                    SceneParent = settings.SceneParent,
                    InstantCreate = settings.InstantCreate
                });
            }
        }

        private void VerifyPreLoadSettings(string name, LoadSettings settings)
        {

        }

        private void VerifyPostLoadSettings(string name, LoadSettings settings)
        {
            if (settings.SceneParent != null)
            {
                if (settings.InstantCreate)
                {
                    CreateScene(name,new CreateSettings{
                        SceneParent = settings.SceneParent
                    });
                }
            }
        }

        private void VerifyPreCreateSettings(string name, CreateSettings settings)
        {
            if (!IsInstanceValid(settings.SceneParent))
            {
                throw new ArgumentException($"Invalid SceneParent argument for CreateScene, scene {name}");
            }
        }

        private void VerifyPostCreateSettings(string name, CreateSettings settings)
        {
            if (settings.DisableProcessing)
            {
                Scenes[name].Disable();
            }
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

        private void OnSceneLoaded(string sceneName)
        {
            EmitSignal(SignalName.SceneLoaded, sceneName);
            ComposerGD?.EmitSignal(ComposerGD.SignalName.SceneLoaded, sceneName);
        }
    }
}

