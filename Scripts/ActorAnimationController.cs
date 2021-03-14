using System;
using Godot;

public class ActorAnimationController : AnimatedSprite
{
    private AnimationPlayer _animationPlayer;

    private Vector2 _startPosition;

    private Action _postAnimation = null;

    public override void _Ready()
    {
        _animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        _startPosition = Position;
    }

    public void PlayAnimation(AnimationState animation)
    {
        _animationPlayer.Play(animation.ToString());
    }

    public void PlayAnimation(AnimationState animation, Action after)
    {
        _postAnimation = after;
        PlayAnimation(animation);
    }

    public void OnAnimationPlayerFinished(string animationName)
    {
        Position = _startPosition;
        _postAnimation();
        _postAnimation = null;
    }
}

public enum AnimationState
{
    Stop,
    WalkRight,
    WalkDown,
    WalkLeft,
    WalkUp,
    StabRight,
    StabDown,
    StabLeft,
    StabUp,
    Death,
}
