using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DropZone : MonoBehaviour, IDropHandler
{
    public Transform plateContent;

    public Sprite painBasSprite;
    public Sprite painHautSprite;
    public Sprite steakSprite;
    public Sprite fromageSprite;
    public Sprite tomateSprite;
    public Sprite saladeSprite;
    public Sprite steakBruleSprite;

    private bool hasPain = false;
    private bool hasSteak = false;
    private bool hasFromage = false;
    private bool hasTomate = false;
    private bool hasSalade = false;
    private bool hasSteakBrule = false;

    public void OnDrop(PointerEventData eventData)
    {
        DraggableItem item = GetDraggedItem(eventData);
        if (item == null) return;

        string ingredient = item.ingredientName;

        GameManager gm = FindObjectOfType<GameManager>();

        if (gm != null)
        {
            gm.AddIngredient(ingredient);
        }

        AddVisualIngredient(ingredient);
        item.MarkDropped();

        if (gm != null)
        {
            if (ingredient == "Steak Cuit")
                gm.CookedSteakTaken();

            if (ingredient == "Steak Brûlé")
                gm.BurntSteakTaken();

            if (ingredient == "Frites")
                gm.FriesTaken();
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

    void AddVisualIngredient(string ingredient)
    {
        if (ingredient == "Pain") hasPain = true;
        if (ingredient == "Steak Cuit") hasSteak = true;
        if (ingredient == "Fromage") hasFromage = true;
        if (ingredient == "Tomate") hasTomate = true;
        if (ingredient == "Salade") hasSalade = true;
        if (ingredient == "Steak Brûlé") hasSteakBrule = true;

        RebuildBurgerVisual();
    }

    void RebuildBurgerVisual()
    {
        ClearVisualObjectsOnly();

        float y = -80f;

        if (hasPain)
        {
            CreateLayer("Pain Bas", painBasSprite, new Vector2(190, 45), y);
            y += 28f;
        }

        if (hasSteak)
        {
            CreateLayer("Steak Cuit", steakSprite, new Vector2(175, 42), y);
            y += 30f;
        }

        if (hasSteakBrule)
        {
            CreateLayer("Steak Brûlé", steakBruleSprite, new Vector2(175, 42), y);
            y += 30f;
        }

        if (hasFromage)
        {
            CreateLayer("Fromage", fromageSprite, new Vector2(190, 34), y);
            y += 24f;
        }

        if (hasTomate)
        {
            CreateLayer("Tomate", tomateSprite, new Vector2(165, 32), y);
            y += 24f;
        }

        if (hasSalade)
        {
            CreateLayer("Salade", saladeSprite, new Vector2(195, 35), y);
            y += 26f;
        }

        if (hasPain)
        {
            CreateLayer("Pain Haut", painHautSprite, new Vector2(200, 60), y);
        }
    }

    void CreateLayer(string name, Sprite sprite, Vector2 size, float y)
    {
        if (sprite == null)
        {
            Debug.LogWarning("Sprite manquant : " + name);
            return;
        }

        GameObject obj = new GameObject(name);
        obj.transform.SetParent(plateContent, false);

        Image img = obj.AddComponent<Image>();
        img.sprite = sprite;
        img.preserveAspect = true;
        img.raycastTarget = false;
        img.color = Color.white;

        RectTransform rt = obj.GetComponent<RectTransform>();
        rt.sizeDelta = size;
        rt.anchoredPosition = new Vector2(0, y);
    }

    void ClearVisualObjectsOnly()
    {
        foreach (Transform child in plateContent)
            Destroy(child.gameObject);
    }

    public void ClearVisualPlate()
    {
        hasPain = false;
        hasSteak = false;
        hasFromage = false;
        hasTomate = false;
        hasSalade = false;
        hasSteakBrule = false;

        ClearVisualObjectsOnly();
    }
}