using Godot;
using System.Collections.Generic;

namespace ComposerLib
{
    [GlobalClass]
    public partial class Composer : Node
    {
        [Signal]
        public delegate void SceneLoadedEventHandler(string sceneName);

        [Signal]
        public delegate void SceneCreatedEventHandler(string sceneName);

        public Godot.Collections.Dictionary<string, Scene> Scenes = new();

        private readonly CreateSettings DefaultSettings = new(){SceneParent = ((SceneTree)Engine.GetMainLoop()).Root};

        private readonly Loader Loader = new();

        public override void _EnterTree()
        {
            AddChild(Loader);
        }

        public void AddScene(string name, string path)
        {
            if (Scenes.ContainsKey(name))
            {
                GD.PrintErr($"AddScene error: Scene {name} already exists in memory.");
                return;
            }

            var scene = new Scene(name, path);
            scene.FinishedLoading += OnSceneLoaded;
            scene.FinishedCreating += OnSceneCreated;

            Scenes.Add(name,scene);
        }

        public void LoadScene(string name)
        {
            if (!Scenes.ContainsKey(name))
            {
                GD.PrintErr($"LoadScene error: Scene {name} doesn't exist in memory.");
                return;
            }

            Scenes[name].Load();
        }

        public void CreateScene(string name, CreateSettings settings = null)
        {
            if (settings == null) settings = DefaultSettings;

            if (!Scenes.ContainsKey(name))
            {
                GD.PrintErr($"CreateScene error: Scene {name} doesn't exist in memory.");
                return;
            }

            Scenes[name].Create(settings.SceneParent);
        }

        public void RemoveScene(string name)
        {
            if (!Scenes.ContainsKey(name))
            {
                GD.PrintErr($"RemoveScene error: Scene {name} doesn't exist in memory.");
                return;
            }

            Scenes[name].Remove();
            Scenes.Remove(name);
        }

        private void OnSceneCreated(string sceneName)
        {
            EmitSignal(SignalName.SceneCreated, sceneName);
        }

        private void OnSceneLoaded(string sceneName)
        {
            EmitSignal(SignalName.SceneLoaded, sceneName);
        }
    }
}

