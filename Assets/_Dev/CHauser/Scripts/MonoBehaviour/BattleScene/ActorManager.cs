using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ActorManager : MonoBehaviour, IDataPersistence
{
    public List<Actor> allActors = new List<Actor>();
    public List<Actor> partyMemberActors = new List<Actor>();
    public List<Actor> enemyActors = new List<Actor>();

    public static ActorManager instance;

    private void Start()
    {
        
    }
    
    public void LoadData(GameData data)
    {
        instance = this;

        allActors = new List<Actor>();
        partyMemberActors = new List<Actor>();
        enemyActors = new List<Actor>();

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

        // HELLA TEMP

        foreach (PlayableCharacter character in DataPersistenceManager.instance.gameData.partyMembers)
        {
            DataPersistenceManager.instance.gameData.squad.Add(character);
        }

        foreach (Actor actor in partyMemberActors)
        {
            actor.InitializeActor();
        }

        TurnManager.instance.currentTurnAgent = partyMemberActors[0].agent;
        TurnManager.instance.turnState = TurnManager.TurnState.PlayerTurn;
        FindFirstObjectByType<PlayerInputUIManager>(FindObjectsInactive.Include).gameObject.SetActive(true);
    }

    public void SaveData(ref GameData data) 
    {

    }

}
