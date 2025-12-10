using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    public static List<Card> Cards = new List<Card>();
    static List<Vector2> positions = new List<Vector2>();
    [SerializeField] private float xSpacing;
    private void Start()
    {
        float x = 100;
        float y = 0;
        int i = 0;

        Cards = Cards.OrderBy(x=>x.cardNumberInHand).ToList();

        foreach (Card card in Cards)
        {
            y = card.transform.lossyScale.y / 100 * 2 + 60;
            positions.Add(new Vector2(x, y));
            x += card.transform.lossyScale.x / 100 * 2 + xSpacing; // Card distance
            RectTransform rect = card.GetComponent<RectTransform>();
            rect.position = positions[i]; i++;
        }
    }

    public static void SnapCardBack(Card card)
    {
        RectTransform rect = card.GetComponent<RectTransform>();
        rect.position = positions[Cards.IndexOf(card)];
    }
    
    public static void SnapAllBack()
    {
        foreach (Card card in Cards)
        {
            RectTransform rect = card.GetComponent<RectTransform>();
            rect.position = positions[Cards.IndexOf(card)];
        }
    }
}
