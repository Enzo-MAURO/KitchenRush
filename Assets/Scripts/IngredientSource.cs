using UnityEngine;
using UnityEngine.EventSystems;

public class IngredientSource : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public GameObject ingredientPrefab;
    public Canvas canvas;

    private DraggableItem currentDraggedItem;

    public void OnBeginDrag(PointerEventData eventData)
    {
        GameObject newItem = Instantiate(ingredientPrefab, canvas.transform);

        currentDraggedItem = newItem.GetComponent<DraggableItem>();

        if (currentDraggedItem != null)
        {
            currentDraggedItem.StartDragFromSource();
            currentDraggedItem.OnBeginDrag(eventData);
            eventData.pointerDrag = newItem;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (currentDraggedItem != null)
        {
            currentDraggedItem.OnDrag(eventData);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (currentDraggedItem != null)
        {
            currentDraggedItem.OnEndDrag(eventData);
            currentDraggedItem = null;
        }
    }
}