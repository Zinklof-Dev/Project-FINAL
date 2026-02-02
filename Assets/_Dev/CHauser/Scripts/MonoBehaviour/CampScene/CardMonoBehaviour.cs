using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardMonoBehaviour : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [HideInInspector] public Card card;
    [HideInInspector] public GameObject contentParent;
    [HideInInspector] public GameObject baseCanvas;
    [HideInInspector] public Card.ItemType itemType;
    RectTransform rt;

    [SerializeField] public Image cardImageObject;

    private void Start()
    {
        rt = GetComponent<RectTransform>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        gameObject.transform.SetParent(baseCanvas.transform);
    }

    public void OnDrag(PointerEventData eventData)
    {
        rt.anchoredPosition += eventData.delta / baseCanvas.GetComponent<Canvas>().scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        gameObject.transform.SetParent(contentParent.transform);
    }

    public void DisplayInfo()
    { 

        CardInfoDisplayer.DisplayCardInfo(card);
    }
}
