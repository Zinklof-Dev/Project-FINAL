using UnityEngine;
using System.Collections.Generic;
using ZinklofDev.Utils.MathZ;
using System;

public class Node
{
    public float g;
    public float h;
    public float f;
    public int gridIndex;
    public Node parent;
    public Vector2 position;

    public Node(float g, float h, int gridIndex, Node parent)
    {
        this.g = g;
        this.h = h;
        this.gridIndex = gridIndex;
        f = g + h;
        this.parent = parent;
        position = GridSystem.points[gridIndex];
    }
}

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
            AStarPath(positionIndex, positionGoalIndex);
            navigating = false;
        }
        if (snapToGrid)
        {
            positionIndex = Mathf.Clamp(positionIndex, 0, GridSystem.points.Count - 1);
            position = GridSystem.points[positionIndex];
            transform.position = new Vector3(position.x, 0, position.y);
        }
    }

    private void AStarPath(int startIndex, int goalIndex)
    {
        path.Clear();
        
        Vector2 goalPosition = GridSystem.points[goalIndex];
        
        List<Node> openList = new List<Node>();
        List<Node> closedList = new List<Node>();
        openList.Add(new Node(0, 0, startIndex, null));

        for (int i = 0; openList.Count != 0 && i < 1000000; i++)
        {
            Node q = openList[0];
            foreach (Node n in openList)
            {
                if(n.f < q.f)
                    q = n;
            }
            
            openList.Remove(q);
                
            foreach (Vector3 direction in directions)
            {
                if(Physics.Raycast(new Vector3(q.position.x, 1, q.position.y), direction, out RaycastHit hit, grid.tileSize))
                    continue;
                    
                Vector2 successorPosition = q.position + (direction * grid.tileSize);
                int sucessorIndex = GridSystem.points.IndexOf(successorPosition);
                if(sucessorIndex == -1)
                    continue;
                    
                Node successor = new Node(q.g + Mathf.Sqrt(Vectors.SqrDist2f(q.position, successorPosition)), DiagonalHeuristic(successorPosition, goalPosition), sucessorIndex, q);
                
                if(sucessorIndex == goalIndex)
                {
                    closedList.Add(sucessor);
                    break;
                }

                bool skip = false;
                
                foreach (Node checOpenkNode in openList)
                {
                    if (checkOpenNode.positionIndex == sucessorIndex && checkOpenNode.f < successor.f)
                        skip = true;
                }

                if (skip)
                    continue;
                    
                foreach (Node checkClosedNode in closedList)
                {
                    if (checkClosedNode.positionIndex == sucessorIndex && checkClosedNode.f < successor.f)
                        skip = true;
                }

                if (skip)
                    continue;
                
                openList.Add(successor);
            }
            closedList.Add(q);
        }
        
        Node next = null;
        int current = closedList.Count - 1;
        for (int 1 = 0; i < closedList.Count; i++)
        {
            next = closedList[current];
            path.Add(next.positionIndex);
            if (next.parent == null)
                break;
            current = next.parent.positionIndex;
        }
        path.Reverse();
    }

    private float DiagonalHeuristic(Vector2 successorPosition, Vector2 goalPosition)
    {
        float dx = Mathf.Abs(successorPosition.x - goalPosition.x);
        float dy = Mathf.Abs(successorPosition.y - goalPosition.y);
        float D = grid.tileSize;
        float D2 = Mathf.Sqrt(2) * D;
        return D * (dx + dy) + (D2 - 2 * D) * Mathf.Min(dx, dy);
    }
}
