using UnityEngine;
using UnityEngine.EventSystems;

public class PanDropZone : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        // On récupère l'objet drag
        DraggableItem item = eventData.pointerDrag.GetComponent<DraggableItem>();

        // Si c'est bien un item et que c'est un steak
        if (item != null && item.ingredientName == "Steak")
        {
            GameManager gm = FindObjectOfType<GameManager>();

            if (gm != null)
            {
                gm.StartSteakCooking();
            }

            Debug.Log("?? Steak déposé dans la poêle");
        }
    }
}