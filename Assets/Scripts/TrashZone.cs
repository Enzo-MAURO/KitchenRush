using UnityEngine;
using UnityEngine.EventSystems;

public class TrashZone : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        DraggableItem item = GetDraggedItem(eventData);
        if (item == null) return;

        string ingredient = item.ingredientName;

        GameManager gm = FindObjectOfType<GameManager>();

        item.MarkDropped();

        if (gm != null)
        {
            gm.TrashIngredient(ingredient);
        }

        Debug.Log("Jeté à la poubelle : " + ingredient);
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