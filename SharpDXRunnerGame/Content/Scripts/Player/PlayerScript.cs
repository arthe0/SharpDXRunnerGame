using System;
using System.Runtime.InteropServices.JavaScript;
using System.Threading;
using System.Windows.Input;
using Engine;
using Engine.BaseAssets.Components;
using LinearAlgebra;
using SharpDXRunnerGame.Content.Scripts.Enums;
using SharpDXRunnerGame.Content.Scripts.Track;

namespace SharpDXRunnerGame.Content.Scripts.Player;

public class PlayerScript : BehaviourComponent
{
    [SerializedField]
    private double jumpForce = 5;
    [SerializedField]
    private double crouchTime = 0.5;

    private Rigidbody rigidBody;
    private PlayerTrackPositions PlayerPosition = PlayerTrackPositions.Center;

    private bool isInAir = false;
    private bool isCrouching = false;

    private double crouchTimeElapsed = 0.0;
    
    public override void Start()
    {
        base.Start();

        rigidBody = GameObject.GetComponent<Rigidbody>();
        rigidBody.Material = new PhysicalMaterial(0.0, 0.0, CombineMode.Minimum, CombineMode.Minimum);

        Collider collider = GameObject.GetComponent<Collider>();
        collider.OnTriggerEnter += CheckCollision;
    }

    public override void Update()
    {
        base.Update();

        if (isCrouching)
        {
            crouchTimeElapsed += Time.DeltaTime;
            if (crouchTimeElapsed >= crouchTime)
            {
                ReleaseCrouch();
            }
        }

        if (Input.IsKeyPressed(Key.Space))
        {
            StartJump();
        }

        if (Input.IsKeyPressed(Key.Down))
        {
            StartCrouch();
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

    private void StartCrouch()
    {
        if(isInAir || isCrouching) return;
        
        isCrouching = true;
        GameObject.Transform.LocalScale = new Vector3(1, 1, 0.5);
    }

    private void ReleaseCrouch()
    {
        if (!isCrouching) return;
        
        GameObject.Transform.LocalScale = new Vector3(1, 1, 1);
        isCrouching = false;
        crouchTimeElapsed = 0.0;
    }

    private void StartJump()
    {
        if(isInAir) return;
        
        if(isCrouching) ReleaseCrouch();
        rigidBody?.AddImpulse(new Vector3(0,0, jumpForce));
        isInAir = true;
    }

    private void EndJump()
    {
        if (isInAir)
        {
            isInAir = false;
        }
    }

    private void CheckCollision(Collider sender, Collider other)
    {
        if (other.GameObject.GetComponent<TileChannel>() != null)
        {
            EndJump();
        }

        if (other.GameObject.GetComponent<ObstacleChannel>() != null)
        {
            Logger.Log(LogType.Warning, "HIT!");
            SceneManager.LoadSceneByName("Main");
        }
    }

    private void MovePlayerTrackPosition(bool left)
    {
        if (left)
        {
            switch (PlayerPosition)
            {
                case PlayerTrackPositions.Left:
                    break;
                case PlayerTrackPositions.Center:
                    GameObject.Transform.Position = new Vector3(-5,-45,GameObject.Transform.Position.z);
                    PlayerPosition = PlayerTrackPositions.Left;
                    break;
                case PlayerTrackPositions.Right:
                    GameObject.Transform.Position = new Vector3(0,-45,GameObject.Transform.Position.z);
                    PlayerPosition = PlayerTrackPositions.Center;
                    break;
            }
        }
        else
        {
            switch (PlayerPosition)
            {
                case PlayerTrackPositions.Left:
                    GameObject.Transform.Position = new Vector3(0,-45,GameObject.Transform.Position.z);
                    PlayerPosition = PlayerTrackPositions.Center;
                    break;
                case PlayerTrackPositions.Center:
                    GameObject.Transform.Position = new Vector3(5,-45,GameObject.Transform.Position.z);
                    PlayerPosition = PlayerTrackPositions.Right;
                    break;
                case PlayerTrackPositions.Right:
                    break;
            }
        }
    }
}