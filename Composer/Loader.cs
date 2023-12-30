using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;

namespace ComposerLib
{
    internal partial class Loader: Node
    {
        [Signal]
        internal delegate void LoaderStartedEventHandler(Scene scene);

        [Signal]
        internal delegate void LoaderLoadingUpdatedEventHandler(Scene scene, float progress);

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

            Godot.Collections.Array progress = new();

            switch (ResourceLoader.LoadThreadedGetStatus(CurrentLoadedScene.Path, progress))
            {
                case ResourceLoader.ThreadLoadStatus.InProgress:
                {
                    EmitSignal(SignalName.LoaderLoadingUpdated, (float)progress[0]);
                    break;
                }
                case ResourceLoader.ThreadLoadStatus.Loaded:
                {
                    var resource = (PackedScene)ResourceLoader.LoadThreadedGet(CurrentLoadedScene.Path);
                    EmitSignal(SignalName.LoaderFinished, CurrentLoadedScene, resource);
                    EndLoad();
                    break;
                }
                case ResourceLoader.ThreadLoadStatus.Failed: case ResourceLoader.ThreadLoadStatus.InvalidResource:
                {
                    EmitSignal(SignalName.LoaderFinished, CurrentLoadedScene);
                    EndLoad();
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

        private void EndLoad()
        {
            LoaderFinished -= CurrentLoadedScene.OnLoaded;
            CurrentLoadedScene = null;
        }
    }
}