using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ActorManager : MonoBehaviour, IDataPersistence
{
  public static List<Actor> allActors = new List<Actor>();
  public static List<Actor> partyMemberActors = new List<Actor>();
  public static List<Actor> enemyActors = new List<Actor>();

  private void Awake()
  {
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
  }

    public void LoadData(GameData data)
    {
        
    }

    public void SaveData(ref GameData data) 
    {

    }
}
