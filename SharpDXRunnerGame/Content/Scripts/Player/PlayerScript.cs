using System.Windows.Input;
using Engine;
using Engine.BaseAssets.Components;
using LinearAlgebra;
using SharpDXRunnerGame.Content.Scripts.Enums;

namespace SharpDXRunnerGame.Content.Scripts.Player;

public class PlayerScript : BehaviourComponent
{
    [SerializedField]
    private double JumpForce = 5;

    private Rigidbody rigidBody;
    private TrackPositions PlayerPosition = TrackPositions.Center;
    
    
    public override void Start()
    {
        base.Start();

        rigidBody = GameObject.GetComponent<Rigidbody>();
        rigidBody.Material = new PhysicalMaterial(0.0, 0.0, CombineMode.Minimum, CombineMode.Maximum);
    }

    public override void Update()
    {
        base.Update();

        if (Input.IsKeyPressed(Key.Space))
        {
            rigidBody?.AddImpulse(new Vector3(0,0, JumpForce));
        }

        if (Input.IsKeyPressed(Key.Left))
        {
            MovePlayerTrackPosition(true);
        }
        
        if (Input.IsKeyPressed(Key.Right))
        {
            MovePlayerTrackPosition(false);
        }
        
    }

    private void MovePlayerTrackPosition(bool left)
    {
        if (left)
        {
            switch (PlayerPosition)
            {
                case TrackPositions.Left:
                    break;
                case TrackPositions.Center:
                    GameObject.Transform.Position += new Vector3(-5,0,0);
                    PlayerPosition = TrackPositions.Left;
                    break;
                case TrackPositions.Right:
                    GameObject.Transform.Position += new Vector3(-5,0,0);
                    PlayerPosition = TrackPositions.Center;
                    break;
            }
        }
        else
        {
            switch (PlayerPosition)
            {
                case TrackPositions.Left:
                    GameObject.Transform.Position += new Vector3(5,0,0);
                    PlayerPosition = TrackPositions.Center;
                    break;
                case TrackPositions.Center:
                    GameObject.Transform.Position += new Vector3(5,0,0);
                    PlayerPosition = TrackPositions.Right;
                    break;
                case TrackPositions.Right:
                    break;
            }
        }
    }
}