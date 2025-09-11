using UnityEngine;
using System.Collections.Generic;
using ZinklofDev.Utils.MathZ;

public class GridDweller : MonoBehaviour
{
/*
    [SerializeField] GridSystem grid;
    [SerializeField] public int positionIndex;
    [SerializeField] public int positionGoalIndex;
    [SerializeField] Vector2 position;
    [SerializeField] public bool snapToGrid = true;
    [SerializeField] public bool navigating = false;
    [SerializeField] public bool registerWithCommand;
    [SerializeField] private Vector3[] directions = {new Vector3(1, 0, 0), new Vector3(1, 0, 1), new Vector3(0, 0, 1), new Vector3(-1, 0, 1), new Vector3(1, 0, -1), new Vector3(-1, 0, 0), new Vector3(-1, 0, -1), new Vector3(0, 0, -1) };
    [SerializeField] private List<int> path = new List<int>();


    private void OnDrawGizmos()
    {
        foreach(Vector3 direction in directions)
        {
            Gizmos.DrawRay(transform.position, direction);
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
        List<int> deadEndPoints = new List<int>();
        List<int> path = new List<int>();
        int nextPoint = positionIndex;

        for (int i = 0; i < grid.mapSize * grid.mapSize; i++)
        {
            List<int> potentialPoints = new List<int>();
            
            if (nextPoint == positionGoalIndex)
            {
                foreach(int point in path)
                    Debug.Log(point);
                return;
            }

            foreach (Vector3 direction in directions)
            {
                if (Physics.Raycast(new Vector3(GridSystem.points[nextPoint].x, 1, GridSystem.points[nextPoint].y), direction, out RaycastHit hit, grid.tileSize))
                    continue;

                Vector3 hitPoint = new Vector3(GridSystem.points[nextPoint].x, 0, GridSystem.points[nextPoint].y) + (direction * (grid.tileSize - 0.1f));
                Debug.Log(hitPoint);
                Vector3 closestGridPoint = new Vector3(GridSystem.points[nextPoint].x, 0, GridSystem.points[nextPoint].y);
                int closestGridIndex = nextPoint;
                int j = 0;

                foreach (Vector2 point in GridSystem.points)
                {
                    if (Vectors.SqrDist3f(hitPoint, new Vector3(point.x, 0f, point.y)) < Vectors.SqrDist3f(closestGridPoint, new Vector3(point.x, 0f, point.y)))
                    {
                        closestGridPoint = hitPoint;
                        closestGridIndex = j;
                    }
                    j++;
                }

                potentialPoints.Add(closestGridIndex);
            }

            bool viablePointFound = false;
            foreach (int point in potentialPoints)
            {
                if (Vectors.SqrDist2f(GridSystem.points[point], GridSystem.points[positionGoalIndex]) < Vectors.SqrDist2f(GridSystem.points[nextPoint], GridSystem.points[positionGoalIndex]) && !path.Contains(point) && !deadEndPoints.Contains(point))
                {
                    nextPoint = point;
                    viablePointFound = true;
                }
            }
            if(viablePointFound)
                path.Add(nextPoint);
            else
            {
                path.RemoveAt(path.Count - 1);
                deadEndPoints.Add(nextPoint);
                nextPoint = path[path.Count - 1];
            }
        }

        foreach(int point in path)
            Debug.Log(point);
    }
    */
}
