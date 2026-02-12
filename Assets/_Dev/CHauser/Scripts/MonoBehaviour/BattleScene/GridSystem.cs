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
    [HideInInspector] public float internalOffsetX;
    [HideInInspector] public float internalOffsetY;
    [SerializeField] public List<Vector2> points = new List<Vector2>();

    public static GridSystem instance;

    private void Awake()
    {
        instance = this;
        internalOffsetX = offsetX - (mapSize / 2 * tileSize);
        internalOffsetY = offsetY - (mapSize / 2 * tileSize);
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
        instance.points = new List<Vector2>();
        float x = 0;
        float y = 0;
        float x1 = instance.internalOffsetX;
        float y1 = instance.internalOffsetY;

        // Logic for creating grid points

        for(x = 0; x < mapSize; x++)
        {
            y1 = instance.internalOffsetY;

            for(y = 0; y < mapSize; y++)
            {
                instance.points.Add(new Vector2(x1, y1));
                y1 += tileSize;
            }

            x1 += tileSize;
        }
    }
}
