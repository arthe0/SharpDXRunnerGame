using Engine;

namespace SharpDXRunnerGame.Content.Scripts.Track;

public class TileGameObject
{
    public TileGameObject(GameObject tile, GameObject[] obstacles)
    {
        this.tile = tile;
        this.obstacles = obstacles;
    }
    public GameObject tile;
    public GameObject[] obstacles;

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