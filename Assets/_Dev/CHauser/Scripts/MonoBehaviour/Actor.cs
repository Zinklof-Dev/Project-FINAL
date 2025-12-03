using System.Collections.Generic;
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

    // TempWinLossManagerEP winLossManager;

    private void Start()
    {
        agent = GetComponent<Agent>();

        if (agent == null)
        {
            Debug.Log("No Agent Attatched to Actor!");
            gameObject.SetActive(false);
        }

        playerInputManager = FindFirstObjectByType<PlayerInputManager>();
        // winLossManager = FindFirstObjectByType<TempWinLossManagerEP>();
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
            // winLossManager.Win();
            
        else if(ActorManager.partyMemberActors.Count == 0)
            // winLossManager.Loss();
            
        Destroy(this.gameObject);
    }
}
/*
// Temporary win / loss screen manager, will move to its' own script, I just don't want .meta file conflicts to occur again.
// Will definitly depricate in the future in favor of a more complex level fail / succeed script, but this will work for now for Experience Pinellas.
// Also def gonna comment this out so that the team doesn't have to do the compiler error shuffle tomorrow.
// Also gonna include these comments in the new script that I'm going to migrate to tommorow. 

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TempWinLossManagerEP : MonoBehaviour // EP stands for Experience Pinellas.
{
    [SerializeField] GameObject winScreen;
    [SerializeField] GameObject lossScreen;
    
    public void Win()
    {
        winScreen.SetActive(true);
    }

    public void Loss()
    {
        lossScreen.SetActive(true);
    }

    public void Reset()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}*/
