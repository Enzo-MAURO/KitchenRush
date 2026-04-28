using UnityEngine;
using UnityEngine.EventSystems;

public class PanDropZone : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        DraggableItem item = GetDraggedItem(eventData);
        if (item == null) return;

        if (item.ingredientName == "Steak" || item.ingredientName == "Steak Cru")
        {
            GameManager gm = FindObjectOfType<GameManager>();

            if (gm != null)
                gm.StartSteakCooking();

            item.MarkDropped();
        }
    }

    DraggableItem GetDraggedItem(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            DraggableItem item = eventData.pointerDrag.GetComponent<DraggableItem>();
            if (item != null) return item;
        }

        return DraggableItem.CurrentDraggedItem;
    }
}