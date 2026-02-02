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
    }
}
