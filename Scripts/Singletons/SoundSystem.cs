using Godot;
using System;
using System.Collections.Immutable;

public class SoundSystem : Node
{
    private static ImmutableArray<AudioStreamSample> _stabSamples = new AudioStreamSample[] { 
        GD.Load<AudioStreamSample>("Assets/Sounds/Stab1.wav"),
        GD.Load<AudioStreamSample>("Assets/Sounds/Stab2.wav"),
        GD.Load<AudioStreamSample>("Assets/Sounds/Stab3.wav"),
        GD.Load<AudioStreamSample>("Assets/Sounds/Stab4.wav"),
        GD.Load<AudioStreamSample>("Assets/Sounds/Stab5.wav"),
    }.ToImmutableArray();

    private static ImmutableArray<AudioStreamSample> _stepSamples = new AudioStreamSample[] { 
        GD.Load<AudioStreamSample>("Assets/Sounds/Step1.wav"),
        GD.Load<AudioStreamSample>("Assets/Sounds/Step2.wav"),
        GD.Load<AudioStreamSample>("Assets/Sounds/Step3.wav"),
        GD.Load<AudioStreamSample>("Assets/Sounds/Step4.wav"),
    }.ToImmutableArray();

    private static AudioStreamPlayer _stabSampler;

    private static AudioStreamPlayer _stepSampler;
    private static Random _random = new Random();
    public override void _Ready()
    {
        _stabSampler = new AudioStreamPlayer();
        _stepSampler = new AudioStreamPlayer();
        _stepSampler.Bus = "Effects";
        _stabSampler.Bus = "Effects";
        AddChild(_stabSampler);
        AddChild(_stepSampler);
    }

    public static void PlayStabSound() {
        _stabSampler.Stop();
        _stabSampler.Stream = _stabSamples[_random.Next(_stabSamples.Length)];
        _stabSampler.Play();

    }

    public static void PlayStepSound() {
        _stepSampler.Stop();
        _stepSampler.Stream = _stepSamples[_random.Next(_stepSamples.Length)];
        _stepSampler.Play();
    }

}
