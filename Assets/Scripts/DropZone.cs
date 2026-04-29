using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DropZone : MonoBehaviour, IDropHandler
{
    public Transform plateContent;
    public Transform friesPlateContent;

    public Sprite painBasSprite;
    public Sprite painHautSprite;
    public Sprite steakSprite;
    public Sprite fromageSprite;
    public Sprite tomateSprite;
    public Sprite saladeSprite;
    public Sprite steakBruleSprite;
    public Sprite fritesSprite;

    private bool hasPain;
    private bool hasSteak;
    private bool hasFromage;
    private bool hasTomate;
    private bool hasSalade;
    private bool hasSteakBrule;
    private bool hasFrites;

    public void OnDrop(PointerEventData eventData)
    {
        DraggableItem item = GetDraggedItem(eventData);
        if (item == null) return;

        string ingredient = item.ingredientName;

        GameManager gm = FindObjectOfType<GameManager>();
        if (gm != null)
            gm.AddIngredient(ingredient);

        if (ingredient == "Frites")
            AddFriesVisual();
        else
            AddVisualIngredient(ingredient);

        item.MarkDropped();

        if (gm != null)
        {
            if (ingredient == "Steak Cuit") gm.CookedSteakTaken();
            if (ingredient == "Steak Brûlé") gm.BurntSteakTaken();
            if (ingredient == "Frites") gm.FriesTaken();
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

    void AddFriesVisual()
    {
        hasFrites = true;

        if (friesPlateContent == null)
        {
            Debug.LogWarning("FriesPlateContent non relié");
            return;
        }

        foreach (Transform child in friesPlateContent)
            Destroy(child.gameObject);

        CreateFriesLayer();
    }

    void RebuildBurgerVisual()
    {
        ClearBurgerOnly();

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
        if (sprite == null) return;

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

    void CreateFriesLayer()
    {
        if (fritesSprite == null) return;

        GameObject obj = new GameObject("Frites");
        obj.transform.SetParent(friesPlateContent, false);

        Image img = obj.AddComponent<Image>();
        img.sprite = fritesSprite;
        img.preserveAspect = true;
        img.raycastTarget = false;
        img.color = Color.white;

        RectTransform rt = obj.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(150, 70);
        rt.anchoredPosition = Vector2.zero;
    }

    void ClearBurgerOnly()
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
        hasFrites = false;

        ClearBurgerOnly();

        if (friesPlateContent != null)
        {
            foreach (Transform child in friesPlateContent)
                Destroy(child.gameObject);
        }
    }
}