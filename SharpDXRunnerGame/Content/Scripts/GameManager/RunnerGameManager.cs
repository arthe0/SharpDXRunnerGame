using Engine;
using Engine.BaseAssets.Components;

namespace SharpDXRunnerGame.Content.Scripts.GameManager;

public class RunnerGameManager : BehaviourComponent
{
    
    public double score = 0.0;
    private double scoreMultiplier = 10;

    public override void Start()
    {
        base.Start();
    }

    public override void Update()
    {
        score += Time.DeltaTime * scoreMultiplier;
    }
}