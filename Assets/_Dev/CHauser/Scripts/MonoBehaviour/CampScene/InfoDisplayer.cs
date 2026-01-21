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


    public static InfoDisplayer instance;

    public static void UpdateDisplay(PlayableCharacter character)
    {
        instance.nameTxt.text = "Name: " + character.name;
        instance.backgroundTxt.text = character.background;
        instance.healthTxt.text = "Health: " + character.health;
        instance.speedTxt.text = "Speed: " + character.speed;
        instance.rangeTxt.text = "Range: " + character.range;
        instance.attackPowerTxt.text = "Attack Power: " + character.attackPower;
    }
}
