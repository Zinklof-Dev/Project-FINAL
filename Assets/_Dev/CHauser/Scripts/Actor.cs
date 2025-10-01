using UnityEngine;

public class Actor : MonoBehaviour
{
    public enum ActorType { PartyMember, Enemy }

    [SerializeField] public ActorType type = ActorType.PartyMember;

    [SerializeField] public string actorName = "Default Actor Name";

    [SerializeField] private float health;
    [SerializeField] private float maxHealth;
    [SerializeField] private float speed;


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
