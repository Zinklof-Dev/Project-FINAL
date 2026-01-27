using UnityEngine;
using UnityEngine.EventSystems;
using ZinklofDev.Utils.MathZ;

public class Card : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField] private Canvas canvas;

    public enum ItemType { Null, ReachPotion, HealthPotion, ActionPointBoostPotion, Axe, Sword };
    [SerializeField] public ItemType itemType;

    // Used if a tool / weapon, reach also used if reach potion
    [SerializeField] private int attackPower;
    [SerializeField] private int reach;

    // Used if potions
    [SerializeField] private int health;
    [SerializeField] private int actionPoints;

    [SerializeField] private GameObject actionPointBoostPotionPrompt;

    [SerializeField] public int cardNumberInHand;

    [SerializeField] public GameObject swordPrefab;
    [SerializeField] public GameObject axePrefab;

    RectTransform rectTransform;
    TurnManager turnManager;
    PlayerInputManager playerInputManager;
    Actor currentSelectedActor;
    bool actorFound = false;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        turnManager = FindFirstObjectByType<TurnManager>();
        playerInputManager = FindFirstObjectByType<PlayerInputManager>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        
    }
    
    public void OnBeginDrag(PointerEventData eventData)
    {

    }
    
    public void OnEndDrag(PointerEventData eventData)
    {
        switch (itemType)
        {
            case ItemType.ReachPotion:
            ReachPotionAction();
            break;
            
            case ItemType.HealthPotion:
            HealthPotionAction();
            break;
            
            case ItemType.ActionPointBoostPotion:
            ActionPointPotionAction();
            break;
            
            case ItemType.Axe:
            AxeAction();
            break;
            
            case ItemType.Sword:
            SwordAction();
            break;
        }
        
    }
    public void OnDrag(PointerEventData eventData)
    {
        if(playerInputManager.state  != PlayerInputManager.InputState.SelectingPartyMember)
            return;

        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void ReachPotionAction()
    {
        if (!Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit))
        {
            CardManager.SnapCardBack(this);
            return;
        }

        float x = Numbers.RoundToMultiple(hit.point.x, GridSystem.staticTileSize, GridSystem.staticOffsetX);
        float y = Numbers.RoundToMultiple(hit.point.z, GridSystem.staticTileSize, GridSystem.staticOffsetY);

        Vector2 selectedPosition = new Vector2(x, y);

        actorFound = false;

        foreach (Actor actor in ActorManager.partyMemberActors)
        {
            if (actor.agent.currentIndex == GridSystem.points.IndexOf(selectedPosition))
            {
                currentSelectedActor = actor;
                actorFound = true;
                break;
            }
        }
        
        if (!actorFound) 
            CardManager.SnapCardBack(this);
        else
        {
            currentSelectedActor.range += reach;
            CardManager.instance.Cards.Remove(this);
            CardManager.SnapAllBack();
            gameObject.SetActive(false);
        }
    }

    public void HealthPotionAction()
    {
        if (!Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit))
        {
            CardManager.SnapCardBack(this);
            return;
        }

        float x = Numbers.RoundToMultiple(hit.point.x, GridSystem.staticTileSize, GridSystem.staticOffsetX);
        float y = Numbers.RoundToMultiple(hit.point.z, GridSystem.staticTileSize, GridSystem.staticOffsetY);

        Vector2 selectedPosition = new Vector2(x, y);

        actorFound = false;

        foreach (Actor actor in ActorManager.partyMemberActors)
        {
            if (actor.agent.currentIndex == GridSystem.points.IndexOf(selectedPosition))
            {
                currentSelectedActor = actor;
                actorFound = true;
                break;
            }
        }
        
        if (!actorFound) 
            CardManager.SnapCardBack(this);
            
        else
        {
            currentSelectedActor.health += health;
            CardManager.instance.Cards.Remove(this);
            CardManager.SnapAllBack();
            gameObject.SetActive(false);
        }
    }

    public void ActionPointPotionAction()
    {
        actionPointBoostPotionPrompt.SetActive(true);
    }

    public void ActionPointPotion(bool use)
    {
        if (use)
        {
            turnManager.TakeAction(-actionPoints);
            CardManager.instance.Cards.Remove(this);
            CardManager.SnapAllBack();
            gameObject.SetActive(false);
            return;
        }
        
        CardManager.SnapCardBack(this);
    }

    public void AxeAction()
    {
        if (!Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit))
        {
            CardManager.SnapCardBack(this);
            return;
        }

        float x = Numbers.RoundToMultiple(hit.point.x, GridSystem.staticTileSize, GridSystem.staticOffsetX);
        float y = Numbers.RoundToMultiple(hit.point.z, GridSystem.staticTileSize, GridSystem.staticOffsetY);

        Vector2 selectedPosition = new Vector2(x, y);

        actorFound = false;

        foreach (Actor actor in ActorManager.partyMemberActors)
        {
            if (actor.agent.currentIndex == GridSystem.points.IndexOf(selectedPosition))
            {
                currentSelectedActor = actor;
                actorFound = true;
                break;
            }
        }
        
        if (!actorFound) 
            CardManager.SnapCardBack(this);
            
        else if (currentSelectedActor.equippedTool != null)
            CardManager.SnapCardBack(this);
            
        else
        {
            currentSelectedActor.equippedTool = new Item(Item.Type.Axe, attackPower, reach);
            currentSelectedActor.attackPower += attackPower; // Temp for now, only works if we can equip one weapon
            currentSelectedActor.range = reach; // Temp for now, only works if we can equip one weapon
            CardManager.instance.Cards.Remove(this);
            CardManager.SnapAllBack();
            Instantiate(axePrefab, currentSelectedActor.weaponSlot);
            gameObject.SetActive(false);
        }
    }

    public void SwordAction()
    {
        if (!Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit))
        {
            CardManager.SnapCardBack(this);
            return;
        }

        float x = Numbers.RoundToMultiple(hit.point.x, GridSystem.staticTileSize, GridSystem.staticOffsetX);
        float y = Numbers.RoundToMultiple(hit.point.z, GridSystem.staticTileSize, GridSystem.staticOffsetY);

        Vector2 selectedPosition = new Vector2(x, y);

        actorFound = false;

        foreach (Actor actor in ActorManager.partyMemberActors)
        {
            if (actor.agent.currentIndex == GridSystem.points.IndexOf(selectedPosition))
            {
                currentSelectedActor = actor;
                actorFound = true;
                break;
            }
        }
        
        if (!actorFound) 
            CardManager.SnapCardBack(this);
            
        else if (currentSelectedActor.equippedTool != null)
            CardManager.SnapCardBack(this);
            
        else
        {
            currentSelectedActor.equippedTool = new Item(Item.Type.Sword, attackPower, reach);
            currentSelectedActor.attackPower += attackPower; // Temp for now, only works if we can equip one weapon
            currentSelectedActor.range = reach; // Temp for now, only works if we can equip one weapon
            Instantiate(swordPrefab, currentSelectedActor.weaponSlot);
            CardManager.instance.Cards.Remove(this);
            CardManager.SnapAllBack();
            gameObject.SetActive(false);
        }
    }
}
