using UnityEngine;

public class InventoryCardPrefabMonoBehaviour : MonoBehaviour
{
    public Card card;

    public void UnEquip()
    {
        InventoryDisplay.instance.currentDisplayedCharacter.inventory.Remove(card);
        InventoryDisplay.instance.DisplayInventory(InventoryDisplay.instance.currentDisplayedCharacter);
        CardManager.instance.cards.Add(card);
        CardManager.instance.PushCardsToScene();
        InfoDisplayer.UpdateDisplay(InventoryDisplay.instance.currentDisplayedCharacter);
    }
}
