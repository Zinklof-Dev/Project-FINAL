using UnityEngine;
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
            List<int> potentialPoints = new List<int>();
            
            foreach (Vector3 direction in directions)
            {
                if(Physics.Raycast(new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), direction, out RaycastHit hit, grid.tileSize))
                    continue;
                    
                Vector3 hitPoint = transform.position + (direction * grid.tileSize);
                Vector3 closestGridPoint;
                int closestGridIndex;
                int i = 0;
                
                foreach(Vector2 point in GridSystem.points)
                {
                    if(Vectors.SqrDist3f(hitPoint, new Vector3(point.x, 0f, point.y)) < Vectors.SqrDist3f(closestGridPoint, new Vector3(point.x, 0f, point.y)))
                    {
                        closestGridPoint = hitPoint;
                        closestGridIndex = i;
                    }
                    i++;
                }

                potentialPoints.Add(closestGridIndex);
            }
            
            foreach (int point in potentialPoints)
            {
                if(Vectors.SqrDist2f(GridSystem[point], GridSystem[positionGoalIndex]) < Vectors.SqrDist2f(GridSystem[positionIndex], GridSystem[positionGoalIndex]))
                    positionIndex = point;
            }
        }
        if (snapToGrid)
        {
            positionIndex = Mathf.Clamp(positionIndex, 0, GridSystem.points.Count - 1);
            position = GridSystem.points[positionIndex];
            transform.position = new Vector3(position.x, 0, position.y);
        }
    }
    */
}
