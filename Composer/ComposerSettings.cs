using Godot;
using System;

namespace ComposerLib
{
    public class CreateSettings
    {
        public Node SceneParent {get; set;} = null;
    }

    public class LoadSettings : CreateSettings
    {
        public bool InstantCreate {get; set;} = false;
    }

    public class AddSettings : LoadSettings
    {
        public bool InstantLoad {get; set;} = false;
    }

    public class ComposerSettings: AddSettings
    {

    }
}