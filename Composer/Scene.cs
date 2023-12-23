using Godot;
using System;

namespace ComposerLib
{
    public partial class Scene : Node
    {
        public string InternalName {get; private set;}
        public string Path {get; set;}
        public PackedScene Resource {get; set;} = null;
        public Node Instance {get; private set;} = null;

        public Scene(string internalName, string path)
        {
            InternalName = internalName;
            Path = path;
        }

        public void Create(Node parent)
        {
            if (Resource == null)
            {
                GD.PrintErr($"Create error for scene {Path}: Resource doesn't exist in memory.");
                return;
            }

            Instance = Resource.Instantiate();
            parent.AddChild(Instance);
        }

        public void Remove()
        {
            Resource?.Dispose();
            Instance?.QueueFree();
        }
    }
}