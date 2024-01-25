using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Engine;
using Engine.BaseAssets.Components;
using SharpDXRunnerGame.Content.Scripts.GameManager;

namespace SharpDXRunnerGame.Content.Scripts.GUI;

public class HUDComponent : BehaviourComponent, INotifyPropertyChanged
{
    [SerializedField] 
    private GameObject runnerGameManager;
    private RunnerGameManager runnerGameManagerScript;
    
    private double scoreValue = 0.0;
    
    public double Score
    {
        get => scoreValue;
        set
        {
            scoreValue = Math.Round(value);
            OnPropertyChanged();
        }
    }
    
    public override void Start()
    {
        runnerGameManagerScript = runnerGameManager.GetComponent<RunnerGameManager>();
        if(runnerGameManagerScript == null) return;
        
        GraphicsCore.ViewportPanel.Dispatcher.Invoke(() =>
        {
            HUD hud = new HUD();
            GraphicsCore.ViewportPanel.Children.Add(hud);
            hud.DataContext = this;
        });
    }

    public override void Update()
    {
        Score = runnerGameManagerScript.score;
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}