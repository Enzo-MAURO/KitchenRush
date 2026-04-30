using UnityEngine;

public class DrinkManager : MonoBehaviour
{
    public Transform spawnParent;

    public RectTransform cocaSpawnPoint;
    public RectTransform fantaSpawnPoint;
    public RectTransform spriteSpawnPoint;
    public RectTransform eauSpawnPoint;

    public GameObject cocaPrefab;
    public GameObject fantaPrefab;
    public GameObject spritePrefab;
    public GameObject eauPrefab;

    public void SpawnCoca()
    {
        SpawnDrink(cocaPrefab, cocaSpawnPoint);
    }

    public void SpawnFanta()
    {
        SpawnDrink(fantaPrefab, fantaSpawnPoint);
    }

    public void SpawnSprite()
    {
        SpawnDrink(spritePrefab, spriteSpawnPoint);
    }

    public void SpawnEau()
    {
        SpawnDrink(eauPrefab, eauSpawnPoint);
    }

    void SpawnDrink(GameObject prefab, RectTransform spawnPoint)
    {
        if (prefab == null || spawnPoint == null || spawnParent == null)
            return;

        GameObject drink = Instantiate(prefab, spawnParent);
        drink.SetActive(true);
        drink.transform.SetAsLastSibling();

        RectTransform drinkRect = drink.GetComponent<RectTransform>();

        if (drinkRect != null)
        {
            drinkRect.position = spawnPoint.position;
            drinkRect.sizeDelta = spawnPoint.sizeDelta;
        }

        CanvasGroup cg = drink.GetComponent<CanvasGroup>();
        if (cg != null)
        {
            cg.alpha = 1;
            cg.interactable = true;
            cg.blocksRaycasts = true;
        }
    }
}