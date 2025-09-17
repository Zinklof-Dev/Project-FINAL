using System;
using System.Collections.Generic;
using UnityEngine;
using ZinklofDev.Utils.MathZ;

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

public class PathFinding
{
    private static Vector3[] directions = { new Vector3(0, 0, -1), new Vector3(0, 0, 1), new Vector3(-1, 0, 0), new Vector3(1, 0, 0), new Vector3(1, 0, 1), new Vector3(-1, 0, 1), new Vector3(1, 0, -1), new Vector3(-1, 0, -1) };

    public static List<int> AStarPath(int startIndex, int goalIndex)
    {
        List<int> path = new List<int>();
        bool pathFound = false;

        DateTime startTime = DateTime.Now;

        Vector2 goalPosition = GridSystem.points[goalIndex];

        List<Node> openList = new List<Node>();
        List<Node> closedList = new List<Node>();
        openList.Add(new Node(0, 0, startIndex, null));

        for (int i = 0; openList.Count != 0 && i < 1000000; i++)
        {
            //await Task.Delay(delay);
            Node q = openList[0];
            foreach (Node n in openList)
            {
                if (n.f < q.f)
                    q = n;
            }

            openList.Remove(q);

            foreach (Vector3 direction in directions)
            {
                //await Task.Delay(delay);
                if (Physics.Raycast(new Vector3(q.position.x, 1, q.position.y), direction, out RaycastHit hit, GridSystem.staticTileSize))
                    continue;

                Vector2 successorPosition = q.position + (new Vector2(direction.x, direction.z) * GridSystem.staticTileSize);
                int sucessorIndex = GridSystem.points.IndexOf(successorPosition);
                if (sucessorIndex == -1)
                    continue;

                Node successor = new Node(q.g + Mathf.Sqrt(Vectors.SqrDist2f(q.position, successorPosition)), DiagonalHeuristic(successorPosition, goalPosition), sucessorIndex, q);

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
            //await Task.Delay(delay);
            closedList.Add(q);
        }

            if (!pathFound)
            {
                Debug.Log("No path found.");
                return null;
            }

            Node current = closedList[closedList.Count - 1];

            for (int i = 0; i < closedList.Count && current != null; i++)
            {
                //await Task.Delay(delay);
                path.Add(current.gridIndex);
                current = current.parent;
            }

            if (path[path.Count - 1] != startIndex)
            {
                path.Add(startIndex);
            }

            path.Reverse();

            Debug.Log("Time it took to run in miliseconds: " + (DateTime.Now - startTime).TotalMilliseconds);

            return path;
    }

    private static float DiagonalHeuristic(Vector2 successorPosition, Vector2 goalPosition)
    {
        float dx = Mathf.Abs(successorPosition.x - goalPosition.x);
        float dy = Mathf.Abs(successorPosition.y - goalPosition.y);
        float D = GridSystem.staticTileSize;
        float D2 = Mathf.Sqrt(2) * D;
        return D * (dx + dy) + (D2 - 2 * D) * Mathf.Min(dx, dy);
    }
}
