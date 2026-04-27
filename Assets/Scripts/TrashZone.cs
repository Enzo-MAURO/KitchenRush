using UnityEngine;
using UnityEngine.EventSystems;

public class TrashZone : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        DraggableItem item = eventData.pointerDrag.GetComponent<DraggableItem>();

        if (item == null) return;

        GameManager gm = FindObjectOfType<GameManager>();

        if (gm != null)
        {
            gm.TrashIngredient(item.ingredientName);
        }

        Debug.Log("Jeté à la poubelle : " + item.ingredientName);
    }
}