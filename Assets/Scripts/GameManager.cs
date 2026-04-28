using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    [Header("UI")]
    public TMP_Text orderText;
    public TMP_Text scoreText;
    public TMP_Text timerText;
    public TMP_Text feedbackText;

    [Header("Game Over")]
    public GameObject gameOverPanel;
    public TMP_Text gameOverText;

    [Header("Plate")]
    public Transform plateContent;
    public DropZone plateDropZone;

    [Header("Cooking")]
    public GameObject cookedSteakItem;
    public GameObject burntSteakItem;

    private List<string> currentIngredients = new List<string>();
    private List<string> targetRecipe = new List<string>();

    private int score = 0;
    private float timeLeft = 60f;
    private bool isGameOver = false;

    // 🔥 COMBO
    private int comboCount = 0;
    private int maxComboBonus = 25;

    // 🗑 POUBELLE
    private int trashCount = 0;

    void Start()
    {
        GenerateRandomRecipe();
        scoreText.text = "Score : 0";
    }

    void Update()
    {
        if (isGameOver) return;

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

    // 🔥 AJOUT INGREDIENT
    public void AddIngredient(string ingredient)
    {
        currentIngredients.Add(ingredient);
    }

    // 🔥 VALIDATION RECETTE
    public void ValidateRecipe()
    {
        if (currentIngredients.Count != targetRecipe.Count)
        {
            BadRecipe();
            return;
        }

        List<string> tempCurrent = new List<string>(currentIngredients);
        List<string> tempTarget = new List<string>(targetRecipe);

        tempCurrent.Sort();
        tempTarget.Sort();

        for (int i = 0; i < tempTarget.Count; i++)
        {
            if (tempCurrent[i] != tempTarget[i])
            {
                BadRecipe();
                return;
            }
        }

        // ✅ BONNE RECETTE
        comboCount++;

        int pointsToAdd = 10 + ((comboCount - 1) * 5);
        pointsToAdd = Mathf.Min(pointsToAdd, maxComboBonus);

        score += pointsToAdd;
        scoreText.text = "Score : " + score;

        ShowFeedback("✔ Combo x" + comboCount + " +" + pointsToAdd, Color.green);

        ClearPlate();
        GenerateRandomRecipe();
    }

    void BadRecipe()
    {
        comboCount = 0;
        trashCount = 0;

        score = Mathf.Max(0, score - 5);
        scoreText.text = "Score : " + score;

        ShowFeedback("❌ Mauvaise recette -5 (combo reset)", Color.red);

        ClearPlate();
    }

    // 🗑 POUBELLE
    public void TrashIngredient(string ingredient)
    {
        if (isGameOver) return;

        trashCount++;
        comboCount = 0;

        score = Mathf.Max(0, score - trashCount);
        scoreText.text = "Score : " + score;

        ShowFeedback("🗑 -" + trashCount + " (combo perdu)", Color.yellow);

        // Reset cuisson si steak
        if (ingredient == "Steak Cuit")
        {
            CookedSteakTaken();
        }

        if (ingredient == "Steak Brûlé")
        {
            BurntSteakTaken();
        }
    }

    // 🔥 RESET STEAK CUIT
    public void CookedSteakTaken()
    {
        if (cookedSteakItem != null)
            cookedSteakItem.SetActive(false);
    }

    // 🔥 RESET STEAK BRULE
    public void BurntSteakTaken()
    {
        if (burntSteakItem != null)
            burntSteakItem.SetActive(false);
    }

    // 🔥 GENERATION RECETTE
    void GenerateRandomRecipe()
    {
        targetRecipe.Clear();

        targetRecipe.Add("Pain");

        if (Random.value > 0.5f)
            targetRecipe.Add("Steak Cuit");

        if (Random.value > 0.5f)
            targetRecipe.Add("Fromage");

        if (Random.value > 0.5f)
            targetRecipe.Add("Tomate");

        if (Random.value > 0.5f)
            targetRecipe.Add("Salade");

        orderText.text = "Commande : " + string.Join(" + ", targetRecipe);
    }

    // 🔥 CLEAR ASSIETTE
    void ClearPlate()
    {
        currentIngredients.Clear();

        if (plateDropZone != null)
        {
            plateDropZone.ClearVisualPlate();
        }
    }

    // 🎯 FEEDBACK
    void ShowFeedback(string message, Color color)
    {
        StopAllCoroutines();

        feedbackText.text = message;
        feedbackText.color = color;

        StartCoroutine(HideFeedback());
    }

    IEnumerator HideFeedback()
    {
        yield return new WaitForSeconds(1.5f);
        feedbackText.text = "";
    }

    // 🔥 GAME OVER
    void EndGame()
    {
        isGameOver = true;

        gameOverPanel.SetActive(true);
        gameOverText.text = "GAME OVER\nScore : " + score;
    }

    // 🔄 RESTART
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}