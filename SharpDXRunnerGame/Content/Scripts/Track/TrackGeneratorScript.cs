using System;
using System.Collections.Generic;
using System.Linq;
using Engine;
using Engine.BaseAssets.Components;
using Engine.BaseAssets.Components.Colliders;
using LinearAlgebra;
using Microsoft.VisualBasic.Logging;
using SharpDXRunnerGame.Content.Scripts.Enums;

namespace SharpDXRunnerGame.Content.Scripts.Track;

public class TrackGeneratorScript : BehaviourComponent
{
    [SerializedField] 
    private double tileSize = 20;
    [SerializedField] 
    private double initialSpeed = 10;
    [SerializedField]
    private double maxSpeed = 30;
    [SerializedField]
    private double maxSpeedProgress = 1000;
    [SerializedField] 
    private int maxTiles = 15;
    [SerializedField] 
    private double farTileY = 160;

    [SerializedField] 
    private Model tileModel;
    [SerializedField] 
    private Material tileMaterial;
    
    [SerializedField] 
    private Model obstacleModel;
    [SerializedField] 
    private Material obstacleDownMaterial;
    [SerializedField] 
    private Material obstacleUpMaterial;

    private double targetPositionY = 0;
    private LinkedList<TileGameObject> tiles = [];

    private double currentSpeed;
    private int progress = 0;
    
    public override void Start()
    {
        Rigidbody.GravitationalAcceleration = new Vector3(0, 0, -40);
        currentSpeed = initialSpeed;
        
        for (int i = 0; i < maxTiles; i++)
        {
            TileGameObject tile;
            tile = i > 5
                ? CreateNewTile(new Vector3(0, farTileY - i * tileSize), false)
                : CreateNewTile(new Vector3(0, farTileY - i * tileSize), true);
            tiles.AddFirst(tile);
        }
        
        Logger.Log(LogType.Info, $"Tiles size is {tiles.Count}");
        targetPositionY = -tileSize;
    }

    public override void Update()
    {
        double deltaY = Time.DeltaTime * currentSpeed;

        GameObject.Transform.Position -= new Vector3(0, deltaY, 0);

        if (GameObject.Transform.Position.y <= targetPositionY)
        {
            SpawnNextTile();
        }
    }

    private void SpawnNextTile()
    {
        targetPositionY = GameObject.Transform.Position.y - tileSize;
        
        TileGameObject lastTile = CreateNewTile(new Vector3(0, tiles.Last.Value.tile.Transform.Position.y + tileSize, 0));
        tiles.AddLast(lastTile);
        
        TileGameObject firstTile = tiles.First();
        tiles.RemoveFirst();
        firstTile.Destroy();

        progress++;
        
        // Linear interpolation
        double a = progress / maxSpeedProgress;
        currentSpeed = initialSpeed * (1 - a) + maxSpeed * a;
        
        Logger.Log(LogType.Info, $"Current speed is {currentSpeed}");
    }

    private TileGameObject CreateNewTile(Vector3 position, bool withObstacle = true)
    {
        TileGameObject tile = new TileGameObject(position, tileModel, tileMaterial, obstacleModel, obstacleUpMaterial,
            obstacleDownMaterial, withObstacle);
        tile.tile.Transform.SetParent(GameObject.Transform);
        
        return tile;
    }
}