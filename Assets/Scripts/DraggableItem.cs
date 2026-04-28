using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public string ingredientName;

    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Canvas canvas;
    private bool createdFromSource = false;
    private bool droppedSuccessfully = false;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        canvas = GetComponentInParent<Canvas>();

        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
    }

    public void StartDragFromSource()
    {
        createdFromSource = true;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        droppedSuccessfully = false;
        canvasGroup.blocksRaycasts = false;
        MoveToMouse(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        MoveToMouse(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;

        if (createdFromSource && !droppedSuccessfully)
        {
            Destroy(gameObject);
        }
    }

    private void MoveToMouse(PointerEventData eventData)
    {
        RectTransform canvasRect = canvas.transform as RectTransform;

        Camera cam = null;
        if (canvas.renderMode != RenderMode.ScreenSpaceOverlay)
            cam = canvas.worldCamera;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect,
            eventData.position,
            cam,
            out Vector2 localPoint
        );

        rectTransform.anchoredPosition = localPoint;
    }

    public void MarkDropped()
    {
        droppedSuccessfully = true;

        if (createdFromSource)
        {
            Destroy(gameObject);
        }
    }
}