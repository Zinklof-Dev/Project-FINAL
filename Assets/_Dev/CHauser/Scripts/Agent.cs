using UnityEngine;
using System.Collections.Generic;
using ZinklofDev.Utils.MathZ;


public class Agent : MonoBehaviour
{
    private enum State { Idle, Moving }

    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float rotationSpeed = 1f;
    [Header("Agent State")]
    [SerializeField] private State currentState = State.Idle;
    [Header("Indicies Of The Grid")]
    [SerializeField] private int startIndex;
    [SerializeField] private int goalIndex;
    [SerializeField] private int currentIndex;
    [SerializeField] private int nextIndex;
    private List<int> path = new List<int>();
    private int step = 1;

    private void OnDrawGizmos()
    {
        if(currentState != State.Moving)
            return;
        
    }

    private void Start()
    {

    }
  
    private void Update()
    {
        currentIndex = Mathf.Clamp(currentIndex, 0, GridSystem.points.Count - 1);
        startIndex = currentIndex;

        switch (currentState)
        {
            case State.Idle:
            transform.position = new Vector3(GridSystem.points[currentIndex].x, transform.position.y, GridSystem.points[currentIndex].y);
            break;
        
            case State.Moving:
            Move();
            break;
        }
        }

    /*private*/ public void StartNavigation()
    {
        if (startIndex == goalIndex)
            return;

        path = PathFinding.AStarPath(startIndex, goalIndex);
        if(path == null)
        {
            Debug.Log("Can't Move! No path found.");
            return;
        }
        currentState = State.Moving;
        step = 1;
    }

    private void Move()
    {
        nextIndex = path[step];

        Vector3 next = new Vector3(GridSystem.points[nextIndex].x, transform.position.y, GridSystem.points[nextIndex].y);
        transform.position = Vector3.MoveTowards(transform.position, next, moveSpeed * Time.deltaTime);

        Vector3 nextLookPosition = new Vector3(GridSystem.points[nextIndex].x, transform.position.y, GridSystem.points[nextIndex].y);
        Vector3 directionToTarget = nextLookPosition - transform.position;
        Quaternion goalRotation = Quaternion.LookRotation(directionToTarget, Vector3.up);
        if (Quaternion.Angle(transform.rotation, goalRotation) < 0.1f)
            transform.rotation = goalRotation;
        else
            transform.rotation = Quaternion.RotateTowards(transform.rotation, goalRotation, rotationSpeed * Time.deltaTime);

        if(Mathf.Sqrt(Vectors.SqrDist3f(transform.position, next)) < 0.01f)
        {
            transform.position = next;
            step++;

            if(step == path.Count)
            {
                currentIndex = goalIndex;
                currentState = State.Idle;
            }
        }
    }
}
