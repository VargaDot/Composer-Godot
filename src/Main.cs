using Godot;
using ComposerLib;

public partial class Main : Node2D
{
    public override void _Ready()
    {
        base._Ready();

        var Composer = GetNode<Composer>("Composer");

        Composer.AddScene("MainMenu","res://src/main_menu.tscn", new(){
            InstantLoad = true,
            InstantCreate = true,
            SceneParent = this
        });
    }
}
