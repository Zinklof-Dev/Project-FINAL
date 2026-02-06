using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour
{
    public PlayableCharacter character;
    private Card weapon;

    public enum ActorType { PartyMember, Enemy }

    [SerializeField] public ActorType type = ActorType.PartyMember;

    [SerializeField] public string actorName = "Default Actor Name";

    [SerializeField] public float health;
    [SerializeField] public float maxHealth;
    [SerializeField] public float speed;
    [SerializeField] public int range; // Range is the number of gird squares away an actor can attack. // GOING TO CHANGE
    [SerializeField] public float attackPower;
    [SerializeField] public Transform weaponSlot;


    public Agent agent;

    public Item equippedTool = null;

    private void Start()
    {
        agent = GetComponent<Agent>();

        if (agent == null)
        {
            Debug.Log("No Agent Attatched to Actor!");
            gameObject.SetActive(false);
        }
    }

    public void InitializeActor()
    {
        try
        {
            character = DataPersistenceManager.instance.gameData.squad[ActorManager.instance.partyMemberActors.IndexOf(this)];
        }

        catch
        {
            Debug.Log("No Character Data Found for Actor: " + actorName);
            Destroy(this.gameObject);
            return;
        }

        actorName = character.name;
        maxHealth = character.maxHealth;
        health = maxHealth;
        speed = character.speed;
        attackPower = character.attackPower;

        weapon = character.GetWeapon();

        if (weapon == null)
            return;

        attackPower += weapon.attackPower;

        VisualWeaponEquip();
    }

    private void VisualWeaponEquip()
    {
        if (weapon == null || weaponSlot == null)
            return;

        GameObject weaponPrefab = WeaponPrefabManager.instance.GetWeaponPrefab(weapon);

        if (weaponPrefab == null)
            return;

        Instantiate(weaponPrefab, weaponSlot);
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if(health <= 0)
            Death();
    }

    private void Death()
    {
        ActorManager.instance.allActors.Remove(this);
        ActorManager.instance.partyMemberActors.Remove(this);
        ActorManager.instance.partyMemberActors.Remove(this);
        Destroy(this.gameObject);
        /*
        if (ActorManager.instance.enemyActors.Count == 0)
            
        //winLossManager.Win();

        else if (ActorManager.instance.partyMemberActors.Count == 0)
            //winLossManager.Loss();

            Destroy(this.gameObject);*/
    }
}

