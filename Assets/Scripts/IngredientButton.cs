using UnityEngine;

public class IngredientButton : MonoBehaviour
{
    public string ingredientName;

    public void OnClick()
    {
        GameManager gm = FindObjectOfType<GameManager>();

        if (gm != null)
        {
            gm.AddIngredient(ingredientName);
        }
    }
}