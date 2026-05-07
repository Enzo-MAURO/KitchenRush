using UnityEngine;

public class PlayerCarry : MonoBehaviour
{
    public Transform handPoint;

    public GameObject carriedObject;

    public bool IsCarrying()
    {
        return carriedObject != null;
    }

    public void PickUp(GameObject prefab)
    {
        if (carriedObject != null)
            Destroy(carriedObject);

        carriedObject = Instantiate(prefab, handPoint.position, Quaternion.identity);

        carriedObject.transform.SetParent(handPoint);

        Rigidbody rb = carriedObject.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.isKinematic = true;
        }
    }

    public string GetCarriedItemName()
    {
        if (carriedObject == null)
            return "";

        IngredientItem item = carriedObject.GetComponent<IngredientItem>();

        if (item != null)
            return item.ingredientName;

        return "";
    }
}