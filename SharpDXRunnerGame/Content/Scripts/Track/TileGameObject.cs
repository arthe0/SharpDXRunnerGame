using System;
using System.Windows.Forms.Design.Behavior;
using Engine;
using Engine.BaseAssets.Components;
using Engine.BaseAssets.Components.Colliders;
using LinearAlgebra;
using SharpDXRunnerGame.Content.Scripts.Enums;

namespace SharpDXRunnerGame.Content.Scripts.Track;

public class TileGameObject : Behavior
{
    public GameObject tile;
    private GameObject[] obstacles;

    public TileGameObject(Vector3 position, Model tileModel, Material tileMaterial, Model obstacleModel, Material obstacleUpMaterial, Material obstacleDownMaterial, bool withObstacle = true)
    {
        tile = GameObject.Instantiate("Tile");
        
        MeshComponent tileMesh = tile.AddComponent<MeshComponent>();
        tileMesh.Model = tileModel;
        tileMesh.Materials[0] = tileMaterial;

        Rigidbody tileRigidBody = tile.AddComponent<Rigidbody>();
        tileRigidBody.IsStatic = true;
        tileRigidBody.Material = new PhysicalMaterial(0.0, 0.0, CombineMode.Minimum, CombineMode.Minimum);

        CubeCollider tileCubeCollider = tile.AddComponent<CubeCollider>();
        tileCubeCollider.Size = new Vector3(2, 2, 0.1);
        
        tile.Transform.Position = position;
        tile.Transform.LocalScale = new Vector3(10, 10, 1);
        tile.Transform.Rotation = Quaternion.FromAxisAngle(Vector3.Forward, 3.14159);

        if (withObstacle)
        {
            AddObstacles(obstacleModel, obstacleUpMaterial, obstacleDownMaterial);
        } 
    }
    public TileGameObject(GameObject tile, GameObject[] obstacles)
    {
        this.tile = tile;
        this.obstacles = obstacles;
    }

    private void AddObstacles(Model obstacleModel, Material obstacleUpMaterial, Material obstacleDownMaterial)
    {
         Random rnd = new Random();
         int obstaclesNum = rnd.Next(0, 6);
         obstacles = new GameObject [obstaclesNum];
            
         ObstacleTrackPositions[] rndPositions =
         [
             ObstacleTrackPositions.LeftDown, ObstacleTrackPositions.CenterDown, ObstacleTrackPositions.RightDown,
             ObstacleTrackPositions.LeftUp, ObstacleTrackPositions.CenterUp, ObstacleTrackPositions.RightUp
         ];
         rnd.Shuffle(rndPositions);
            
         for (int i = 0; i < obstaclesNum; i++)
         {
             GameObject obstacle = GameObject.Instantiate("Obstacle"); 
             obstacle.Transform.Position = tile.Transform.Position;
             obstacle.Transform.Rotation = tile.Transform.Rotation;
                
             MeshComponent obstacleMesh = obstacle.AddComponent<MeshComponent>();
             obstacleMesh.Model = obstacleModel;

             switch (rndPositions[i])
             {
                 case ObstacleTrackPositions.LeftDown:
                     obstacle.Transform.Position += new Vector3(-5,0,0.5);
                     obstacleMesh.Materials[0] = obstacleDownMaterial;
                     break;
                 case ObstacleTrackPositions.CenterDown:
                     obstacle.Transform.Position += new Vector3(0,0,0.5);
                     obstacleMesh.Materials[0] = obstacleDownMaterial;
                     break;
                 case ObstacleTrackPositions.RightDown:
                     obstacle.Transform.Position += new Vector3(5, 0, 0.5);
                     obstacleMesh.Materials[0] = obstacleDownMaterial;
                     break;
                 case ObstacleTrackPositions.LeftUp:
                     obstacle.Transform.Position += new Vector3(-5,0,2.4);
                     obstacleMesh.Materials[0] = obstacleUpMaterial;
                     break;
                 case ObstacleTrackPositions.CenterUp:
                     obstacle.Transform.Position += new Vector3(0,0,2.4);
                     obstacleMesh.Materials[0] = obstacleUpMaterial;
                     break;
                 case ObstacleTrackPositions.RightUp:
                     obstacle.Transform.Position += new Vector3(5, 0, 2.4);
                     obstacleMesh.Materials[0] = obstacleUpMaterial;
                     break;
             }
                
             obstacle.Transform.SetParent(tile.Transform);

             Rigidbody obstacleRigidBody = obstacle.AddComponent<Rigidbody>();
             obstacleRigidBody.IsStatic = true;

             CubeCollider obstacleCubeCollider = obstacle.AddComponent<CubeCollider>();
             obstacleCubeCollider.Size = new Vector3(2, 2, 2);
             obstacleCubeCollider.IsTrigger = true;
             
                
             obstacles[i] = obstacle;
         }
    }

    public void Destroy()
    {
        tile.Destroy();
        foreach (var obstacle in obstacles)
        {
            obstacle.Destroy();
        }

        obstacles = [];
    }
}