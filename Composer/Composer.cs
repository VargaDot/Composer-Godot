using Godot;
using System;

namespace ComposerLib
{
    public partial class Composer : Node
    {
        [Signal]
        public delegate void SceneLoadedEventHandler(string sceneName);

        [Signal]
        public delegate void SceneCreatedEventHandler(string sceneName);

        public Godot.Collections.Dictionary<string, Scene> Scenes = new();
        internal ComposerGD ComposerGD {get; set;} = null;
        private readonly CreateSettings DefaultCreateSettings = new(){SceneParent = ((SceneTree)Engine.GetMainLoop()).Root};
        private readonly Loader Loader = new();

        public override void _EnterTree()
        {
            AddChild(Loader,true);
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

        public void CreateScene(string name, CreateSettings settings = null)
        {
            settings ??= DefaultCreateSettings;

            if (!Scenes.ContainsKey(name))
            {
                GD.PrintErr($"CreateScene error: Scene {name} doesn't exist in memory.");
                return;
            }

            VerifyPreCreateSettings(name, settings);

            Scenes[name].Create(settings.SceneParent);

            VerifyPostCreateSettings(name, settings);
        }

        public void ReplaceScene(string sceneToRemove, string sceneToAdd, Node parent)
        {
            RemoveScene(sceneToRemove);
            CreateScene(sceneToAdd, new CreateSettings{
                SceneParent = parent
            });
        }

        public void RemoveScene(string name)
        {
            if (!Scenes.ContainsKey(name))
            {
                GD.PrintErr($"RemoveScene error: Scene {name} doesn't exist in memory.");
                return;
            }

            Scenes[name].Remove();
        }

        public void DisposeScene(string name)
        {
            if (!Scenes.ContainsKey(name))
            {
                GD.PrintErr($"DisposeScene error: Scene {name} doesn't exist in memory.");
                return;
            }

            Scenes[name].Dispose();
            Scenes.Remove(name);
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
            if (settings.SceneParent == null)
            {
                throw new ArgumentException($"Invalid SceneParent argument for CreateScene, scene {name}");
            }
        }

        private void VerifyPostCreateSettings(string name, CreateSettings settings)
        {

        }

        private void OnSceneCreated(string sceneName)
        {
            EmitSignal(SignalName.SceneCreated, sceneName);
            ComposerGD?.EmitSignal(ComposerGD.SignalName.SceneCreated, sceneName);
        }

        private void OnSceneLoaded(string sceneName)
        {
            EmitSignal(SignalName.SceneLoaded, sceneName);
            ComposerGD?.EmitSignal(ComposerGD.SignalName.SceneLoaded, sceneName);
        }
    }
}

