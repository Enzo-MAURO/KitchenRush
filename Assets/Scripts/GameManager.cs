using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject gameOverPanel;
    public TMP_Text gameOverText;
    private bool isGameOver = false;
    public TMP_Text timerText;
    private float timeLeft = 60f;
    public TMP_Text scoreText;
    private int score = 0;
    public TMP_Text orderText;

    public List<string> currentIngredients = new List<string>();
    public List<string> targetRecipe = new List<string>();

    private List<List<string>> recipes = new List<List<string>>()
    {
        new List<string>() { "Pain", "Steak", "Fromage" },
        new List<string>() { "Pain", "Steak", "Salade" },
        new List<string>() { "Pain", "Fromage", "Tomate" },
        new List<string>() { "Salade", "Tomate", "Fromage" }
    };

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
    void Start()
    {
        GenerateRandomRecipe();
        scoreText.text = "Score : 0";
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
        if (currentIngredients.Count != targetRecipe.Count)
        {
            Debug.Log("❌ Mauvaise recette");
            score = Mathf.Max(0, score - 5);
            scoreText.text = "Score : " + score;

            currentIngredients.Clear();
            return;
        }

        for (int i = 0; i < targetRecipe.Count; i++)
        {
            if (currentIngredients[i] != targetRecipe[i])
            {
                Debug.Log("❌ Mauvaise recette");
                score = Mathf.Max(0, score - 5);
                scoreText.text = "Score : " + score;

                currentIngredients.Clear();
                return;
            }
        }

        Debug.Log("✅ Bonne recette !");
        Debug.Log("✅ Bonne recette !");
        score += 10;
        scoreText.text = "Score : " + score;
        currentIngredients.Clear();
        GenerateRandomRecipe();
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