using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour, IDataPersistence
{
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private GameObject content;
    [SerializeField] private GameObject baseCanvas;

    [Header("TEMP")]

    [SerializeField] public Sprite axeSprite; // TEMP
    [SerializeField] public Sprite swordSprite; // TEMP

    [Header("TWO HANDED")]

    [SerializeField] public Sprite zweihanderSprite;
    [SerializeField] public Sprite daneAxeSprite;
    [SerializeField] public Sprite greatSwordSprite;
    [SerializeField] public Sprite twoHandedMaceSprite;
    [SerializeField] public Sprite twoHandedHammerSprite;

    [Header("POLEARM")]

    [SerializeField] public Sprite spearSprite;
    [SerializeField] public Sprite bardicheSprite;
    [SerializeField] public Sprite voulgeSprite;
    [SerializeField] public Sprite glaveSprite;

    [Header("ONE HANDED")]

    [SerializeField] public Sprite bastardSwordSprite;
    [SerializeField] public Sprite armingSwordSprite;
    [SerializeField] public Sprite nobleArmingSwordSprite;
    [SerializeField] public Sprite nordAxeSprite;
    [SerializeField] public Sprite battleAxeSprite;
    [SerializeField] public Sprite spikedMaceSprite;
    [SerializeField] public Sprite cavMaceSprite;
    [SerializeField] public Sprite battleHammerSprite;
    [SerializeField] public Sprite knifeSprite;

    [Header("BOWS")]

    [SerializeField] public Sprite longbowSprite;
    [SerializeField] public Sprite shortbowSprite;
    [SerializeField] public Sprite curvedBowSprite;
    [SerializeField] public Sprite huntingBowSprite;

    [Header("CROSSBOWS")]

    [SerializeField] public Sprite arbalestSprite;
    [SerializeField] public Sprite earlyCrossbowSprite;

    [Header("SPECIAL")]

    [SerializeField] public Sprite halberdSprite;

    [Header("SUPPORTER ITEMS")]

    [SerializeField] public Sprite healthSupporterSprite;
    [SerializeField] public Sprite rangeSupporterSprite;
    [SerializeField] public Sprite actionPointsSupporterSprite;


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

        switch (card.itemType)
        {
            // TEMP PLACEHOLDERS
            case Card.ItemType.Sword:
                cardMonoBehaviour.cardImageObject.sprite = swordSprite;
                break;

            case Card.ItemType.Axe:
                cardMonoBehaviour.cardImageObject.sprite = axeSprite;
                break;

            // TWO HANDED
            case Card.ItemType.Zweihander:
                cardMonoBehaviour.cardImageObject.sprite = zweihanderSprite;
                break;

            case Card.ItemType.Dane_Axe:
                cardMonoBehaviour.cardImageObject.sprite = daneAxeSprite;
                break;

            case Card.ItemType.Great_Sword:
                cardMonoBehaviour.cardImageObject.sprite = greatSwordSprite;
                break;

            case Card.ItemType.Two_Handed_Mace:
                cardMonoBehaviour.cardImageObject.sprite = twoHandedMaceSprite;
                break;

            case Card.ItemType.Two_Handed_Hammer:
                cardMonoBehaviour.cardImageObject.sprite = twoHandedHammerSprite;
                break;

            // POLEARM
            case Card.ItemType.Spear:
                cardMonoBehaviour.cardImageObject.sprite = spearSprite;
                break;

            case Card.ItemType.Bardiche:
                cardMonoBehaviour.cardImageObject.sprite = bardicheSprite;
                break;

            case Card.ItemType.Voulge:
                cardMonoBehaviour.cardImageObject.sprite = voulgeSprite;
                break;

            case Card.ItemType.Glave:
                cardMonoBehaviour.cardImageObject.sprite = glaveSprite;
                break;

            // ONE HANDED
            case Card.ItemType.Bastard_Sword:
                cardMonoBehaviour.cardImageObject.sprite = bastardSwordSprite;
                break;

            case Card.ItemType.Arming_Sword:
                cardMonoBehaviour.cardImageObject.sprite = armingSwordSprite;
                break;

            case Card.ItemType.Noble_Arming_Sword:
                cardMonoBehaviour.cardImageObject.sprite = nobleArmingSwordSprite;
                break;

            case Card.ItemType.Nord_Axe:
                cardMonoBehaviour.cardImageObject.sprite = nordAxeSprite;
                break;

            case Card.ItemType.Battle_Axe:
                cardMonoBehaviour.cardImageObject.sprite = battleAxeSprite;
                break;

            case Card.ItemType.Spiked_Mace:
                cardMonoBehaviour.cardImageObject.sprite = spikedMaceSprite;
                break;

            case Card.ItemType.Cav_Mace:
                cardMonoBehaviour.cardImageObject.sprite = cavMaceSprite;
                break;

            case Card.ItemType.Battle_Hammer:
                cardMonoBehaviour.cardImageObject.sprite = battleHammerSprite;
                break;

            case Card.ItemType.Knife:
                cardMonoBehaviour.cardImageObject.sprite = knifeSprite;
                break;

            // BOWS
            case Card.ItemType.Longbow:
                cardMonoBehaviour.cardImageObject.sprite = longbowSprite;
                break;

            case Card.ItemType.Shortbow:
                cardMonoBehaviour.cardImageObject.sprite = shortbowSprite;
                break;

            case Card.ItemType.Curved_Bow:
                cardMonoBehaviour.cardImageObject.sprite = curvedBowSprite;
                break;

            case Card.ItemType.Hunting_Bow:
                cardMonoBehaviour.cardImageObject.sprite = huntingBowSprite;
                break;

            // CROSSBOWS
            case Card.ItemType.Arbalest:
                cardMonoBehaviour.cardImageObject.sprite = arbalestSprite;
                break;

            case Card.ItemType.Early_Crossbow:
                cardMonoBehaviour.cardImageObject.sprite = earlyCrossbowSprite;
                break;

            // SPECIAL
            case Card.ItemType.Halberd:
                cardMonoBehaviour.cardImageObject.sprite = halberdSprite;
                break;

            // SUPPORTER

            case Card.ItemType.Health_Supporter:
                cardMonoBehaviour.cardImageObject.sprite = healthSupporterSprite;
                break;

            case Card.ItemType.Range_Supporter:
                cardMonoBehaviour.cardImageObject.sprite = rangeSupporterSprite;
                break;

            case Card.ItemType.Action_Points_Supporter:
                cardMonoBehaviour.cardImageObject.sprite = actionPointsSupporterSprite;
                break;
        }
    }
}
