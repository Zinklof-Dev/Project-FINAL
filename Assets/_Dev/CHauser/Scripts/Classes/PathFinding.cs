using System.Collections.Generic;
using System;
using UnityEngine;
using Bastion.Utils.MathZ;

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
        position = GridSystem.instance.points[gridIndex];
    }
}

public class PathFinding
{
    public static Vector3[] directions = { new Vector3(-1, 0, 1), new Vector3(1, 0, -1), new Vector3(-1, 0, -1), new Vector3(1, 0, 1), new Vector3(1, 0, 0),  new Vector3(0, 0, -1), new Vector3(0, 0, 1), new Vector3(-1, 0, 0) };

    public static List<int> AStarPath(int startIndex, int goalIndex, float yPositionToCheck)
    {
        //DateTime startTime = DateTime.Now;

        List<int> path = new List<int>();
        bool pathFound = false;

        Vector2 goalPosition = GridSystem.instance.points[goalIndex];

        List<Node> openList = new List<Node>();
        List<Node> closedList = new List<Node>();
        openList.Add(new Node(0, 0, startIndex, null));

        for (int i = 0; openList.Count != 0 && i < 1000000; i++)
        {
            Node q = openList[0];
            foreach (Node n in openList)
            {
                if (n.f < q.f)
                    q = n;
            }

            openList.Remove(q);

            foreach (Vector3 direction in directions)
            {
                if (Physics.Raycast(new Vector3(q.position.x, yPositionToCheck, q.position.y), direction, out RaycastHit hit, GridSystem.instance.tileSize * Mathf.Sqrt(2)))
                    continue;

                Vector2 successorPosition = q.position + (new Vector2(direction.x, direction.z) * GridSystem.instance.tileSize);
                int sucessorIndex = GridSystem.instance.points.IndexOf(successorPosition);
                if (sucessorIndex == -1)
                    continue;

                Node successor = new Node(q.g + Mathf.Sqrt(Vectors.SqrDist2f(q.position, successorPosition)), DiagonalHeuristic(successorPosition, goalPosition), sucessorIndex, q);
                //Node successor = new Node(q.g + Vectors.SqrDist2f(q.position, successorPosition), EuclidianHeuristic(successorPosition, goalPosition), sucessorIndex, q);

                if (sucessorIndex == goalIndex)
                {
                    closedList.Add(successor);
                    pathFound = true;
                    break;
                }

                bool skip = false;

                foreach (Node checkOpenNode in openList)
                {
                    if (checkOpenNode.gridIndex == sucessorIndex && checkOpenNode.f < successor.f)
                        skip = true;
                }

                if (skip)
                    continue;

                foreach (Node checkClosedNode in closedList)
                {
                    if (checkClosedNode.gridIndex == sucessorIndex && checkClosedNode.f < successor.f)
                        skip = true;
                }

                if (skip)
                    continue;

                openList.Add(successor);
            }

            if (pathFound)
                break;

            closedList.Add(q);
        }

            if (!pathFound)
            {
                return null;
            }

            Node current = closedList[closedList.Count - 1];

            for (int i = 0; i < closedList.Count && current != null; i++)
            {
                path.Add(current.gridIndex);
                current = current.parent;
            }

            if (path[path.Count - 1] != startIndex)
            {
                path.Add(startIndex);
            }

            path.Reverse();

            //Debug.Log("Time it took to run in miliseconds: " + (DateTime.Now - startTime).TotalMilliseconds);

            return path;
    }

    private static float DiagonalHeuristic(Vector2 successorPosition, Vector2 goalPosition)
    {
        float dx = Mathf.Abs(successorPosition.x - goalPosition.x);
        float dy = Mathf.Abs(successorPosition.y - goalPosition.y);
        float D = GridSystem.instance.tileSize;
        float D2 = Mathf.Sqrt(2) * D;
        return D * (dx + dy) + (D2 - 2 * D) * Mathf.Min(dx, dy);
    }

    private static float EuclidianHeuristic(Vector2 successorPosition, Vector2 goalPosition)
    {
        // I know I don't really need a functiom for this, function is just here to keep it organized.
        return Vectors.SqrDist2f(successorPosition, goalPosition);
    }

    public static List<int> TrimPath(List<int> path)
    {
        List<int> result = new List<int>();

        Vector3 previousDirection = new Vector3();

        for(int i = 1; i < path.Count; i++)
        {
            Vector3 direction = GridSystem.instance.points[path[i]] - GridSystem.instance.points[path[i - 1]];

            if (direction == previousDirection)
            {
                previousDirection = direction;
                continue;
            }

            previousDirection = direction;
            result.Add(path[i - 1]);
        }

        if (result[result.Count - 1] != path[path.Count - 1])
            result.Add(path[path.Count - 1]);

        return result;
    }
}
