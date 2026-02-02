using UnityEngine;

[System.Serializable]
public class Card
{
    public enum ItemType { Null, Sword, Axe }; // Temp card types, will go in and set up properly later once Lucas gets all the new cards done\
    public enum Rarity { Common, Uncommon, Rare, Epic, Legendary };
    public enum CardClass { Weapon, SupporterItem };
    public enum CardSubClass { Melee, Ranged, Magic, Healing, Buff, Debuff };

    public ItemType itemType;
    public Rarity rarity;
    public CardClass cardClass;
    public CardSubClass cardSubClass;

    public Card(ItemType itemType)
    {
        this.itemType = itemType;

        switch (itemType)
        {
            case ItemType.Sword:
                rarity = Rarity.Common;
                cardClass = CardClass.Weapon;
                cardSubClass = CardSubClass.Melee;
                break;
            case ItemType.Axe:
                rarity = Rarity.Uncommon;
                cardClass = CardClass.Weapon;
                cardSubClass = CardSubClass.Melee;
                break;
        }
    }
}
