using UnityEngine;
using UnityEngine.EventSystems;

public class CardMonoBehaviour : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [HideInInspector] public Card card;
    [HideInInspector] public GameObject contentParent;
    [HideInInspector] public GameObject baseCanvas;
    RectTransform rt;

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
}
