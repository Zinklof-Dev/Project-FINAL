using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Actor : MonoBehaviour
{
    public enum ActorType { PartyMember, Enemy }

    [SerializeField] public ActorType type = ActorType.PartyMember;

    [SerializeField] public string actorName = "Default Actor Name";
    // Probably gonna replace this 
    [SerializeField] public List<Item> equippedItems = new List<Item>();
    [SerializeField] public List<Item> actorInventory = new List<Item>();

    [SerializeField] public float health;
    [SerializeField] public float maxHealth;
    [SerializeField] public float speed;
    [SerializeField] public int range; // Range is the number of gird squares away an actor can attack.
    [SerializeField] public float attackPower;


    public Agent agent;
    [SerializeField] public PlayerInputManager playerInputManager;

    public Item equippedTool = null;

    TempWinLossManagerEP winLossManager;

    private void Start()
    {
        agent = GetComponent<Agent>();

        if (agent == null)
        {
            Debug.Log("No Agent Attatched to Actor!");
            gameObject.SetActive(false);
        }

        playerInputManager = FindFirstObjectByType<PlayerInputManager>();
        winLossManager = FindFirstObjectByType<TempWinLossManagerEP>();
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if(health <= 0)
            Death();
    }

    private void Death()
    {
        ActorManager.allActors.Remove(this);
        ActorManager.partyMemberActors.Remove(this);
        ActorManager.enemyActors.Remove(this);

        if (ActorManager.enemyActors.Count == 0)
         winLossManager.Win();

        else if (ActorManager.partyMemberActors.Count == 0)
         winLossManager.Loss();

        Destroy(this.gameObject);
    }
}

