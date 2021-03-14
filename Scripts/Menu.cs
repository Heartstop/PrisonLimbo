using Godot;

public class Menu : Node
{
    private SceneTransition _sceneTransition;
    private HSlider _musicSlider;
    private HSlider _effectSlider;
    public override void _Ready()
    {
        base._Ready();
        _musicSlider = GetNode<HSlider>("MarginContainer/VboxSliders/HboxMusic/HSlider");
        _effectSlider = GetNode<HSlider>("MarginContainer/VboxSliders/HboxMusic/HSlider");

        _effectSlider.Value = AudioServer.GetBusVolumeDb(1);
        _musicSlider.Value = AudioServer.GetBusVolumeDb(2);
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
    
    public void EffectValueChanged(float value) => AudioServer.SetBusVolumeDb(1, value);
    public void MusicValueChanged(float value) => AudioServer.SetBusVolumeDb(2, value);


}
