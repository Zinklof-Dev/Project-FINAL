using TMPro;
using UnityEngine;

public class InfoDisplayer : MonoBehaviour
{
    [SerializeField] public TMP_Text nameTxt = null;
    [SerializeField] public TMP_Text backgroundTxt = null;
    [SerializeField] public TMP_Text healthTxt = null;
    [SerializeField] public TMP_Text speedTxt = null;
    [SerializeField] public TMP_Text rangeTxt = null;
    [SerializeField] public TMP_Text attackPowerTxt = null;
    [SerializeField] private InventoryDisplay inventoryDisplay = null;


    public static InfoDisplayer instance;

    public static void UpdateDisplay(PlayableCharacter character)
    {
        instance.nameTxt.text = "Name: " + character.name;
        instance.backgroundTxt.text = character.background;
        instance.healthTxt.text = "Health: " + character.health;
        instance.speedTxt.text = "Speed: " + character.speed;
        instance.rangeTxt.text = "Best Range: " + instance.GetEquippedWeaponBestRange(character);
        instance.attackPowerTxt.text = "Attack Power: " + character.attackPower + " + " + instance.GetEquippedWeaponAttackPower(character).ToString();

        if(character.inventory != null)
            instance.inventoryDisplay.DisplayInventory(character);
    }

    private float GetEquippedWeaponBestRange(PlayableCharacter character)
    {
        foreach (Card card in character.inventory)
        {
            if (card.cardClass == Card.CardClass.Weapon)
                return card.bestRange;
        }

        return 0;
    }

    private float GetEquippedWeaponAttackPower(PlayableCharacter character)
    {
        foreach (Card card in character.inventory)
        {
            if (card.cardClass == Card.CardClass.Weapon)
                return card.attackPower;
        }

        return 0;
    }
}
