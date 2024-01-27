using Godot;

namespace ComposerLib
{
    [GlobalClass]
    public partial class SceneSettings: Resource
    {
        public Node DefaultParent {
            get => _defaultParent;
            set
            {
                if (IsInstanceValid(value))
                    _defaultParent = value;
                else
                    GD.PrintErr($"Node {value} is invalid parent.");
            }
        }
        private Node _defaultParent = ((SceneTree)Engine.GetMainLoop()).Root;

        [Export]
        public bool InstantLoad {get; set;} = false;

        [Export]
        public bool InstantCreate {get; set;} = false;

        [Export]
        public bool DisableProcessing {get; set;} = false;

        [Export]
        public bool UseSubthreads {get; set;} = false;

        [Export]
        public ResourceLoader.CacheMode CacheMode {get; set;} = ResourceLoader.CacheMode.Reuse;
    }
}