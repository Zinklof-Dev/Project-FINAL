using UnityEngine;
using System.Collections.Generic;
using ZinklofDev.Utils.MathZ;
using System;
using System.Threading.Tasks;

public class Node
{
    public float g;
    public float h;
    public float f;
    public int gridIndex;

    public Node(float g, float h, int gridIndex)
    {
        this.g = g;
        this.h = h;
        this.gridIndex = gridIndex;
        f = g + h;
    }
}

public class AStar
{
    
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
    [SerializeField] private int speedDelay = 0;


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

    private async void AStarPath(int startIndex, int goalIndex)
    {
        List<Node> openList = new List<Node>();
        List<Node> closedList = new List<Node>();
        openList.Add(new Node(0, 0, startIndex));

        while(openList.Count != 0)
        {
            Node q = new Node(Mathf.Infinity, Mathf.Infinity, 0);
            foreach (Node n in openList)
            {
                if(n.f < q.f)
                    q = n;
            }
        }
    }
}
