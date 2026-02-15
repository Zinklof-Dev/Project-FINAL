using UnityEngine;
using System.Collections.Generic;
using ZinklofDev.Utils.MathZ;

public class EnemyAI : MonoBehaviour
{
    public static EnemyAI instance;

    public enum EnemyState { Thinking, Moving, Attacking }
    public EnemyState state = EnemyState.Thinking;

    private Actor playerTarget;
    bool targetFound = false;
    public Actor currentTurnActor;
   // [SerializeField] List<Actor> evaluatedTargets = new List<Actor>();
    List<Actor> attackedTargets = new List<Actor>();
    List<int> currentBestPath = null;
    float currentBestDistance = Mathf.Infinity;
    List<Actor> evaluatedTargets = new List<Actor>();


    private void Start()
    {
        instance = this;
    }

    public void EnemyTurn()
    {
        switch (state)
        {
            case EnemyState.Thinking:

                Think();

                break;

            case EnemyState.Moving:

                Moving();

                break;

            case EnemyState.Attacking:

                break;

        }

    }

    private void Think()
    {
        FindTarget();

        if (targetFound)
        {
            //Debug.Log("[EnemyAI] " + currentTurnActor.actorName + " Selected Target Actor: " + playerTarget.actorName);
        }
        else
        {
            Debug.Log("[EnemyAI] " + currentTurnActor.actorName + " Selected No Target Actor");
            evaluatedTargets.Clear();
            TurnManager.instance.UpdateEnemyActorTurn();
            return;
        }

        int? goalIndex = FindIfGoalIndexHasView();

        if (goalIndex == null)
        {
            evaluatedTargets.Add(playerTarget);
            return;
        }
        
        state = EnemyState.Moving;
        currentTurnActor.agent.goalIndex = (int) goalIndex;
        currentTurnActor.agent.StartNavigation();
        evaluatedTargets.Clear();
        Debug.Log("[EnemyAI] " + currentTurnActor.actorName + " Selected Target Actor: " + playerTarget.actorName);
    }

    private void FindTarget()
    {
        playerTarget = null;
        targetFound = false;
        currentBestPath = null;
        currentBestDistance = Mathf.Infinity;

        foreach (Actor actor in ActorManager.instance.partyMemberActors)
        {
            if(evaluatedTargets.Contains(actor))
            {
                continue;
            }

            List<int> pathToActor = PathFinding.AStarPath(currentTurnActor.agent.currentIndex, actor.agent.currentIndex, 0.5f);

            if (pathToActor == null)
            {
                evaluatedTargets.Add(actor);
                continue; 
            }
            
            if (currentBestPath == null)
            {
                targetFound = true;
                currentBestPath = pathToActor;
                playerTarget = actor;
                currentBestDistance = pathToActor.Count - (int)currentTurnActor.range;
                currentBestDistance = Mathf.Clamp(currentBestDistance, 0, pathToActor.Count);
                continue;
            }

            int distanceToIdealRange = pathToActor.Count - (int) currentTurnActor.range;
            distanceToIdealRange = Mathf.Clamp(distanceToIdealRange, 0, pathToActor.Count);

            if (distanceToIdealRange < currentBestDistance)
            {
                targetFound = true;
                currentBestPath = pathToActor;
                playerTarget = actor;
                currentBestDistance = distanceToIdealRange;
                continue;
            }
       }
    }

    private int? FindIfGoalIndexHasView()
    {
        int goalIndex = currentBestPath[(int) Mathf.Clamp((currentBestPath.Count - 1) - currentTurnActor.range, 0, (currentBestPath.Count - 1))];
        Vector3 goalPosition = new Vector3(GridSystem.instance.points[goalIndex].x, 0.5f, GridSystem.instance.points[goalIndex].y);
        Vector3 playerPosition = new Vector3(GridSystem.instance.points[playerTarget.agent.currentIndex].x, 0.5f, GridSystem.instance.points[playerTarget.agent.currentIndex].y);
        Vector3 directionToPlayerFromGoal = (playerPosition - goalPosition).normalized;

        RaycastHit hit;
        if (Physics.Raycast(goalPosition, directionToPlayerFromGoal, out hit))
        {
            if(hit.transform.gameObject != playerTarget.gameObject) 
                return null;
        }

        return goalIndex;
    }

    private void Moving()
    {
        if (currentTurnActor.agent.currentState == Agent.State.Idle)
        {   
            // TODO- chage it to attacking instead, I just have it think again because the attacking logic isn't set up yet
            state = EnemyState.Thinking; // EnemyState.Attacking
            TurnManager.instance.UpdateEnemyActorTurn();
        }
    }
}
