using System.Collections.Generic;
using System;
using static Actor;

[Serializable]
public class PlayableCharacter
{
    public static List<PlayableCharacter> partyMembers = new List<PlayableCharacter>();
    
    ActorType type = ActorType.PartyMember;
    public string name;
    public string background;
    public float health;
    public float maxHealth;
    public float speed;
    public float range;
    public float attackPower;
    public List<Item> actorInventory;
    public List<Item> equippedItems;
    public Item equippedTool = null;

    public PlayableCharacter(string name, string background, float maxHealth, float speed, float range, float attackPower, bool addToParty)
    {
        this.name = name;
        this.background = background;
        this.maxHealth = maxHealth;
        health = maxHealth;
        this.speed = speed;
        this.range = range;
        this.attackPower = attackPower;
        actorInventory = new List<Item>();
        equippedItems = new List<Item>();
        equippedTool = null;

        if (addToParty)
            partyMembers.Add(this);
    }

    public void UpdateData(string name, string background, float maxHealth, float health, float speed, float range, float attackPower, List<Item> actorInventory, List<Item> equippedItems, Item equippedTool)
    {
        this.name = name;
        this.background = background;
        this.maxHealth = maxHealth;
        this.health = health;
        this.speed = speed;
        this.range = range;
        this.attackPower = attackPower;
        this.actorInventory = actorInventory;
        this.equippedItems = equippedItems;
        this.equippedTool = equippedTool;
    }
}
