using Godot;
using ComposerLib;

public partial class Main : Node2D
{
    public override async void _Ready()
    {
        base._Ready();

        var Composer = GetNode<Composer>("Composer");

        Composer.AddScene("MainMenu","res://src/main_menu.tscn");
        Composer.LoadScene("MainMenu");

        await ToSignal(Composer,Composer.SignalName.SceneLoaded);

        Composer.CreateScene("MainMenu", new ComposerSettings(){SceneParent = this});

        await ToSignal(GetTree().CreateTimer(2),SceneTreeTimer.SignalName.Timeout);

        Composer.RemoveScene("MainMenu");
    }
}
