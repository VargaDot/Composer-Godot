using Godot;
using System.Collections.Generic;

namespace ComposerLib
{
    internal partial class Loader: Node
    {
        [Signal]
        internal delegate void LoaderStartedEventHandler(Scene scene);

        [Signal]
        internal delegate void LoaderFinishedEventHandler(Scene scene, PackedScene resource=null);

        private static Queue<Scene> SceneQueue = new();
        private static Scene CurrentLoadedScene = null;

        internal static void AddToQueue(Scene scene)
        {
            SceneQueue.Enqueue(scene);
        }

        public override void _Process(double delta)
        {
            base._Process(delta);

            if (CurrentLoadedScene == null)
            {
                if (SceneQueue.Count > 0)
                {
                    BeginNewLoad();
                }
                else return;
            }

            switch (ResourceLoader.LoadThreadedGetStatus(CurrentLoadedScene.Path))
            {
                case ResourceLoader.ThreadLoadStatus.Loaded:
                {
                    var resource = (PackedScene)ResourceLoader.LoadThreadedGet(CurrentLoadedScene.Path);
                    EmitSignal(SignalName.LoaderFinished, CurrentLoadedScene, resource);
                    CurrentLoadedScene = null;
                    break;
                }
                case ResourceLoader.ThreadLoadStatus.Failed:
                {
                    EmitSignal(SignalName.LoaderFinished, CurrentLoadedScene);
                    CurrentLoadedScene = null;
                    break;
                }
            }
        }

        private void BeginNewLoad()
        {
            CurrentLoadedScene = SceneQueue.Dequeue();
            LoaderFinished += CurrentLoadedScene.OnLoaded;
            EmitSignal(SignalName.LoaderStarted, CurrentLoadedScene);
            ResourceLoader.LoadThreadedRequest(CurrentLoadedScene.Path);
        }
    }
}