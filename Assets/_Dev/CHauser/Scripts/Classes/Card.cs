using UnityEngine;

[System.Serializable]
public class Card
{
    public enum ItemType { Null, Sword, Axe }; // Temp card types, will go in and set up properly later once Lucas gets all the new cards done
    public ItemType itemType;

    public Card(ItemType itemType)
    {
        this.itemType = itemType;
    }
}
