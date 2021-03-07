using Godot;
using System;

public class ActorAnimationController : AnimatedSprite
{
    [Signal]
    public delegate void AnimationFinished();
    
    private Vector2 _startPostistion;
    private AnimationPlayer _animationPlayer = null!;
    public override void _Ready()
    {
        _animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        _startPostistion = Position;
    }

    public void PlayAnimation(AnimationState animation){
        GD.Print(animation.ToString());
        _animationPlayer.Play(animation.ToString());;
    }

    public void OnAnimationPlayerFinished(String animationName){

        Position = _startPostistion;
        EmitSignal(nameof(AnimationFinished));
    }
}
public enum AnimationState {
    Stop,
    Right,
    Down,
    Left,
    Up,
}