using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ZinklofDev.Utils.MathZ;

public class EnemyAI : MonoBehaviour
{
    PlayerInputManager playerInputManager;
    TurnManager turnManager;

    [SerializeField] List<Actor> allActors = new List<Actor>();
    [SerializeField] List<Actor> partyMemberActors = new List<Actor>();
    [SerializeField] List<Actor> enemyActors = new List<Actor>();
    Actor currentSelectedEnemy = null;
    Actor currentSelectedPlayer = null;

    public enum State { SelectingEnemyAndPlayer, Moving, Attacking, PlayerTurn };
    State currentState = State.PlayerTurn;

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
    }

    private void ChooseEnemyAndPlayer()
    {
        // Placeholder, will have a list of actors that have already taken an action and then move them to an already acted list
        currentSelectedEnemy = enemyActors[0];
        currentSelectedPlayer = playerActors[0];
        
        float closestDistance = Mathf.Infinity;
        
        foreach (Actor enemyActor in enemyActors)
        {
            foreach (Actor playerActor in playerActors)
            {
                List<int> path = PathFinding.AStarPath(enemyActor.agent.currentIndex, playerActor.agent.currentIndex, 2);
                if (path.Count < closestDistance)
                {
                    closestDistance = path.Count;
                    currentSelectedEnemy = enemyActor;
                    currentSelectedPlayer = playerActor;
                }
            }
        }
        currentState = State.Navigating;
    }

    private void NavigateToActor()
    {
        List<int> path = List<int> path = PathFinding.AStarPath(currentSelectedEnemy.agent.currentIndex, currentSelectedPlayer.agent.currentIndex, 2);
        path.RemoveAt(path.Count - 1);
        
        currentSelectedEnemyActor.agent.goalIndex = path[path.Count - 1];
        currentSelectedEnemyActor.agent.StartNavigation();
        currentState = State.Moving;
    }
}
