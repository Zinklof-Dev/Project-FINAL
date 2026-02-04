using UnityEngine;

public class WeaponPrefabManager : MonoBehaviour
{
    // TEMP
    [SerializeField] public GameObject swordPrefab;
    [SerializeField] public GameObject axePrefab;

    // TWO HANDED 
    [SerializeField] public GameObject ZweihanderPrefab;
    [SerializeField] public GameObject Dane_AxePrefab;
    [SerializeField] public GameObject Great_SwordPrefab;
    [SerializeField] public GameObject Two_Handed_MacePrefab;
    [SerializeField] public GameObject Two_Handed_HammerPrefab;

    // POLEARM 
    [SerializeField] public GameObject SpearPrefab;
    [SerializeField] public GameObject BardichePrefab;
    [SerializeField] public GameObject VoulgePrefab;
    [SerializeField] public GameObject GlavePrefab;

    // ONE HANDED 
    [SerializeField] public GameObject Bastard_SwordPrefab;
    [SerializeField] public GameObject Arming_SwordPrefab;
    [SerializeField] public GameObject Noble_Arming_SwordPrefab;
    [SerializeField] public GameObject Nord_AxePrefab;
    [SerializeField] public GameObject Battle_AxePrefab;
    [SerializeField] public GameObject Spiked_MacePrefab;
    [SerializeField] public GameObject Cav_MacePrefab;
    [SerializeField] public GameObject Battle_HammerPrefab;
    [SerializeField] public GameObject KnifePrefab;

    // BOWS 
    [SerializeField] public GameObject LongbowPrefab;
    [SerializeField] public GameObject ShortbowPrefab;
    [SerializeField] public GameObject Curved_BowPrefab;
    [SerializeField] public GameObject Hunting_BowPrefab;

    // CROSSBOWS 
    [SerializeField] public GameObject ArbalestPrefab;
    [SerializeField] public GameObject Early_CrossbowPrefab;

    // SPECIAL 
    [SerializeField] public GameObject HalberdPrefab;


    public static WeaponPrefabManager instance;

    private void Awake()
    {
        instance = this;
    }

    public GameObject GetWeaponPrefab(Card card)
    {
        if(card.cardClass != Card.CardClass.Weapon)
        {
            Debug.Log("Card is not a weapon!");
            return null;
        }

        switch(card.itemType)
        {
            case Card.ItemType.Sword:
                return swordPrefab;

            case Card.ItemType.Axe:
                return axePrefab;


            // TWO HANDED
            case Card.ItemType.Zweihander:
                return ZweihanderPrefab;
            case Card.ItemType.Dane_Axe:
                return Dane_AxePrefab;
            case Card.ItemType.Great_Sword:
                return Great_SwordPrefab;
            case Card.ItemType.Two_Handed_Mace:
                return Two_Handed_MacePrefab;
            case Card.ItemType.Two_Handed_Hammer:
                return Two_Handed_HammerPrefab;

            // POLEARM
            case Card.ItemType.Spear:
                return SpearPrefab;
            case Card.ItemType.Bardiche:
                return BardichePrefab;
            case Card.ItemType.Voulge:
                return VoulgePrefab;
            case Card.ItemType.Glave:
                return GlavePrefab;

            // ONE HANDED
            case Card.ItemType.Bastard_Sword:
                return Bastard_SwordPrefab;
            case Card.ItemType.Arming_Sword:
                return Arming_SwordPrefab;
            case Card.ItemType.Noble_Arming_Sword:
                return Noble_Arming_SwordPrefab;
            case Card.ItemType.Nord_Axe:
                return Nord_AxePrefab;
            case Card.ItemType.Battle_Axe:
                return Battle_AxePrefab;
            case Card.ItemType.Spiked_Mace:
                return Spiked_MacePrefab;
            case Card.ItemType.Cav_Mace:
                return Cav_MacePrefab;
            case Card.ItemType.Battle_Hammer:
                return Battle_HammerPrefab;
            case Card.ItemType.Knife:
                return KnifePrefab;

            // BOWS
            case Card.ItemType.Longbow:
                return LongbowPrefab;
            case Card.ItemType.Shortbow:
                return ShortbowPrefab;
            case Card.ItemType.Curved_Bow:
                return Curved_BowPrefab;
            case Card.ItemType.Hunting_Bow:
                return Hunting_BowPrefab;

            // CROSSBOWS
            case Card.ItemType.Arbalest:
                return ArbalestPrefab;
            case Card.ItemType.Early_Crossbow:
                return Early_CrossbowPrefab;

            // SPECIAL
            case Card.ItemType.Halberd:
                return HalberdPrefab;

        }

        Debug.Log("No prefab found for weapon: " + card.itemType);
        return null;
    }
}
