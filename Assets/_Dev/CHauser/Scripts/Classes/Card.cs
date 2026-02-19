using UnityEngine;

[System.Serializable]
public class Card
{
   public enum ItemType 
    {
        // WEAPONS

        Sword, Axe, // <--- TEMP PLACEHOLDERS
        // TWO HANDED
        Zweihander, Dane_Axe, Great_Sword, Two_Handed_Mace, Two_Handed_Hammer,
        // POLEARM
        Spear, Bardiche, Voulge, Glave,
        // ONE HANDED
        Bastard_Sword, Arming_Sword, Noble_Arming_Sword, Nord_Axe, Battle_Axe, Spiked_Mace, Cav_Mace, Battle_Hammer, Knife,
        // BOWS
        Longbow, Shortbow, Curved_Bow, Hunting_Bow,
        // CROSSBOWS
        Arbalest, Early_Crossbow,
        // SPECIAL
        Halberd,

        //////////////////////////////////////////////////////////////////////

        // SUPPORTER ITEMS
        Health_Supporter, Range_Supporter, Action_Points_Supporter
    }; 

    public enum Rarity { Common, Uncommon, Rare, Epic, Legendary };
    public enum CardClass { Weapon, SupporterItem };
    public enum CardSubClass { Two_Handed, Polearm, One_Handed, Bow, Crossbow, Special, None };

    public ItemType itemType;
    public Rarity rarity;
    public CardClass cardClass;
    public CardSubClass cardSubClass;

    public string description;

    // For weapons
    public float attackPower;
    public float bestRange;

    // For supporter items
    public float healthBonus;
    public float rangeBonus;
    public float actionPointBonus;


