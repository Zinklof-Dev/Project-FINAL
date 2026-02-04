using System.Collections.Generic;
using System;
using static Actor;

[Serializable]
public class PlayableCharacter
{
    public static List<PlayableCharacter> partyMembers = new List<PlayableCharacter>();
    
    //ActorType type = ActorType.PartyMember;
    public string name;
    public string background;
    public float health;
    public float maxHealth;
    public float speed;
    public float attackPower;
    public List<Card> inventory;

    public PlayableCharacter(string name, string background, float maxHealth, float speed, float attackPower, bool addToParty)
    {
        this.name = name;
        this.background = background;
        this.maxHealth = maxHealth;
        health = maxHealth;
        this.speed = speed;
        this.attackPower = attackPower;
        inventory = new List<Card>();

        if (addToParty)
            partyMembers.Add(this);
    }

    public void UpdateData(string name, string background, float maxHealth, float health, float speed, float attackPower, List<Card> inventory)
    {
        this.name = name;
        this.background = background;
        this.maxHealth = maxHealth;
        this.health = health;
        this.speed = speed;
        this.attackPower = attackPower;
        this.inventory = inventory;
    }
}
