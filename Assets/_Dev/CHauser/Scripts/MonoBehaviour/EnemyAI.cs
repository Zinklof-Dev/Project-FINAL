using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    PlayerInputManager playerInputManager;
    TurnManager turnManager;

    [SerializeField] List<Actor> allActors = new List<Actor>();
    [SerializeField] List<Actor> partyMemberActors = new List<Actor>();
    [SerializeField] List<Actor> enemyActors = new List<Actor>();

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
}
