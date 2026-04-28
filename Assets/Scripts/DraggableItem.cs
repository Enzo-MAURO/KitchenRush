using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public string ingredientName;
    public bool destroyAfterDrop = true;

    public static DraggableItem CurrentDraggedItem;

    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Canvas canvas;

    private Vector2 startPosition;
    private bool createdFromSource = false;
    private bool wasDropped = false;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        canvas = GetComponentInParent<Canvas>();

        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
    }

    public void SetCreatedFromSource(bool value)
    {
        createdFromSource = value;
        destroyAfterDrop = value;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        CurrentDraggedItem = this;
        wasDropped = false;
        startPosition = rectTransform.anchoredPosition;
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

        if (!wasDropped)
        {
            if (createdFromSource)
                Destroy(gameObject);
            else
                rectTransform.anchoredPosition = startPosition;
        }

        if (CurrentDraggedItem == this)
            CurrentDraggedItem = null;
    }

    void MoveToMouse(PointerEventData eventData)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            eventData.position,
            canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera,
            out Vector2 pos
        );

        rectTransform.anchoredPosition = pos;
    }

    public void MarkDropped()
    {
        wasDropped = true;
        canvasGroup.blocksRaycasts = true;

        if (CurrentDraggedItem == this)
            CurrentDraggedItem = null;

        if (destroyAfterDrop)
            Destroy(gameObject);
        else
            rectTransform.anchoredPosition = startPosition;
    }

    public void ForceResetDrag()
    {
        wasDropped = true;

        if (canvasGroup != null)
            canvasGroup.blocksRaycasts = true;

        if (CurrentDraggedItem == this)
            CurrentDraggedItem = null;

        if (rectTransform != null)
            rectTransform.anchoredPosition = startPosition;
    }
}