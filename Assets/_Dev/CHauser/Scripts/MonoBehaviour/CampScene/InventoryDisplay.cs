using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryDisplay : MonoBehaviour, IDropHandler
{
    [HideInInspector] public PlayableCharacter currentDisplayedCharacter;

    [SerializeField] private GameObject inventoryCardPrefab;
    [SerializeField] private int maxInventorySize = 7;

    List<GameObject> instantiatedCards = new List<GameObject>();

    public static InventoryDisplay instance;

    public void DisplayInventory(PlayableCharacter character)
    {
        currentDisplayedCharacter = character;

        ClearDisplayedInventory();

        foreach (Card card in character.inventory)
        {
            GameObject displayedCard = Instantiate(inventoryCardPrefab, transform);
            displayedCard.GetComponent<InventoryCardPrefabMonoBehaviour>().card = card;
            instantiatedCards.Add(displayedCard);
            Image displayedCardImage = displayedCard.GetComponent<Image>();

            switch (card.itemType)
            {
                // TEMP PLACEHOLDERS
                case Card.ItemType.Sword:
                    displayedCardImage.sprite = CardManager.instance.swordSprite;
                    break;

                case Card.ItemType.Axe:
                    displayedCardImage.sprite = CardManager.instance.axeSprite;
                    break;

                // TWO HANDED
                case Card.ItemType.Zweihander:
                    displayedCardImage.sprite = CardManager.instance.zweihanderSprite;
                    break;

                case Card.ItemType.Dane_Axe:
                    displayedCardImage.sprite = CardManager.instance.daneAxeSprite;
                    break;

                case Card.ItemType.Great_Sword:
                    displayedCardImage.sprite = CardManager.instance.greatSwordSprite;
                    break;

                case Card.ItemType.Two_Handed_Mace:
                    displayedCardImage.sprite = CardManager.instance.twoHandedMaceSprite;
                    break;

                case Card.ItemType.Two_Handed_Hammer:
                    displayedCardImage.sprite = CardManager.instance.twoHandedHammerSprite;
                    break;

                // POLEARMS
                case Card.ItemType.Spear:
                    displayedCardImage.sprite = CardManager.instance.spearSprite;
                    break;

                case Card.ItemType.Bardiche:
                    displayedCardImage.sprite = CardManager.instance.bardicheSprite;
                    break;

                case Card.ItemType.Voulge:
                    displayedCardImage.sprite = CardManager.instance.voulgeSprite;
                    break;

                case Card.ItemType.Glave:
                    displayedCardImage.sprite = CardManager.instance.glaveSprite;
                    break;

                // ONE HANDED
                case Card.ItemType.Bastard_Sword:
                    displayedCardImage.sprite = CardManager.instance.bastardSwordSprite;
                    break;

                case Card.ItemType.Arming_Sword:
                    displayedCardImage.sprite = CardManager.instance.armingSwordSprite;
                    break;

                case Card.ItemType.Noble_Arming_Sword:
                    displayedCardImage.sprite = CardManager.instance.nobleArmingSwordSprite;
                    break;

                case Card.ItemType.Nord_Axe:
                    displayedCardImage.sprite = CardManager.instance.nordAxeSprite;
                    break;

                case Card.ItemType.Battle_Axe:
                    displayedCardImage.sprite = CardManager.instance.battleAxeSprite;
                    break;

                case Card.ItemType.Spiked_Mace:
                    displayedCardImage.sprite = CardManager.instance.spikedMaceSprite;
                    break;

                case Card.ItemType.Cav_Mace:
                    displayedCardImage.sprite = CardManager.instance.cavMaceSprite;
                    break;

                case Card.ItemType.Battle_Hammer:
                    displayedCardImage.sprite = CardManager.instance.battleHammerSprite;
                    break;

                case Card.ItemType.Knife:
                    displayedCardImage.sprite = CardManager.instance.knifeSprite;
                    break;

                // BOWS
                case Card.ItemType.Longbow:
                    displayedCardImage.sprite = CardManager.instance.longbowSprite;
                    break;

                case Card.ItemType.Shortbow:
                    displayedCardImage.sprite = CardManager.instance.shortbowSprite;
                    break;

                case Card.ItemType.Curved_Bow:
                    displayedCardImage.sprite = CardManager.instance.curvedBowSprite;
                    break;

                case Card.ItemType.Hunting_Bow:
                    displayedCardImage.sprite = CardManager.instance.huntingBowSprite;
                    break;

                // CROSSBOWS
                case Card.ItemType.Arbalest:
                    displayedCardImage.sprite = CardManager.instance.arbalestSprite;
                    break;

                case Card.ItemType.Early_Crossbow:
                    displayedCardImage.sprite = CardManager.instance.earlyCrossbowSprite;
                    break;

                // SPECIAL
                case Card.ItemType.Halberd:
                    displayedCardImage.sprite = CardManager.instance.halberdSprite;
                    break;

                // SUPPORTER ITEMS
                case Card.ItemType.Health_Supporter:
                    displayedCardImage.sprite = CardManager.instance.healthSupporterSprite;
                    break;

                case Card.ItemType.Range_Supporter:
                    displayedCardImage.sprite = CardManager.instance.rangeSupporterSprite;
                    break;

                case Card.ItemType.Action_Points_Supporter:
                    displayedCardImage.sprite = CardManager.instance.actionPointsSupporterSprite;
                    break;

                default:
                    displayedCardImage.sprite = null;
                    break;
            }

        }
    }

    private void ClearDisplayedInventory()
    {
        foreach (GameObject cardObject in instantiatedCards)
        {
            Destroy(cardObject);
        }

        instantiatedCards = new List<GameObject>();
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (currentDisplayedCharacter == null)
        {
            return;
        }

        if (currentDisplayedCharacter.inventory.Count >= maxInventorySize)
        {
            return;
        }

        GameObject droppedCardGameObject = eventData.pointerDrag;
        Card droppedCard = droppedCardGameObject.GetComponent<CardMonoBehaviour>().card;

        if (droppedCard.cardClass == Card.CardClass.Weapon)
        {
            foreach (Card card in currentDisplayedCharacter.inventory)
            {
                if (card.cardClass == Card.CardClass.Weapon)
                {
                    return;
                }
            }
        }

        currentDisplayedCharacter.inventory.Add(droppedCard);
        DisplayInventory(currentDisplayedCharacter);
        CardManager.instance.cards.Remove(droppedCard);
        CardManager.instance.cardsInScene.Remove(droppedCardGameObject);
        InfoDisplayer.UpdateDisplay(currentDisplayedCharacter);
        Destroy(droppedCardGameObject);
    }
}
