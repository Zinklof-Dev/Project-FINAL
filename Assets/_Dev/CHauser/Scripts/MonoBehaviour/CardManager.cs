using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    public static List<Card> Cards = new List<Card>();
    static List<Vector2> positions = new List<Vector2>();
    private void Start()
    {
        Cards = FindObjectsByType<Card>(FindObjectsSortMode.None).ToList();

        float x = 0;
        float y = 0;
        int i = 0;

        foreach (Card card in Cards)
        {
            y = card.transform.lossyScale.y / 100 * 2 + 10;
            positions.Add(new Vector2(x, y));
            x += card.transform.lossyScale.x / 100 * 2 + 10; // Card distance
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
