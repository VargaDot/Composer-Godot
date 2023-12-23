using Godot;
using System.Collections.Generic;

namespace ComposerLib
{
    [GlobalClass]
    public partial class Composer : Node
    {
        [Signal]
        public delegate void SceneBeganLoadingEventHandler(Scene scene);

        [Signal]
        public delegate void SceneLoadedEventHandler(Scene scene);

        public Godot.Collections.Dictionary<string, Scene> Scenes = new();


        private Queue<Scene> SceneQueue = new();
        private Scene CurrentLoadedScene = null;

        private readonly ComposerSettings DefaultSettings = new(){SceneParent = ((SceneTree)Engine.GetMainLoop()).Root};

        public void AddScene(string name, string path)
        {
            if (Scenes.ContainsKey(name))
            {
                GD.PrintErr($"AddScene error: Scene {name} already exists in memory.");
                return;
            }

            var scene = new Scene(name, path);

            Scenes.Add(name,scene);
        }

        public void LoadScene(string name)
        {
            if (!Scenes.ContainsKey(name))
            {
                GD.PrintErr($"LoadScene error: Scene {name} doesn't exist in memory.");
                return;
            }

            SceneQueue.Enqueue(Scenes[name]);
        }

        public void CreateScene(string name, ComposerSettings settings = null)
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

        public override void _Process(double delta)
        {
            base._Process(delta);

            if (CurrentLoadedScene == null)
            {
                if (SceneQueue.Count > 0)
                {
                    CurrentLoadedScene = SceneQueue.Dequeue();
                    EmitSignal(SignalName.SceneBeganLoading, CurrentLoadedScene);
                    ResourceLoader.LoadThreadedRequest(CurrentLoadedScene.Path);
                }
            }
            else
            {
                if (ResourceLoader.LoadThreadedGetStatus(CurrentLoadedScene.Path) == ResourceLoader.ThreadLoadStatus.Loaded)
                {
                    EmitSignal(SignalName.SceneLoaded, CurrentLoadedScene);
                    Scenes[CurrentLoadedScene.InternalName].Resource = (PackedScene)ResourceLoader.LoadThreadedGet(CurrentLoadedScene.Path);
                    CurrentLoadedScene = null;
                }
            }
        }
    }
}

