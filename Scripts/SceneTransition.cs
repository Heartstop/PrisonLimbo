using Godot;
using System;

public class SceneTransition : ColorRect
{
    private AnimationPlayer _animationPlayer;
    private Action _postFade = null;
    public override void _Ready()
    {
        _animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
    }

    public void FadeIn(){
        _animationPlayer.Play("FadeIn");
    }

    public void FadeIn(Action postFade){
        _postFade = postFade;
        FadeIn();
    }
    
    public void FadeOut(){
        _animationPlayer.PlayBackwards("FadeIn");
    }

    public void FadeOut(Action postFade){
        _postFade = postFade;
        FadeOut();
    }

    private void OnAnimationFinished(string animationName) {
        if(_postFade != null){
            _postFade();
        }
        _postFade = null;
    }
}
