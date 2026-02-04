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
                case Card.ItemType.Sword:
                    displayedCardImage.sprite = CardManager.instance.swordSprite;
                    break;
                case Card.ItemType.Axe:
                    displayedCardImage.sprite = CardManager.instance.axeSprite;
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

        foreach (Card card in currentDisplayedCharacter.inventory)
        {
            if (card.cardClass == Card.CardClass.Weapon)
            {
                return;
            }
        }

        GameObject droppedCard = eventData.pointerDrag;
        currentDisplayedCharacter.inventory.Add(droppedCard.GetComponent<CardMonoBehaviour>().card);
        DisplayInventory(currentDisplayedCharacter);
        CardManager.instance.cards.Remove(droppedCard.GetComponent<CardMonoBehaviour>().card);
        CardManager.instance.cardsInScene.Remove(droppedCard);
        InfoDisplayer.UpdateDisplay(currentDisplayedCharacter);
        Destroy(droppedCard);
    }
}
