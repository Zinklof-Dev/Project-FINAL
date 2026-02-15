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
    [SerializeField] List<Actor> evaluatedTargets = new List<Actor>();
    List<Actor> attackedTargets = new List<Actor>();


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

        if (!targetFound)
        {
            TurnManager.instance.UpdateEnemyActorTurn();
            evaluatedTargets = new List<Actor>();
            return;
        }

        int? goalIndex = FindGoalIndex();

        if (goalIndex == null)
        {
            evaluatedTargets.Add(playerTarget);
            playerTarget = null;
            return;
        }

        // TODO - logic to attack

        evaluatedTargets = new List<Actor>();
        currentTurnActor.agent.goalIndex = (int) goalIndex;
        currentTurnActor.agent.StartNavigation();
        state = EnemyState.Moving;
    }

    private void FindTarget()
    {
        playerTarget = null;
        targetFound = false;

        foreach (Actor player in ActorManager.instance.partyMemberActors)
        {
            if (evaluatedTargets.Contains(player))
                continue;


            if (playerTarget == null)
            {
                List<int> path = PathFinding.AStarPath(currentTurnActor.agent.currentIndex, player.agent.currentIndex, 1);

                if (path != null)
                {
                    playerTarget = player;
                    targetFound = true;
                }

                continue;
            }

            List<int> pathToCurrent = PathFinding.AStarPath(currentTurnActor.agent.currentIndex, playerTarget.agent.currentIndex, 1);
            List<int> pathToNext = PathFinding.AStarPath(currentTurnActor.agent.currentIndex, player.agent.currentIndex, 1);

            if (pathToNext == null)
                continue;

            if (Mathf.Abs(pathToNext.Count - 1 - currentTurnActor.range) < Mathf.Abs(pathToCurrent.Count - 1 - currentTurnActor.range))
            {
                playerTarget = player;
                targetFound = true;
            }
        }
    }

    private int? FindGoalIndex()
    {
        List<int> path = PathFinding.AStarPath(currentTurnActor.agent.currentIndex, playerTarget.agent.currentIndex, 1);

        if (path == null)
            return null;

        if (path.Count - 1 > TurnManager.instance.actionPoints)
            return null;

        return path[Mathf.Clamp(path.Count - 1 - (int) currentTurnActor.range, 0, path.Count - 1)];
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
