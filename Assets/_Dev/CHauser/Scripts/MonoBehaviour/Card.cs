using UnityEngine;
using UnityEngine.EventSystems;
using ZinklofDev.Utils.MathZ;

public class Card : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    RectTransform rectTransform;
    TurnManager turnManager;
    Actor currentSelectedActor;
    bool actorFound = false;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        turnManager = FindFirstObjectByType<TurnManager>();
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        
    }
    public void OnBeginDrag(PointerEventData eventData)
    {

    }
    public void OnEndDrag(PointerEventData eventData)
    {
        if (!Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit))
            return;

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
            // Attatch item to Actor
        }
    }
    public void OnDrag(PointerEventData eventData)
    {
        if(turnManager.currentTurn == TurnManager.Turn.EnemyTurn)
            return;

        rectTransform.anchoredPosition += eventData.delta;
    }
}
