using System;
using Engine;
using Engine.BaseAssets.Components;

namespace Game1.Content.Scripts
{
    public class TestComponent : BehaviourComponent
    {
        [SerializedField] private double Value = 1;
        
        public override void Start()
        {
            base.Start();
            Logger.Log(LogType.Info, $"Component started with Value = {Value}");           
        }
    }
}

