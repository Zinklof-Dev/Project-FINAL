/*
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ZinklofDev.Utils.MathZ;

public class EnemyAI : MonoBehaviour
{
    PlayerInputManager playerInputManager;
    TurnManager turnManager;

    [SerializeField] int attackCost = 1;

    [SerializeField] List<Actor> allActors = new List<Actor>();
    [SerializeField] List<Actor> partyMemberActors = new List<Actor>();
    [SerializeField] List<Actor> enemyActors = new List<Actor>();
    [SerializeField] List<Actor> enemyActorsUsed  = new List<Actor>();
    [SerializeField] List<Actor> partyMemberActorsAtacked = new List<Actor>()
    
    Actor currentSelectedEnemyActor = null;
    Actor currentSelectedPlayerActor = null;

    public enum State { SelectingEnemyAndPlayer, Navigating, Moving, Attacking, PlayerTurn };
    State currentState = State.SelectingEnemyAndPlayer;

    bool usedListsCleared = false;

    private void Start()
    {
        playerInputManager = FindFirstObjectByType<PlayerInputManager>();
        turnManager = FindFirstObjectByType<TurnManager>();

        allActors = FindObjectsByType<Actor>(FindObjectsSortMode.None).ToList();

        foreach (Actor actor in allActors)
        {
            switch (actor.type)
            {
                case Actor.ActorType.Enemy:
                    enemyActors.Add(actor);
                    break;

                case Actor.ActorType.PartyMember:
                    partyMemberActors.Add(actor);
                    break;
            }
        }
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
                NavigateToActor()
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
        currentSelectedEnemy = null;
        currentSelectedPlayer = null;

        bool usedListsCleared = false;
        
        float closestDistance = Mathf.Infinity;
        
        foreach (Actor enemyActor in enemyActors)
        {
            if (enemyActorsUsed.Count(x => x == enemyActor) >= 3 /*Arbitraity number for how many times one enemy will be used per turn, will tweak later)
                continue;
            
            foreach (Actor playerActor in partyMemberActors)
            {
                if (partyMemberActorsAtacked.Count(x => x == playerActor) >= 3 /*Arbitraity number for how many times one player will be targeted per turn, will tweak later)
                    continue;
                
                List<int> path = PathFinding.AStarPath(enemyActor.agent.currentIndex, playerActor.agent.currentIndex, 2);

                if((path.Count - 1) /*AP needed to move + attackCost /*AP needed to attack > turnManager.enemyActionPoints)
                    continue;
                
                if (path.Count < closestDistance)
                {
                    closestDistance = path.Count;
                    currentSelectedEnemyActor = enemyActor;
                    currentSelectedPlayerActor = playerActor;
                }
            }
        }
        
        if (usedListsCleared && (currentSelectedEnemy == null || currentSelectedPlayer == null))
        {
            usedListsCleared = false;
            // Nothing is availible to move, so end turn
            turnManager.TakeAction(enemyActionPoints);
        }
        
        if (currentSelectedEnemy == null || currentSelectedPlayer == null)
        {
            enemyActorsUsed  = new List<Actor>();
            partyMemberActorsAtacked = new List<Actor>();
            usedListsCleared = true;
            return;
        }
         
        usedListsCleared = false;
        currentState = State.Navigating;
    }

    private void NavigateToActor()
    {
        List<int> path = List<int> path = PathFinding.AStarPath(currentSelectedEnemyActor.agent.currentIndex, currentSelectedPlayerActor.agent.currentIndex, 2);
        path.RemoveAt(path.Count - 1);
        
        currentSelectedEnemyActor.agent.goalIndex = path[path.Count - 1];
        currentSelectedEnemyActor.agent.StartNavigation();
        currentState = State.Moving;
    }

    private void Attack()
    {
        // Placeholder, need to add health system
        turnManager.TakeAction(attackCost);
        currentState = State.SelectingEnemyAndPlayer;
    }
}
*/
