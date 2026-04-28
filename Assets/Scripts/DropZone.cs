using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DropZone : MonoBehaviour, IDropHandler
{
    [Header("Zone burger")]
    public Transform plateContent;

    [Header("Sprites burger")]
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

    private GameObject painBasObj;
    private GameObject painHautObj;
    private GameObject steakObj;
    private GameObject fromageObj;
    private GameObject tomateObj;
    private GameObject saladeObj;
    private GameObject steakBruleObj;

    public void OnDrop(PointerEventData eventData)
    {
        DraggableItem item = eventData.pointerDrag.GetComponent<DraggableItem>();
        if (item == null) return;

        GameManager gm = FindObjectOfType<GameManager>();

        if (gm != null)
        {
            gm.AddIngredient(item.ingredientName);

            if (item.ingredientName == "Steak Cuit")
                gm.CookedSteakTaken();

            if (item.ingredientName == "Steak Brûlé")
                gm.BurntSteakTaken();
        }

        AddVisualIngredient(item.ingredientName);

        item.MarkDropped();

        Debug.Log("Déposé dans l'assiette : " + item.ingredientName);
    }

    void AddVisualIngredient(string ingredient)
    {
        if (ingredient == "Pain")
        {
            hasPain = true;
        }
        else if (ingredient == "Steak Cuit")
        {
            hasSteak = true;
        }
        else if (ingredient == "Fromage")
        {
            hasFromage = true;
        }
        else if (ingredient == "Tomate")
        {
            hasTomate = true;
        }
        else if (ingredient == "Salade")
        {
            hasSalade = true;
        }
        else if (ingredient == "Steak Brûlé")
        {
            hasSteakBrule = true;
        }

        RebuildBurgerVisual();
    }

    void RebuildBurgerVisual()
    {
        ClearVisualObjectsOnly();

        float y = -80f;

        if (hasPain)
        {
            painBasObj = CreateLayer("Pain Bas", painBasSprite, new Vector2(180, 45), y);
            y += 28f;
        }

        if (hasSteak)
        {
            steakObj = CreateLayer("Steak", steakSprite, new Vector2(170, 42), y);
            y += 30f;
        }

        if (hasSteakBrule)
        {
            steakBruleObj = CreateLayer("Steak Brûlé", steakBruleSprite, new Vector2(170, 42), y);
            y += 30f;
        }

        if (hasFromage)
        {
            fromageObj = CreateLayer("Fromage", fromageSprite, new Vector2(185, 34), y);
            y += 24f;
        }

        if (hasTomate)
        {
            tomateObj = CreateLayer("Tomate", tomateSprite, new Vector2(165, 32), y);
            y += 24f;
        }

        if (hasSalade)
        {
            saladeObj = CreateLayer("Salade", saladeSprite, new Vector2(190, 35), y);
            y += 26f;
        }

        if (hasPain)
        {
            painHautObj = CreateLayer("Pain Haut", painHautSprite, new Vector2(190, 60), y);
        }
    }

    GameObject CreateLayer(string objectName, Sprite sprite, Vector2 size, float yPosition)
    {
        GameObject layer = new GameObject(objectName);
        layer.transform.SetParent(plateContent, false);

        Image img = layer.AddComponent<Image>();
        img.sprite = sprite;
        img.preserveAspect = true;

        RectTransform rt = layer.GetComponent<RectTransform>();
        rt.sizeDelta = size;
        rt.anchoredPosition = new Vector2(0, yPosition);

        return layer;
    }

    void ClearVisualObjectsOnly()
    {
        foreach (Transform child in plateContent)
        {
            Destroy(child.gameObject);
        }
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