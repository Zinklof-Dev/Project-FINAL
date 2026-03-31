using UnityEngine;
using System.Collections.Generic;
using Bastion.Utils.MathZ;
using Unity.VisualScripting;

public class EnemyAI : MonoBehaviour
{
    public static EnemyAI instance;

    public enum EnemyState { Thinking, Moving, Attacking }
    public EnemyState state = EnemyState.Thinking;
    public int attackAPCost = 5;

    private Actor playerTarget;
    bool targetFound = false;
    public Actor currentTurnActor;
    List<int> currentBestPath = null;
    int currentBestIndex;
    bool adjacentFound = false;


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

                Attacking();

                break;

        }

    }

    private void Think()
    {
        FindTarget();

        if(adjacentFound)
        {
            adjacentFound = false;
            return;
        }

        if (!targetFound)
        {
            Debug.Log("[EnemyAI] " + currentTurnActor.actorName + " Selected No Target Actor");
            TurnManager.instance.UpdateEnemyActorTurn();

            return;
        }
        
        state = EnemyState.Moving;
        currentTurnActor.agent.goalIndex = currentBestIndex;
        currentTurnActor.agent.StartNavigation();
        Debug.Log("[EnemyAI] " + currentTurnActor.actorName + " Selected Target Actor: " + playerTarget.actorName);
    }

    private void FindTarget()
    {
        playerTarget = null;
        targetFound = false;
        currentBestPath = null;

        foreach (Actor actor in ActorManager.instance.partyMemberActors)
        {
            Debug.Log("New Actor");


            List<int> pathToActor = PathFinding.AStarPath(currentTurnActor.agent.currentIndex, actor.agent.currentIndex, 2.25f);

            if (pathToActor == null)
            {
                continue; 
            }

            int newGoalPathIndex = pathToActor.Count - 2;

            if (newGoalPathIndex <= 0)
            {
                newGoalPathIndex = 0;

                // THIS MEANS THAT THEY ARE ADJACENT, GO INTO ATTACK MODE BYPASS MOVMENT

                playerTarget = actor;
                Debug.Log("[EnemyAI] " + currentTurnActor.actorName + " Selected Target Actor: " + playerTarget.actorName);

                //currentTurnActor.agent.LookAtEnemy(playerTarget);

                state = EnemyState.Attacking;

                adjacentFound = true;

                return;
            }

            pathToActor = PathFinding.AStarPath(currentTurnActor.agent.currentIndex, pathToActor[newGoalPathIndex], 2.25f);

            if (pathToActor == null)
            {
                continue;
            }

            newGoalPathIndex = pathToActor.Count - 1 - ((int)currentTurnActor.range - 1);

            if (newGoalPathIndex <= 0)
            { 
                newGoalPathIndex = 0;

                // THIS MEANS THAT THEY ARE ADJACENT, GO INTO ATTACK MODE BYPASS MOVMENT

                playerTarget = actor;
                Debug.Log("[EnemyAI] " + currentTurnActor.actorName + " Selected Target Actor: " + playerTarget.actorName);

                //currentTurnActor.agent.LookAtEnemy(playerTarget);

                state = EnemyState.Attacking;

                adjacentFound = true;

                return;
            }

            int newGoalIndex = pathToActor[newGoalPathIndex];
            List<int> pathToBestIndex = PathFinding.AStarPath(currentTurnActor.agent.currentIndex, newGoalIndex, 1);

            if (pathToBestIndex == null)
                continue;

            if (pathToBestIndex.Count - 1 + attackAPCost > TurnManager.instance.maxActionPoints)
                continue;

            Vector3 goalPosition = new Vector3(GridSystem.instance.points[newGoalIndex].x, 0.5f, GridSystem.instance.points[newGoalIndex].y);
            Vector3 playerPosition = new Vector3(GridSystem.instance.points[actor.agent.currentIndex].x, 0.5f, GridSystem.instance.points[actor.agent.currentIndex].y);
            Vector3 directionToPlayerFromGoal = (playerPosition - goalPosition).normalized;

            RaycastHit hit;
            if (Physics.Raycast(goalPosition, directionToPlayerFromGoal, out hit))
            {
                if (hit.transform.gameObject != actor.gameObject)
                    continue;
            }

            if (currentBestPath == null || pathToBestIndex.Count < currentBestPath.Count)
            {
                targetFound = true;
                currentBestPath = pathToBestIndex;
                playerTarget = actor;
                currentBestIndex = newGoalIndex;
                continue;
            }
        }
    }

    private void Moving()
    {
        if (currentTurnActor.agent.currentState == Agent.State.Idle)
        {
            //currentTurnActor.agent.LookAtEnemy(playerTarget);
            state = EnemyState.Attacking;
        }
    }

    private void Attacking()
    {
        // TODO- wait until the attack anim is done
        playerTarget.TakeDamage(currentTurnActor.attackPower);
        state = EnemyState.Thinking;
        TurnManager.instance.UpdateEnemyActorTurn();
    }
}
