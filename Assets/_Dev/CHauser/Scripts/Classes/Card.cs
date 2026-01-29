using UnityEngine;

[System.Serializable]
public class Card
{
    public enum ItemType { Null, Sword };
    public ItemType itemType;

    public Card(ItemType itemType)
    {
        this.itemType = itemType;
    }
}
