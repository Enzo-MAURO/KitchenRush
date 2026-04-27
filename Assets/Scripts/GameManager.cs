using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("UI")]
    public TMP_Text orderText;
    public TMP_Text scoreText;
    public TMP_Text timerText;

    [Header("Game Over")]
    public GameObject gameOverPanel;
    public TMP_Text gameOverText;

    [Header("Plate")]
    public Transform plateContent;

    [Header("Cooking")]
    public Slider cookingSlider;
    public Image cookingFillImage;
    public GameObject cookedSteakItem;
    public GameObject burntSteakItem;

    private List<string> currentIngredients = new List<string>();
    private List<string> targetRecipe = new List<string>();

    private int score = 0;
    private float timeLeft = 60f;
    private bool isGameOver = false;

    private bool isCookingSteak = false;
    private bool cookedSteakReady = false;
    private bool burntSteakReady = false;

    private float cookingValue = 0f;
    private float cookedSteakTimer = 5f;

    private List<List<string>> recipes = new List<List<string>>()
    {
        new List<string>() { "Pain", "Steak Cuit", "Fromage" },
        new List<string>() { "Pain", "Steak Cuit", "Salade" },
        new List<string>() { "Pain", "Fromage", "Tomate" },
        new List<string>() { "Salade", "Tomate", "Fromage" }
    };

    void Start()
    {
        scoreText.text = "Score : 0";
        timerText.text = "Temps : 60";

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        if (cookedSteakItem != null)
            cookedSteakItem.SetActive(false);

        if (burntSteakItem != null)
            burntSteakItem.SetActive(false);

        if (cookingSlider != null)
        {
            cookingSlider.minValue = 0f;
            cookingSlider.maxValue = 100f;
            cookingSlider.value = 0f;
        }

        UpdateCookingBarColor();
        GenerateRandomRecipe();
    }

    void Update()
    {
        if (isGameOver) return;

        UpdateTimer();
        UpdateSteakCooking();
        UpdateCookedSteakTimer();
    }

    void UpdateTimer()
    {
        if (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
            timerText.text = "Temps : " + Mathf.Ceil(timeLeft);
        }
        else
        {
            EndGame();
        }
    }

    void UpdateSteakCooking()
    {
        if (!isCookingSteak) return;

        cookingValue += Time.deltaTime * 35f;
        cookingSlider.value = cookingValue;
        UpdateCookingBarColor();

        if (cookingValue >= 100f)
        {
            SteakReady();
        }
    }

    void UpdateCookedSteakTimer()
    {
        if (!cookedSteakReady) return;

        cookedSteakTimer -= Time.deltaTime;

        if (cookedSteakTimer <= 0f)
        {
            BurnSteak();
        }
    }

    public void AddIngredient(string ingredient)
    {
        if (isGameOver) return;

        currentIngredients.Add(ingredient);
        Debug.Log("Liste actuelle : " + string.Join(", ", currentIngredients));
    }

    public void ValidateRecipe()
    {
        if (isGameOver) return;

        bool recipeIsCorrect = CompareRecipesWithoutOrder(currentIngredients, targetRecipe);

        if (recipeIsCorrect)
        {
            Debug.Log("✅ Bonne recette !");
            score += 10;
            scoreText.text = "Score : " + score;

            ClearPlate();
            GenerateRandomRecipe();
        }
        else
        {
            Debug.Log("❌ Mauvaise recette");
            score = Mathf.Max(0, score - 5);
            scoreText.text = "Score : " + score;

            ClearPlate();
        }
    }

    bool CompareRecipesWithoutOrder(List<string> playerRecipe, List<string> targetRecipe)
    {
        if (playerRecipe.Count != targetRecipe.Count)
            return false;

        List<string> playerCopy = new List<string>(playerRecipe);
        List<string> targetCopy = new List<string>(targetRecipe);

        playerCopy.Sort();
        targetCopy.Sort();

        for (int i = 0; i < targetCopy.Count; i++)
        {
            if (playerCopy[i] != targetCopy[i])
                return false;
        }

        return true;
    }

    void ClearPlate()
    {
        currentIngredients.Clear();

        if (plateContent != null)
        {
            foreach (Transform child in plateContent)
            {
                Destroy(child.gameObject);
            }
        }
    }

    void GenerateRandomRecipe()
    {
        int randomIndex;

        do
        {
            randomIndex = Random.Range(0, recipes.Count);
        }
        while (recipes[randomIndex] == targetRecipe && recipes.Count > 1);

        targetRecipe = new List<string>(recipes[randomIndex]);
        orderText.text = "Commande : " + string.Join(" + ", targetRecipe);

        Debug.Log("Nouvelle commande : " + string.Join(", ", targetRecipe));
    }

    public void StartSteakCooking()
    {
        if (isGameOver) return;
        if (isCookingSteak || cookedSteakReady || burntSteakReady) return;

        isCookingSteak = true;
        cookingValue = 0f;
        cookedSteakTimer = 5f;

        cookingSlider.value = 0f;

        if (cookedSteakItem != null)
            cookedSteakItem.SetActive(false);

        if (burntSteakItem != null)
            burntSteakItem.SetActive(false);

        UpdateCookingBarColor();

        Debug.Log("🔥 Cuisson du steak démarrée");
    }

    void SteakReady()
    {
        isCookingSteak = false;
        cookedSteakReady = true;
        burntSteakReady = false;
        cookedSteakTimer = 5f;

        cookedSteakItem.SetActive(true);

        if (burntSteakItem != null)
            burntSteakItem.SetActive(false);

        UpdateCookingBarColor();

        Debug.Log("✅ Steak cuit prêt ! Tu as 5 secondes pour le prendre");
    }

    void BurnSteak()
    {
        isCookingSteak = false;
        cookedSteakReady = false;
        burntSteakReady = true;

        if (cookedSteakItem != null)
            cookedSteakItem.SetActive(false);

        if (burntSteakItem != null)
            burntSteakItem.SetActive(true);

        cookingSlider.value = 100f;
        UpdateCookingBarColor();

        Debug.Log("❌ Steak brûlé");
    }

    public void CookedSteakTaken()
    {
        cookedSteakReady = false;
        burntSteakReady = false;

        if (cookedSteakItem != null)
            cookedSteakItem.SetActive(false);

        if (burntSteakItem != null)
            burntSteakItem.SetActive(false);

        cookingValue = 0f;
        cookedSteakTimer = 5f;
        cookingSlider.value = 0f;
        UpdateCookingBarColor();

        Debug.Log("🥩 Steak cuit récupéré");
    }

    public void BurntSteakTaken()
    {
        isCookingSteak = false;
        cookedSteakReady = false;
        burntSteakReady = false;

        if (burntSteakItem != null)
            burntSteakItem.SetActive(false);

        if (cookedSteakItem != null)
            cookedSteakItem.SetActive(false);

        cookingValue = 0f;
        cookedSteakTimer = 5f;

        if (cookingSlider != null)
            cookingSlider.value = 0f;

        UpdateCookingBarColor();

        Debug.Log("💀 Steak brûlé retiré de la poêle");
    }

    void UpdateCookingBarColor()
    {
        if (cookingFillImage == null) return;

        if (burntSteakReady)
        {
            cookingFillImage.color = Color.black;
        }
        else if (cookedSteakReady)
        {
            cookingFillImage.color = Color.green;
        }
        else if (isCookingSteak)
        {
            cookingFillImage.color = Color.yellow;
        }
        else
        {
            cookingFillImage.color = Color.white;
        }
    }
    public void TrashIngredient(string ingredient)
    {
        if (isGameOver) return;

        score = Mathf.Max(0, score - 1);
        scoreText.text = "Score : " + score;

        if (ingredient == "Steak Cuit")
        {
            CookedSteakTaken();
        }

        if (ingredient == "Steak Brûlé")
        {
            BurntSteakTaken();
        }

        Debug.Log("🗑️ " + ingredient + " jeté. -1 point");
    }
    void EndGame()
    {
        isGameOver = true;

        gameOverPanel.SetActive(true);
        gameOverText.text = "GAME OVER\nScore : " + score;

        Debug.Log("Fin du jeu");
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}