using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CardInfoDisplayer : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private TMP_Text cardNameTxt;
    [SerializeField] private TMP_Text cardDescriptionTxt;

    public static CardInfoDisplayer instance;

    public static void DisplayCardInfo(Card card)
    {
        instance.cardNameTxt.text = card.itemType.ToString();
        instance.cardDescriptionTxt.text = card.description;

        switch (card.itemType)
        {
            // TEMP PLACEHOLDERS
            case Card.ItemType.Sword:
                instance.image.sprite = CardManager.instance.swordSprite;
                break;

            case Card.ItemType.Axe:
                instance.image.sprite = CardManager.instance.axeSprite;
                break;

            // TWO HANDED
            case Card.ItemType.Zweihander:
                instance.image.sprite = CardManager.instance.zweihanderSprite;
                break;

            case Card.ItemType.Dane_Axe:
                instance.image.sprite = CardManager.instance.daneAxeSprite;
                break;

            case Card.ItemType.Great_Sword:
                instance.image.sprite = CardManager.instance.greatSwordSprite;
                break;

            case Card.ItemType.Two_Handed_Mace:
                instance.image.sprite = CardManager.instance.twoHandedMaceSprite;
                break;

            case Card.ItemType.Two_Handed_Hammer:
                instance.image.sprite = CardManager.instance.twoHandedHammerSprite;
                break;

            // POLEARM
            case Card.ItemType.Spear:
                instance.image.sprite = CardManager.instance.spearSprite;
                break;

            case Card.ItemType.Bardiche:
                instance.image.sprite = CardManager.instance.bardicheSprite;
                break;

            case Card.ItemType.Voulge:
                instance.image.sprite = CardManager.instance.voulgeSprite;
                break;

            case Card.ItemType.Glave:
                instance.image.sprite = CardManager.instance.glaveSprite;
                break;

            // ONE HANDED
            case Card.ItemType.Bastard_Sword:
                instance.image.sprite = CardManager.instance.bastardSwordSprite;
                break;

            case Card.ItemType.Arming_Sword:
                instance.image.sprite = CardManager.instance.armingSwordSprite;
                break;

            case Card.ItemType.Noble_Arming_Sword:
                instance.image.sprite = CardManager.instance.nobleArmingSwordSprite;
                break;

            case Card.ItemType.Nord_Axe:
                instance.image.sprite = CardManager.instance.nordAxeSprite;
                break;

            case Card.ItemType.Battle_Axe:
                instance.image.sprite = CardManager.instance.battleAxeSprite;
                break;

            case Card.ItemType.Spiked_Mace:
                instance.image.sprite = CardManager.instance.spikedMaceSprite;
                break;

            case Card.ItemType.Cav_Mace:
                instance.image.sprite = CardManager.instance.cavMaceSprite;
                break;

            case Card.ItemType.Battle_Hammer:
                instance.image.sprite = CardManager.instance.battleHammerSprite;
                break;

            case Card.ItemType.Knife:
                instance.image.sprite = CardManager.instance.knifeSprite;
                break;

            // BOWS
            case Card.ItemType.Longbow:
                instance.image.sprite = CardManager.instance.longbowSprite;
                break;

            case Card.ItemType.Shortbow:
                instance.image.sprite = CardManager.instance.shortbowSprite;
                break;

            case Card.ItemType.Curved_Bow:
                instance.image.sprite = CardManager.instance.curvedBowSprite;
                break;

            case Card.ItemType.Hunting_Bow:
                instance.image.sprite = CardManager.instance.huntingBowSprite;
                break;

            // CROSSBOWS
            case Card.ItemType.Arbalest:
                instance.image.sprite = CardManager.instance.arbalestSprite;
                break;

            case Card.ItemType.Early_Crossbow:
                instance.image.sprite = CardManager.instance.earlyCrossbowSprite;
                break;

            // SPECIAL
            case Card.ItemType.Halberd:
                instance.image.sprite = CardManager.instance.halberdSprite;
                break;

            case Card.ItemType.Health_Supporter:
                instance.image.sprite = CardManager.instance.healthSupporterSprite;
                break;

            case Card.ItemType.Range_Supporter:
                instance.image.sprite = CardManager.instance.rangeSupporterSprite;
                break;

            case Card.ItemType.Action_Points_Supporter:
                instance.image.sprite = CardManager.instance.actionPointsSupporterSprite;
                break;
        }

    }
}
