using Godot;
using System;

namespace ComposerLib
{
    public partial class CreateSettings: Resource
    {
        public Node SceneParent {get; set;} = null;
    }

    public partial class LoadSettings : CreateSettings
    {
        public bool InstantCreate {get; set;} = false;
    }

    public partial class AddSettings : LoadSettings
    {
        public bool InstantLoad {get; set;} = false;
    }

    public partial class ComposerSettings: AddSettings
    {

    }
}