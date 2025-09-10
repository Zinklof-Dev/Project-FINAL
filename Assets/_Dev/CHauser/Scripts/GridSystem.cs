using System.Collections.Generic;
using ZinklofDev.ConsoleV2;
using UnityEngine;

public class GridSystem : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private bool drawGizmos = true;
    [Header("Map Attributes")]
    [SerializeField] public float tileSize = 2f;
    [SerializeField] private int mapSize = 10;
    [SerializeField] private float offsetX = 0;
    [SerializeField] private float offsetY = 0;
    private static float staticOffsetX;
    private static float staticOffsetY;
    [SerializeField] public static List<Vector2> points = new List<Vector2>();
    [Header("Grid Dwellers (Currently for Debugging)")]
    [SerializeField] public static List<GridDweller> dwellers = new List<GridDweller>();

    private void Start()
    {
        staticOffsetX = offsetX - (mapSize);
        staticOffsetY = offsetY - (mapSize);
        GenerateGrid(tileSize, mapSize);
    }

    private void OnDrawGizmos()
    {
        if(!drawGizmos) return;

        Gizmos.color = Color.green;

        foreach (Vector2 point in points)
        {
            Gizmos.DrawSphere(new Vector3(point.x, 0, point.y), 0.5f);
        }
    }

    [Command("Generates Grid")]
    public static void GenerateGrid(float tileSize, int mapSize)
    {
        points = new List<Vector2>();
        float x = 0;
        float y = 0;
        float x1 = staticOffsetX;
        float y1 = staticOffsetY;

        // Logic for creating grid points

        for(x = 0; x < mapSize; x++)
        {
            y1 = staticOffsetY;

            for(y = 0; y < mapSize; y++)
            {
                points.Add(new Vector2(x1, y1));
                y1 += tileSize;
            }

            x1 += tileSize;
        }
    }
    [Command("Moves Grid Dwellers")]
    public static void MoveDweller(int dwellerIndex, int gridIndex)
    {
        dwellers[dwellerIndex].positionIndex = gridIndex;
    }

    private void OnApplicationQuit()
    {
        points = new List<Vector2>();
        dwellers = new List<GridDweller>();
    }
}
