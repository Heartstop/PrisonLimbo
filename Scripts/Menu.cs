using Godot;
using System;

public class Menu : Node
{
    private SceneTransition _sceneTransition;

    public override void _Ready()
    {
        base._Ready();
        _sceneTransition = GetNode<SceneTransition>("SceneTransition");
        _sceneTransition.FadeOut();
    }
    public override void _Input(InputEvent @event)
    {
        base._Input(@event);
        if(@event.IsActionPressed("ui_accept")){
            _sceneTransition.FadeIn(() => {
                GetTree().ChangeScene("Scenes/GameController.tscn");
            });
        }
    }
}
