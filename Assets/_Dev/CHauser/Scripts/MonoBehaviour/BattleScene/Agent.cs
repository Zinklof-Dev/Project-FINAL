using UnityEngine;
using System.Collections.Generic;
using ZinklofDev.Utils.MathZ;


public class Agent : MonoBehaviour
{
    public enum State { Idle, Moving, Battle }

    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float rotationSpeed = 1f;
    [Header("Agent State")]
    [SerializeField] public State currentState = State.Idle;
    [Header("Indicies Of The Grid")]
    [SerializeField] private int startIndex;
    [SerializeField] public int goalIndex;
    [SerializeField] public int currentIndex;
    [SerializeField] private int nextIndex;
    private List<int> path = new List<int>();
    private int step = 1;

    private Actor actor;

    private void OnDrawGizmos()
    {
        if (currentState != State.Moving)
            return;
        foreach (int i in path)
        { 
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(new Vector3(GridSystem.instance.points[i].x,1, GridSystem.instance.points[i].y), 0.25f);
        }
    }

    private void Start()
    {
        actor = GetComponent<Actor>();
    }
  
    private void Update()
    {
        currentIndex = Mathf.Clamp(currentIndex, 0, GridSystem.instance.points.Count - 1);
        startIndex = currentIndex;

        switch (currentState)
        {
            case State.Idle:

            transform.position = new Vector3(GridSystem.instance.points[currentIndex].x, transform.position.y, GridSystem.instance.points[currentIndex].y);

            break;
        
            case State.Moving:
                nextIndex = path[step];
                Rotate();
                Move();

            break;
        }
        }

    /*private*/ public void StartNavigation()
    {
        if (startIndex == goalIndex)
            return;

        path = PathFinding.AStarPath(startIndex, goalIndex, 1);
        if(path == null)
        {
            Debug.Log("Can't Move! No path found.");
            return;
        }
        path = PathFinding.TrimPath(path);
        currentState = State.Moving;
        step = 1;
    }

    public void Move()
    {
        Vector3 next = new Vector3(GridSystem.instance.points[nextIndex].x, transform.position.y, GridSystem.instance.points[nextIndex].y);
        

        transform.position = Vector3.MoveTowards(transform.position, next, moveSpeed * Time.deltaTime);

        if(Mathf.Sqrt(Vectors.SqrDist3f(transform.position, next)) < 0.01f)
        {
            step++;

            if (step == path.Count)
            {
                currentIndex = goalIndex;
                currentState = State.Idle;
            }

            transform.position = next;
        }
    }

    public void Rotate()
    {
        Vector3 nextLookPosition = new Vector3(GridSystem.instance.points[nextIndex].x, transform.position.y, GridSystem.instance.points[nextIndex].y);
        Vector3 directionToTarget = nextLookPosition - transform.position;
        Quaternion goalRotation = Quaternion.LookRotation(directionToTarget, Vector3.up);

        if (Quaternion.Angle(transform.rotation, goalRotation) < 0.1f)
            transform.rotation = goalRotation;
        else
            transform.rotation = Quaternion.RotateTowards(transform.rotation, goalRotation, rotationSpeed * Time.deltaTime);
    }
}
