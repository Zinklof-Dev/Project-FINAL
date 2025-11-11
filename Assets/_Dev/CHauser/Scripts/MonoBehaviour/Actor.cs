using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour
{
    public enum ActorType { PartyMember, Enemy }

    [SerializeField] public ActorType type = ActorType.PartyMember;

    [SerializeField] public string actorName = "Default Actor Name";

    [SerializeField] public List<Item> equippedItems = new List<Item>();
    [SerializeField] public List<Item> actorInventory = new List<Item>();

    [SerializeField] public float health;
    [SerializeField] public float maxHealth;
    [SerializeField] public float speed;
    [SerializeField] public int range; // Range is the number of gird squares away an actor can attack.


    public Agent agent;
    [SerializeField] public PlayerInputManager playerInputManager;


    private void Start()
    {
        agent = GetComponent<Agent>();

        if (agent == null)
        {
            Debug.Log("No Agent Attatched to Actor!");
            gameObject.SetActive(false);
        }

        playerInputManager = FindFirstObjectByType<PlayerInputManager>();
    }
}
