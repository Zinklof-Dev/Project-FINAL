using UnityEngine;
using System.Collections.Generic;


public class Agent : MonoBehaviour
{
    public enum State { Idle, Moving, Battle }

    [SerializeField] private float moveDuration = 0.25f; // seconds per grid point
    [SerializeField] private float rotationDuration = 0.25f; // seconds to fully rotate
    [SerializeField] private float rotationDurationToLookAtEnemy = 0.25f;
    [Header("Agent State")]
    [SerializeField] public State currentState = State.Idle;
    [Header("Indicies Of The Grid")]
    [SerializeField] private int startIndex;
    [SerializeField] public int goalIndex;
    [SerializeField] public int currentIndex;

    private List<int> path = new List<int>();
    private List<int> trimmedPath = new List<int>();
    private int step = 1;
    private float gridPointsToMove = 0;
    private float movementT = 0;
    private float rotationT = 0;
    private Vector3 nextPosition = new Vector3();
    private Vector3 prevPosition = new Vector3();
    private Quaternion nextRotation = Quaternion.identity;
    private Quaternion prevRotation = Quaternion.identity;
    private Vector3 directionToTarget = new Vector3();
    private float sqrtOfTwo = 1.4142135624f;

    private bool rotatingToEnemy = false;
    private Vector3 directionToEnemy = new Vector3();
    private Quaternion goalRotationToLookAtEnemy  = Quaternion.identity;
    private Quaternion startRotationToLookAtEnemy = Quaternion.identity;
    private float enemyLookatT = 0;

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

                Rotate();
                Move();

                break;
        }

        if (rotatingToEnemy)
            RotateToEnemy();
    }

    public void StartNavigation()
    {
        if (startIndex == goalIndex)
            return;

        path = PathFinding.AStarPath(startIndex, goalIndex, 0.5f);

        if(path == null)
        {
            Debug.Log("Can't Move! No path found.");
            return;
        }

        trimmedPath = PathFinding.TrimPath(path);

        step = 1;
        currentState = State.Moving;
        StartNewMove();
    }

    private void Move()
    {
        if (movementT >= 1)
        {
            if (step + 1 == trimmedPath.Count)
            {
                currentIndex = goalIndex;
                currentState = State.Idle;
                return;
            }

            step++;
            StartNewMove();
            return;
        }

        movementT += Time.deltaTime / (moveDuration * gridPointsToMove);
        //float smoothedT = Mathf.SmoothStep(0, 1, movementT);
        transform.position = Vector3.Lerp(prevPosition, nextPosition, movementT);
    }

    private void Rotate()
    {
        if (rotationT >= 1)
                return;

        rotationT += Time.deltaTime / rotationDuration;
        transform.rotation = Quaternion.Slerp(prevRotation, nextRotation, rotationT);
    }

    private void StartNewMove()
    {
        movementT = 0;
        rotationT = 0;
        prevPosition = new Vector3(GridSystem.instance.points[trimmedPath[step - 1]].x, transform.position.y, GridSystem.instance.points[trimmedPath[step - 1]].y);
        nextPosition = new Vector3(GridSystem.instance.points[trimmedPath[step]].x, transform.position.y, GridSystem.instance.points[trimmedPath[step]].y);
        prevRotation = transform.rotation;
        directionToTarget = (nextPosition - transform.position).normalized;
        nextRotation = Quaternion.LookRotation(directionToTarget, Vector3.up);

        gridPointsToMove = path.IndexOf(trimmedPath[step]) - path.IndexOf(trimmedPath[step - 1]);

        if (Mathf.Abs(directionToTarget.x) > 0 && Mathf.Abs(directionToTarget.z) > 0)
        {
            gridPointsToMove *= sqrtOfTwo;
        }
    }

    public void LookAtEnemy (Actor enemy)
    {
        directionToEnemy = (enemy.transform.position - transform.position).normalized;
        startRotationToLookAtEnemy = transform.rotation;
        goalRotationToLookAtEnemy = Quaternion.LookRotation(directionToEnemy, Vector3.up);
        enemyLookatT = 0;
        rotatingToEnemy = true;
    }

    private void RotateToEnemy()
    {
        if (BattleCameraMover.instance.moving)
            return;

        if (enemyLookatT >= 1)
        {
            rotatingToEnemy = false;
            return;
        }

        enemyLookatT += Time.deltaTime / rotationDurationToLookAtEnemy;
        transform.rotation = Quaternion.Slerp(startRotationToLookAtEnemy, goalRotationToLookAtEnemy, enemyLookatT);
    }
}
