using System.Collections.Generic;
using static Actor;

public class PlayableCharacter
{
    ActorType type;
    public string name;
    public string background;
    public float health;
    public float maxHealth;
    public float speed;
    public float range;
    public float attackPower;
    public List<Item> actorInventory;
    public List<Item> equippedItems;
    public int ID { get;  private set; }
}