    public Card(ItemType itemType)
    {
        this.itemType = itemType;

        switch (itemType)
        {
            // TEMP
            case ItemType.Sword:
                rarity = Rarity.Common;
                cardClass = CardClass.Weapon;
                cardSubClass = CardSubClass.One_Handed;
                description = "A standard sword, reliable and well-balanced. \n+5 attack \nMost Efficent 2 Units Away";
                attackPower = 5;
                bestRange = 2;
                break;

            case ItemType.Axe:
                rarity = Rarity.Uncommon;
                cardClass = CardClass.Weapon;
                cardSubClass = CardSubClass.One_Handed;
                description = "A sturdy axe, capable of dealing heavy damage. \n+5 attack \nMost Efficent 2 Units Away";
                attackPower = 5;
                bestRange = 2;
                break;
            //////////////////////////////////////////////////////////////////

            // TWO HANDED
            case ItemType.Zweihander:
                rarity = Rarity.Legendary;
                cardClass = CardClass.Weapon;
                cardSubClass = CardSubClass.Two_Handed;
                description = "Quick, meduim damage, and can parry.";
                break;

            case ItemType.Dane_Axe:
                rarity = Rarity.Rare;
                cardClass = CardClass.Weapon;
                cardSubClass = CardSubClass.Two_Handed;
                description = "Meduim speed, high damage.";
                break;

            case ItemType.Great_Sword:
                rarity = Rarity.Epic;
                cardClass = CardClass.Weapon;
                cardSubClass = CardSubClass.Two_Handed;
                break;

            case ItemType.Two_Handed_Mace:
                rarity = Rarity.Uncommon;
                cardClass = CardClass.Weapon;
                cardSubClass = CardSubClass.Two_Handed;
                break;

            case ItemType.Two_Handed_Hammer:
                rarity = Rarity.Uncommon;
                cardClass = CardClass.Weapon;
                cardSubClass = CardSubClass.Two_Handed;
                break;

            // POLEARM
            case ItemType.Spear:
                rarity = Rarity.Common;
                cardClass = CardClass.Weapon;
                cardSubClass = CardSubClass.Polearm;
                break;

            case ItemType.Bardiche:
                rarity = Rarity.Rare;
                cardClass = CardClass.Weapon;
                cardSubClass = CardSubClass.Polearm;
                break;

            case ItemType.Voulge:
                rarity = Rarity.Rare;
                cardClass = CardClass.Weapon;
                cardSubClass = CardSubClass.Polearm;
                break;

            case ItemType.Glave:
                rarity = Rarity.Epic;
                cardClass = CardClass.Weapon;
                cardSubClass = CardSubClass.Polearm;
                break;

            // ONE HANDED
            case ItemType.Bastard_Sword:
                rarity = Rarity.Uncommon;
                cardClass = CardClass.Weapon;
                cardSubClass = CardSubClass.One_Handed;
                break;

            case ItemType.Arming_Sword:
                rarity = Rarity.Common;
                cardClass = CardClass.Weapon;
                cardSubClass = CardSubClass.One_Handed;
                break;

            case ItemType.Noble_Arming_Sword:
                rarity = Rarity.Rare;
                cardClass = CardClass.Weapon;
                cardSubClass = CardSubClass.One_Handed;
                break;

            case ItemType.Nord_Axe:
                rarity = Rarity.Common;
                cardClass = CardClass.Weapon;
                cardSubClass = CardSubClass.One_Handed;
                break;

            case ItemType.Battle_Axe:
                rarity = Rarity.Uncommon;
                cardClass = CardClass.Weapon;
                cardSubClass = CardSubClass.One_Handed;
                break;

            case ItemType.Spiked_Mace:
                rarity = Rarity.Common;
                cardClass = CardClass.Weapon;
                cardSubClass = CardSubClass.One_Handed;
                break;

            case ItemType.Cav_Mace:
                rarity = Rarity.Rare;
                cardClass = CardClass.Weapon;
                cardSubClass = CardSubClass.One_Handed;
                break;

            case ItemType.Battle_Hammer:
                rarity = Rarity.Uncommon;
                cardClass = CardClass.Weapon;
                cardSubClass = CardSubClass.One_Handed;
                break;

            case ItemType.Knife:
                rarity = Rarity.Common;
                cardClass = CardClass.Weapon;
                cardSubClass = CardSubClass.One_Handed;
                break;

            // BOWS
            case ItemType.Longbow:
                rarity = Rarity.Uncommon;
                cardClass = CardClass.Weapon;
                cardSubClass = CardSubClass.Bow;
                break;

            case ItemType.Shortbow:
                rarity = Rarity.Common;
                cardClass = CardClass.Weapon;
                cardSubClass = CardSubClass.Bow;
                break;

            case ItemType.Curved_Bow:
                rarity = Rarity.Rare;
                cardClass = CardClass.Weapon;
                cardSubClass = CardSubClass.Bow;
                break;

            case ItemType.Hunting_Bow:
                rarity = Rarity.Epic;
                cardClass = CardClass.Weapon;
                cardSubClass = CardSubClass.Bow;
                break;

            // CROSSBOWS
            case ItemType.Arbalest:
                rarity = Rarity.Rare;
                cardClass = CardClass.Weapon;
                cardSubClass = CardSubClass.Crossbow;
                break;

            case ItemType.Early_Crossbow:
                rarity = Rarity.Common;
                cardClass = CardClass.Weapon;
                cardSubClass = CardSubClass.Crossbow;
                break;

            // SPECIAL            
            case ItemType.Halberd:
                rarity = Rarity.Legendary;
                cardClass = CardClass.Weapon;
                cardSubClass = CardSubClass.Special;
                break;

            // Supporter Items
            case ItemType.Health_Supporter:
                rarity = Rarity.Common;
                cardClass = CardClass.SupporterItem;
                cardSubClass = CardSubClass.None;
                healthBonus = 25;
                description = $"Gives character {healthBonus} extra health.";
                break;


            case ItemType.Range_Supporter:
                rarity = Rarity.Common;
                cardClass = CardClass.SupporterItem;
                cardSubClass = CardSubClass.None;
                rangeBonus = 1;
                break;

            case ItemType.Action_Points_Supporter:
                rarity = Rarity.Rare;
                cardClass = CardClass.SupporterItem;
                cardSubClass = CardSubClass.None;
                actionPointBonus = 5;
                description = $"Gives character {actionPointBonus} extra Action Points.";
                break;
        }
    }
}