using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour, IDataPersistence
{
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private GameObject content;
    [SerializeField] private GameObject baseCanvas;

    [SerializeField] public Sprite axeSprite; // TEMP
    [SerializeField] public Sprite swordSprite; // TEMP

    public List<Card> cards = new List<Card>();
    public List<GameObject> cardsInScene = new List<GameObject>();

    public static CardManager instance;

    private void Start()
    {
        instance = this;

        if (CardInfoDisplayer.instance == null)
        {
            CardInfoDisplayer.instance = FindFirstObjectByType<CardInfoDisplayer>(FindObjectsInactive.Include);
        }
        if(InventoryDisplay.instance == null)
        {
            InventoryDisplay.instance = FindFirstObjectByType<InventoryDisplay>(FindObjectsInactive.Include);
        }
    }

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
    
    public void PushCardsToScene()
    {
        foreach (GameObject card in cardsInScene)
        {
            Destroy(card);
        }

        cardsInScene = new List<GameObject>();

        foreach (Card card in cards)
        {
            InstantiateCard(card);
        }
    }

    private void InstantiateCard(Card card)
    {
        GameObject cardGameObject = Instantiate(cardPrefab, content.transform);
        cardsInScene.Add(cardGameObject);

        CardMonoBehaviour cardMonoBehaviour = cardGameObject.GetComponent<CardMonoBehaviour>();
        cardMonoBehaviour.contentParent = content;
        cardMonoBehaviour.card = card;
        cardMonoBehaviour.baseCanvas = baseCanvas;
        cardMonoBehaviour.itemType = card.itemType;

        switch(card.itemType)
        {
            case Card.ItemType.Axe:
                cardMonoBehaviour.cardImageObject.sprite = axeSprite;
                break;
            case Card.ItemType.Sword:
                cardMonoBehaviour.cardImageObject.sprite = swordSprite;
                break;
        }
    }
}
