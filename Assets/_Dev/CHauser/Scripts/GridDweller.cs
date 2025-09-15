using UnityEngine;
using System.Collections.Generic;
using ZinklofDev.Utils.MathZ;
using System;
using Unity.Mathematics;

public class GridDweller : MonoBehaviour
{
    [SerializeField] GridSystem grid;
    [SerializeField] public int positionIndex;
    [SerializeField] public int positionGoalIndex;
    [SerializeField] Vector2 position;
    [SerializeField] public bool snapToGrid = true;
    [SerializeField] public bool navigating = false;
    [SerializeField] public bool registerWithCommand;
    [SerializeField] private Vector3[] directions = { new Vector3(0, 0, -1),  new Vector3(0, 0, 1), new Vector3(-1, 0, 0), new Vector3(1, 0, 0), new Vector3(1, 0, 1), new Vector3(-1, 0, 1), new Vector3(1, 0, -1), new Vector3(-1, 0, -1) };
    [SerializeField] private List<int> path = new List<int>();


    private void OnDrawGizmos()
    {
        foreach(Vector3 direction in directions)
        {
            Gizmos.DrawRay(transform.position, direction);
        }

        int count = 0;

        foreach(int point in path)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(new Vector3(GridSystem.points[point].x, 1, GridSystem.points[point].y), 0.25f);
            if(count < path.Count - 1)
            {
                Gizmos.DrawLine(new Vector3(GridSystem.points[point].x, 1, GridSystem.points[point].y), new Vector3(GridSystem.points[path[count + 1]].x, 1, GridSystem.points[path[count + 1]].y));
            }
            count++;
        }
    }

    private void Start()
    {
        if(registerWithCommand)
            GridSystem.dwellers.Add(this);
    }

    private void Update()
    {
        if(navigating)
        {
            MakePath();
            navigating = false;
        }
        if (snapToGrid)
        {
            positionIndex = Mathf.Clamp(positionIndex, 0, GridSystem.points.Count - 1);
            position = GridSystem.points[positionIndex];
            transform.position = new Vector3(position.x, 0, position.y);
        }
    }

    private void MakePath()
    {
        DateTime startTime = DateTime.Now;
        // Defines a list of points that lead to dead ends
        List<int> deadEndPoints = new List<int>();
        // Clears the path so a new one can be made
        path = new List<int>();
        // Initilizes the start of the path to be the starting position
        path.Add(positionIndex);
        // Stores the point that is checked and then stored in the path
        int nextPoint = positionIndex;
        List<int> potentialPoints = new List<int>();

        for (int i = 0; i < grid.mapSize * grid.mapSize * 10; i++)
        {
            if (nextPoint == positionGoalIndex)
            {
                Debug.Log((DateTime.Now - startTime).TotalMilliseconds); 
                return;
            }
            
            potentialPoints = new List<int>();            
            
            foreach (Vector3 direction in directions)
            {
                if (Physics.Raycast(new Vector3(GridSystem.points[nextPoint].x, 1, GridSystem.points[nextPoint].y), direction, out RaycastHit hit, grid.tileSize))
                    continue;
                    
                Vector3 hitPoint = new Vector3 (GridSystem.points[nextPoint].x, 0, GridSystem.points[nextPoint].y) + (direction * grid.tileSize);
                int index = GridSystem.points.IndexOf(new Vector2(hitPoint.x, hitPoint.z));
                
                if (index == -1)
                    continue;

                potentialPoints.Add(index);
            }

            bool viablePointFound = false;
            bool first = true;

            foreach (int point in potentialPoints)
            {
                if (first)
                {
                    if (!path.Contains(point) && !deadEndPoints.Contains(point))
                    {
                        first = false;
                        nextPoint = point;
                        viablePointFound = true;
                        continue;
                    }
                }

                float dx1 = Mathf.Abs(GridSystem.points[point].x - GridSystem.points[positionGoalIndex].x);
                float dy1 = Mathf.Abs(GridSystem.points[point].y - GridSystem.points[positionGoalIndex].y);
                float dx2 = Mathf.Abs(GridSystem.points[nextPoint].x - GridSystem.points[positionGoalIndex].x);
                float dy2 = Mathf.Abs(GridSystem.points[nextPoint].y - GridSystem.points[positionGoalIndex].y);
                float D = grid.tileSize;
                float D2 = Mathf.Sqrt(grid.tileSize * grid.tileSize * 2);
                float h1 = D * (dx1 + dy1) + (D2 - 2 * D) * Mathf.Min(dx1, dy1);
                float h2 = D * (dx2 + dy2) + (D2 - 2 * D) * Mathf.Min(dx2, dy2);
                /*
                F(n) = g(n) + h(n)
                dx = abs(current_cell.x – goal.x)
                dy = abs(current_cell.y – goal.y)
 
                h = D * (dx + dy) + (D2 - 2 * D) * min(dx, dy)
                where D is length of each node (usually = 1) and D2 is diagonal distance between each node (usually = sqrt(2)). 
                */
                
                if (Mathf.Sqrt(Vectors.SqrDist2f(GridSystem.points[point], GridSystem.points[positionGoalIndex])) + h1 < Mathf.Sqrt(Vectors.SqrDist2f(GridSystem.points[nextPoint], GridSystem.points[positionGoalIndex])) + h2 && !path.Contains(point) && !deadEndPoints.Contains(point))
                {
                    nextPoint = point;
                    viablePointFound = true;
                }
            }
            if (viablePointFound)
                path.Add(nextPoint);

            else
            {
                path.RemoveAt(path.Count - 1);
                deadEndPoints.Add(nextPoint);
                if (path.Count == 0)
                {
                    Debug.Log("Goal is unreachable. No path found!");
                    return;
                }
                nextPoint = path[path.Count - 1];
            }
        }

        foreach(int point in path)
            Debug.Log(point);
    }
}
