using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour, IDataPersistence
{
    [SerializeField] private GameObject cardPrefabSword;
    [SerializeField] private GameObject cardPrefabAxe;

    [SerializeField] private GameObject content;
    [SerializeField] private GameObject baseCanvas;

    private List<Card> cards = new List<Card>();
    private List<GameObject> cardsInScene = new List<GameObject>();

    public void LoadData(GameData data)
    {
        cards = data.cards;

        foreach (GameObject card in cardsInScene)
        {
            Destroy(card);
        }

        PushCardsToScene();
    }

    public void SaveData(ref GameData data)
    {
        data.cards = cards;
    }
    
    private void PushCardsToScene()
    {
        cardsInScene = new List<GameObject>();

        foreach (Card card in cards)
        {
            GameObject cardGameObject;
            
            switch (card.itemType)
            {
                case Card.ItemType.Sword:
                    cardGameObject = Instantiate(cardPrefabSword, content.transform);
                    SetCardMonoBehaviourVariables(cardGameObject, card);
                    cardsInScene.Add(cardGameObject);
                    break;

                case Card.ItemType.Axe:
                    cardGameObject = Instantiate(cardPrefabAxe, content.transform);
                    SetCardMonoBehaviourVariables(cardPrefabAxe, card);
                    cardsInScene.Add(cardGameObject);
                    break;

                case Card.ItemType.Null:
                    Debug.Log("Card Type is Null!");
                    break;
            }
        }
    }

    private void SetCardMonoBehaviourVariables(GameObject cardGameObject, Card card)
    {
        CardMonoBehaviour cardMonoBehaviour = cardGameObject.GetComponent<CardMonoBehaviour>();
        cardMonoBehaviour.contentParent = content;
        cardMonoBehaviour.card = card;
        cardMonoBehaviour.baseCanvas = baseCanvas;
    }
}
