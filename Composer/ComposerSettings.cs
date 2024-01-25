using Godot;

namespace ComposerLib
{
    public partial class SceneSettings: Resource
    {
        private Node _sceneParent = ((SceneTree)Engine.GetMainLoop()).Root;
        public Node SceneParent {
            get => _sceneParent;
            set
            {
                if (IsInstanceValid(value))
                    _sceneParent = value;
                else
                    GD.PrintErr($"Node {value} is invalid parent.");
            }
        }

        public bool DisableProcessing {get; set;} = false;
        public bool InstantCreate {get; set;} = false;
        public bool UseSubthreads {get; set;} = false;
        public ResourceLoader.CacheMode CacheMode {get; set;} = ResourceLoader.CacheMode.Reuse;
        public bool InstantLoad {get; set;} = false;
    }
}