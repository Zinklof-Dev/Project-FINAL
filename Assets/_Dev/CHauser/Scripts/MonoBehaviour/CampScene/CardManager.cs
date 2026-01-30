using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour, IDatapersistence
{
    [SerializeField] private GameObject cardPrefabSword;

    [SerializeField] private GameObject content;

    public List<Card> cards;

    private List<GameObject> cardsInScene;

    public void LoadData(GameData data)
    {
        cards = data.cards;

        foreach (Card card in cards)
        {
            Debug.Log(card);
        }

        PushCardsToScene();
    }

    public void SaveData(ref GameData data)
    {
        data.cards = cards;
    }
    
    public void PushCardsToScene()
    {
        cardsInScene = new List<GameObject>();

        foreach (Card card in cards)
        {
            switch (card.itemType)
            {
                case Card.ItemType.Sword:
                    GameObject cardGameobject = Instantiate(cardPrefabSword, content.transform);
                    cardsInScene.Add(cardGameobject);
                    break;

                case Card.ItemType.Null:
                    Debug.Log("Card Type is Null!");
                    break;
            }
        }
    }
}
