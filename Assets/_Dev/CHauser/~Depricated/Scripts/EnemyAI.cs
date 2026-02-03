/*using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ZinklofDev.Utils.MathZ;
public class EnemyAI : MonoBehaviour
{
    PlayerInputManager playerInputManager;
    TurnManager turnManager;

    [SerializeField] int attackCost = 1;

    [SerializeField] List<Actor> enemyActorsUsed  = new List<Actor>();
    [SerializeField] List<Actor> partyMemberActorsAtacked = new List<Actor>();
    
    Actor currentSelectedEnemyActor = null;
    Actor currentSelectedPlayerActor = null;

    public enum State { SelectingEnemyAndPlayer, Navigating, Moving, Attacking, PlayerTurn };
    public State currentState = State.SelectingEnemyAndPlayer;

    bool usedListsCleared = false;

    private void Start()
    {
        playerInputManager = FindFirstObjectByType<PlayerInputManager>();
        turnManager = FindFirstObjectByType<TurnManager>();
    }

    private void Update()
    {
        if (turnManager.currentTurn == TurnManager.Turn.PlayerTurn)
            return;
        switch (currentState)
        {
            case State.SelectingEnemyAndPlayer:
                ChooseEnemyAndPlayer();
                break;
                
            case State.Navigating:
                NavigateToActor();
                break;
                
            case State.Moving:
                if (currentSelectedEnemyActor.agent.currentState == Agent.State.Idle)
                    currentState = State.Attacking;
                break;
                
            case State.Attacking:
                Attack();
                break;
        }
    }

    private void ChooseEnemyAndPlayer()
    {
        currentSelectedEnemyActor = null;
        currentSelectedPlayerActor = null;
        
        float closestDistance = Mathf.Infinity;
        
        foreach (Actor enemyActor in ActorManager.enemyActors)
        {
            if (enemyActorsUsed.Count(x => x == enemyActor) >= 3 /*Arbitraity number for how many times one enemy will be used per turn, will tweak later)
                continue;
            
            foreach (Actor playerActor in ActorManager.partyMemberActors)
            {
                if (partyMemberActorsAtacked.Count(x => x == playerActor) >= 3 /*Arbitraity number for how many times one player will be targeted per turn, will tweak later*)
                    continue;
                
                List<int> path = PathFinding.AStarPath(enemyActor.agent.currentIndex, playerActor.agent.currentIndex, 2);
                
                if (path == null)
                    continue;

                if((path.Count - 1) /*AP needed to move* + attackCost /*AP needed to attack* > turnManager.enemyActionPoints)
                    continue;
                
                if (path.Count < closestDistance)
                {
                    closestDistance = path.Count;
                    currentSelectedEnemyActor = enemyActor;
                    currentSelectedPlayerActor = playerActor;
                }
            }
        }
        
        if (usedListsCleared && (currentSelectedEnemyActor == null || currentSelectedPlayerActor == null))
        {
            usedListsCleared = false;
            enemyActorsUsed  = new List<Actor>();
            partyMemberActorsAtacked = new List<Actor>();
            // Nothing is availible to move, so end turn
            turnManager.TakeAction(turnManager.enemyActionPoints);
        }
        
        else if (currentSelectedEnemyActor == null || currentSelectedPlayerActor == null)
        {
            enemyActorsUsed  = new List<Actor>();
            partyMemberActorsAtacked = new List<Actor>();
            usedListsCleared = true;
            return;
        }
         
        usedListsCleared = false;
        enemyActorsUsed.Add(currentSelectedEnemyActor);
        partyMemberActorsAtacked.Add(currentSelectedPlayerActor);
        currentState = State.Navigating;
    }

    private void NavigateToActor()
    {
        List<int> path = PathFinding.AStarPath(currentSelectedEnemyActor.agent.currentIndex, currentSelectedPlayerActor.agent.currentIndex, 2);
        path.RemoveAt(path.Count - 1);
        
        currentSelectedEnemyActor.agent.goalIndex = path[path.Count - 1];
        currentSelectedEnemyActor.agent.StartNavigation();
        currentState = State.Moving;
    }

    private void Attack()
    {
        // Placeholder, need to add health system
        turnManager.TakeAction(attackCost);
        currentSelectedPlayerActor.TakeDamage(currentSelectedEnemyActor.attackPower);
        currentState = State.SelectingEnemyAndPlayer;
    }
}*/
