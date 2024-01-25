using System;
using System.Collections.Generic;
using System.Linq;
using Engine;
using Engine.BaseAssets.Components;
using Engine.BaseAssets.Components.Colliders;
using LinearAlgebra;
using SharpDXRunnerGame.Content.Scripts.Enums;

namespace SharpDXRunnerGame.Content.Scripts.Track;

public class TrackGeneratorScript : BehaviourComponent
{
    [SerializedField] 
    private double tileSize = 20;
    [SerializedField] 
    private double initialSpeed = 10;
    [SerializedField]
    private double maxSpeed = 100;
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
    private Material obstacleMaterial;

    private double targetPositionY = 0;
    private LinkedList<TileGameObject> tiles = [];

    private double currentSpeed; 
    
    public override void Start()
    {
        Rigidbody.GravitationalAcceleration = new Vector3(0, 0, -40);
        currentSpeed = initialSpeed;
        
        for (int i = 0; i < maxTiles; i++)
        {
            TileGameObject tile = CreateNewTile(new Vector3(0, farTileY - i * tileSize));
            tiles.AddFirst(tile);
        }
        
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
        
        TileGameObject firstTile = tiles.First();
        firstTile.Destroy();
        tiles.RemoveFirst();

        TileGameObject lastTile = CreateNewTile(new Vector3(0, tiles.Last.Value.tile.Transform.Position.y + tileSize, 0));
        tiles.AddLast(lastTile);
    }

    private TileGameObject CreateNewTile(Vector3 position)
    {
        GameObject tile = GameObject.Instantiate("Tile");
        
        MeshComponent tileMesh = tile.AddComponent<MeshComponent>();
        tileMesh.Model = tileModel;
        tileMesh.Materials[0] = tileMaterial;

        Rigidbody tileRigidBody = tile.AddComponent<Rigidbody>();
        tileRigidBody.IsStatic = true;

        CubeCollider tileCubeCollider = tile.AddComponent<CubeCollider>();
        tileCubeCollider.Size = new Vector3(2, 2, 0.1);
        
        tile.Transform.Position = position;
        tile.Transform.LocalScale = new Vector3(10, 10, 1);
        tile.Transform.Rotation = Quaternion.FromAxisAngle(Vector3.Forward, 3.14159);
        tile.Transform.SetParent(GameObject.Transform);

        GameObject[] AddObstacle()
        {
            Random rnd = new Random();
            int obstaclesNum = rnd.Next(0, 3);
            GameObject[] obstacles = new GameObject [obstaclesNum];
            
            TrackPositions[] rndPositions = { TrackPositions.Left, TrackPositions.Center, TrackPositions.Right };
            rnd.Shuffle(rndPositions);
            
            for (int i = 0; i < obstaclesNum; i++)
            {
                GameObject obstacle = GameObject.Instantiate("Obstacle");
                obstacle.Transform.Position = tile.Transform.Position;
                obstacle.Transform.Rotation = tile.Transform.Rotation;

                switch (rndPositions[i])
                {
                    case TrackPositions.Left:
                        obstacle.Transform.Position += new Vector3(-5,0,1);
                        break;
                    case TrackPositions.Center:
                        obstacle.Transform.Position += new Vector3(0,0,1);
                        break;
                    case TrackPositions.Right:
                        obstacle.Transform.Position += new Vector3(5, 0, 1);
                        break;
                }
                
                obstacle.Transform.SetParent(tile.Transform);
                MeshComponent obstacleMesh = obstacle.AddComponent<MeshComponent>();
                obstacleMesh.Model = obstacleModel;
                obstacleMesh.Materials[0] = obstacleMaterial;

                Rigidbody obstacleRigidBody = obstacle.AddComponent<Rigidbody>();
                obstacleRigidBody.IsStatic = true;

                CubeCollider obstacleCubeCollider = obstacle.AddComponent<CubeCollider>();
                obstacleCubeCollider.Size = new Vector3(2, 2, 2);
                
                obstacles[i] = obstacle;
            }

            return obstacles;
        }
        
        return new TileGameObject(tile, AddObstacle());
    }
}