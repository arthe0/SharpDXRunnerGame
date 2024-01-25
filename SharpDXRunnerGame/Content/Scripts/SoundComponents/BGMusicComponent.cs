using Engine;
using Engine.BaseAssets.Components;

namespace SharpDXRunnerGame.Content.Scripts.SoundComponents;

public class BGMusicComponent : BehaviourComponent
{
    [SerializedField]
    private Sound music = null;

    public override void Start()
    {
        SoundCore.Play(music);
    }
}