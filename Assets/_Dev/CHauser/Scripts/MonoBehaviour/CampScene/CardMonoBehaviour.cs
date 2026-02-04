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
    CanvasGroup canvasGroup;

    [SerializeField] public Image cardImageObject;

    private void Start()
    {
        rt = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        gameObject.transform.SetParent(baseCanvas.transform);
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rt.anchoredPosition += eventData.delta / baseCanvas.GetComponent<Canvas>().scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        gameObject.transform.SetParent(contentParent.transform);
        canvasGroup.blocksRaycasts = true;
    }

    public void DisplayInfo()
    {
        CardInfoDisplayer.instance.gameObject.SetActive(true);
        CardInfoDisplayer.DisplayCardInfo(card);
    }
}
