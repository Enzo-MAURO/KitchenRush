using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class DropZone : MonoBehaviour, IDropHandler
{
    public Transform plateContent;

    public void OnDrop(PointerEventData eventData)
    {
        DraggableItem item = eventData.pointerDrag.GetComponent<DraggableItem>();

        if (item == null) return;

        GameManager gm = FindObjectOfType<GameManager>();

        if (gm != null)
        {
            gm.AddIngredient(item.ingredientName);

            if (item.ingredientName == "Steak Cuit")
            {
                gm.CookedSteakTaken();
            }

            if (item.ingredientName == "Steak Brûlé")
            {
                gm.BurntSteakTaken();
            }
        }

        GameObject visualItem = new GameObject(item.ingredientName);
        visualItem.transform.SetParent(plateContent, false);

        Image img = visualItem.AddComponent<Image>();
        img.color = item.GetComponent<Image>().color;

        RectTransform rt = visualItem.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(120, 40);

        GameObject textObj = new GameObject("Text");
        textObj.transform.SetParent(visualItem.transform, false);

        TMP_Text txt = textObj.AddComponent<TextMeshProUGUI>();
        txt.text = item.ingredientName;
        txt.alignment = TextAlignmentOptions.Center;
        txt.fontSize = 22;

        RectTransform textRt = textObj.GetComponent<RectTransform>();
        textRt.anchorMin = Vector2.zero;
        textRt.anchorMax = Vector2.one;
        textRt.offsetMin = Vector2.zero;
        textRt.offsetMax = Vector2.zero;

        Debug.Log("Déposé dans l'assiette : " + item.ingredientName);
    }
}