using System.Collections.Generic;
using ZinklofDev.ConsoleV2;
using UnityEngine;

public class GridSystem : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private bool drawGizmos = true;

    [Header("Map Attributes")]
    [SerializeField] public float tileSize = 2f;
    [SerializeField] public int mapSize = 10;
    [SerializeField] public float offsetX = 0;
    [SerializeField] public float offsetY = 0;
    [SerializeField] public List<Vector3> points = new List<Vector3>();

    public static GridSystem instance;

    private void Start()
    {
        instance = this;
        offsetX -= (mapSize / 2) * tileSize;
        offsetY -= (mapSize / 2) * tileSize;
        GenerateGrid(tileSize, mapSize);
    }

    private void OnDrawGizmos()
    {
        if(!drawGizmos) return;

        Gizmos.color = Color.green;

        foreach (Vector3 point in points)
        {
            Gizmos.DrawSphere(point, 0.5f);
        }
    }

    [Command("Generates Grid")]
    public static void GenerateGrid(float tileSize, int mapSize)
    {
        instance.points = new List<Vector3>();
        float x = 0;
        float y = 0;
        float x1 = instance.offsetX;
        float y1 = instance.offsetY;

        // Logic for creating grid points

        for(x = 0; x < mapSize; x++)
        {
            y1 = instance.offsetY;

            for(y = 0; y < mapSize; y++)
            {
                instance.points.Add(new Vector3(x1, 0, y1));
                y1 += tileSize;
            }

            x1 += tileSize;
        }
    }
}
