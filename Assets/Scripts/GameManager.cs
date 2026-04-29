using UnityEngine;
using UnityEngine.UI;
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
    public TMP_Text comboText;
    public TMP_Text feedbackText;

    [Header("Game Over")]
    public GameObject gameOverPanel;
    public TMP_Text gameOverText;

    [Header("Plate")]
    public DropZone plateDropZone;

    [Header("Steak Visuals In Pan")]
    public Slider cookingSlider;
    public Image cookingFillImage;
    public GameObject cookingSteakItem;
    public GameObject cookedSteakItem;
    public GameObject burntSteakItem;

    [Header("Steak Drag Prefabs")]
    public GameObject steakCuitPrefab;
    public GameObject steakBrulePrefab;
    public Transform steakSpawnParent;
    public RectTransform steakSpawnPoint;

    [Header("Fryer")]
    public GameObject normalBackground;
    public GameObject fryerCookingBackground;
    public GameObject fryerReadyBackground;
    public GameObject friesReadyItem;
    public float friesCookingTime = 15f;
    public int maxFriesPortions = 3;

    private float friesTimer = 0f;
    private bool friesCooking = false;
    private int friesPortionsLeft = 0;

    private List<string> currentIngredients = new List<string>();
    private List<string> targetRecipe = new List<string>();

    private int score = 0;
    private int comboCount = 0;
    private int maxComboBonus = 25;
    private int trashCount = 0;

    private float timeLeft = 60f;
    private bool isGameOver = false;

    private bool isCookingSteak = false;
    private bool cookedSteakReady = false;
    private bool burntSteakReady = false;
    private float cookingValue = 0f;
    private float cookedSteakTimer = 5f;

    private GameObject currentSpawnedSteak;

    void Start()
    {
        scoreText.text = "0";
        timerText.text = FormatTime(timeLeft);
        UpdateComboText();

        if (feedbackText != null)
            feedbackText.text = "";

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        ResetSteak();

        if (friesReadyItem != null)
            friesReadyItem.SetActive(false);

        if (normalBackground != null)
            normalBackground.SetActive(true);

        if (fryerCookingBackground != null)
            fryerCookingBackground.SetActive(false);

        if (fryerReadyBackground != null)
            fryerReadyBackground.SetActive(false);

        if (cookingSlider != null)
        {
            cookingSlider.value = 0f;
            cookingSlider.maxValue = 100f;
        }

        GenerateRandomRecipe();
    }

    void Update()
    {
        if (isGameOver) return;

        UpdateTimer();
        UpdateSteakCooking();
        UpdateCookedSteakTimer();
        UpdateFryer();
    }

    string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        return minutes.ToString("00") + ":" + seconds.ToString("00");
    }

    void UpdateTimer()
    {
        if (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
            timerText.text = FormatTime(timeLeft);
        }
        else
        {
            EndGame();
        }
    }

    void UpdateComboText()
    {
        if (comboText == null) return;

        if (comboCount <= 0)
            comboText.text = "x0";
        else
            comboText.text = "x" + comboCount;
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

        List<string> currentCopy = new List<string>(currentIngredients);
        List<string> targetCopy = new List<string>(targetRecipe);

        currentCopy.Sort();
        targetCopy.Sort();

        bool correct = currentCopy.Count == targetCopy.Count;

        if (correct)
        {
            for (int i = 0; i < targetCopy.Count; i++)
            {
                if (currentCopy[i] != targetCopy[i])
                {
                    correct = false;
                    break;
                }
            }
        }

        if (correct)
        {
            comboCount++;

            int points = 10 + ((comboCount - 1) * 5);
            points = Mathf.Min(points, maxComboBonus);

            score += points;
            scoreText.text = score.ToString();
            UpdateComboText();

            ShowFeedback("Bonne recette +" + points, Color.green);

            ClearPlateAndCooking();
            GenerateRandomRecipe();
        }
        else
        {
            comboCount = 0;
            trashCount = 0;

            score = Mathf.Max(0, score - 5);
            scoreText.text = score.ToString();
            UpdateComboText();

            ShowFeedback("Mauvaise recette -5", Color.red);

            ClearPlateAndCooking();
        }
    }

    void GenerateRandomRecipe()
{
    targetRecipe.Clear();

    targetRecipe.Add("Pain");

    if (Random.value > 0.5f) targetRecipe.Add("Steak Cuit");
    if (Random.value > 0.5f) targetRecipe.Add("Fromage");
    if (Random.value > 0.5f) targetRecipe.Add("Tomate");
    if (Random.value > 0.5f) targetRecipe.Add("Salade");

    // Frites parfois demandées
    if (Random.value > 0.6f) targetRecipe.Add("Frites");

    orderText.text = string.Join(" + ", targetRecipe);
}

    void ClearPlateAndCooking()
    {
        currentIngredients.Clear();

        if (plateDropZone != null)
            plateDropZone.ClearVisualPlate();

        ResetSteak();
    }

    public void StartSteakCooking()
    {
        if (isGameOver) return;
        if (isCookingSteak || cookedSteakReady || burntSteakReady) return;

        ClearSpawnedSteak();

        isCookingSteak = true;
        cookedSteakReady = false;
        burntSteakReady = false;

        cookingValue = 0f;
        cookedSteakTimer = 5f;

        if (cookingSlider != null)
            cookingSlider.value = 0f;

        if (cookingSteakItem != null)
            cookingSteakItem.SetActive(true);

        if (cookedSteakItem != null)
            cookedSteakItem.SetActive(false);

        if (burntSteakItem != null)
            burntSteakItem.SetActive(false);

        UpdateCookingBarColor();
    }

    void UpdateSteakCooking()
    {
        if (!isCookingSteak) return;

        cookingValue += Time.deltaTime * 35f;

        if (cookingSlider != null)
            cookingSlider.value = cookingValue;

        UpdateCookingBarColor();

        if (cookingValue >= 100f)
            SteakReady();
    }

    void SteakReady()
    {
        isCookingSteak = false;
        cookedSteakReady = true;
        burntSteakReady = false;
        cookedSteakTimer = 5f;

        if (cookingSteakItem != null)
            cookingSteakItem.SetActive(false);

        if (cookedSteakItem != null)
            cookedSteakItem.SetActive(false);

        if (burntSteakItem != null)
            burntSteakItem.SetActive(false);

        ClearSpawnedSteak();
        SpawnSteakPrefab(steakCuitPrefab);

        UpdateCookingBarColor();
    }

    void UpdateCookedSteakTimer()
    {
        if (!cookedSteakReady) return;

        cookedSteakTimer -= Time.deltaTime;

        if (cookedSteakTimer <= 0f)
            BurnSteak();
    }

    void BurnSteak()
    {
        isCookingSteak = false;
        cookedSteakReady = false;
        burntSteakReady = true;

        if (cookingSteakItem != null)
            cookingSteakItem.SetActive(false);

        if (cookedSteakItem != null)
            cookedSteakItem.SetActive(false);

        if (burntSteakItem != null)
            burntSteakItem.SetActive(false);

        if (cookingSlider != null)
            cookingSlider.value = 100f;

        ClearSpawnedSteak();
        SpawnSteakPrefab(steakBrulePrefab);

        UpdateCookingBarColor();
    }

    void SpawnSteakPrefab(GameObject prefab)
    {
        if (prefab == null)
        {
            Debug.LogError("Prefab steak manquant");
            return;
        }

        Transform parent = steakSpawnParent != null ? steakSpawnParent : transform.root;

        currentSpawnedSteak = Instantiate(prefab, parent);
        currentSpawnedSteak.SetActive(true);
        currentSpawnedSteak.transform.SetAsLastSibling();

        RectTransform rt = currentSpawnedSteak.GetComponent<RectTransform>();

        if (rt != null && steakSpawnPoint != null)
        {
            rt.position = steakSpawnPoint.position;
            rt.sizeDelta = steakSpawnPoint.sizeDelta;
        }

        CanvasGroup cg = currentSpawnedSteak.GetComponent<CanvasGroup>();
        if (cg != null)
        {
            cg.alpha = 1f;
            cg.interactable = true;
            cg.blocksRaycasts = true;
        }

        DraggableItem drag = currentSpawnedSteak.GetComponent<DraggableItem>();
        if (drag != null)
        {
            drag.destroyAfterDrop = true;
        }

        Debug.Log("Steak spawned : " + currentSpawnedSteak.name);
    }

    void ClearSpawnedSteak()
    {
        if (currentSpawnedSteak != null)
        {
            Destroy(currentSpawnedSteak);
            currentSpawnedSteak = null;
        }
    }

    public void CookedSteakTaken()
    {
        ResetSteak();
    }

    public void BurntSteakTaken()
    {
        ResetSteak();
    }

    void ResetSteak()
    {
        isCookingSteak = false;
        cookedSteakReady = false;
        burntSteakReady = false;

        cookingValue = 0f;
        cookedSteakTimer = 5f;

        ClearSpawnedSteak();

        if (cookingSlider != null)
            cookingSlider.value = 0f;

        if (cookingSteakItem != null)
            cookingSteakItem.SetActive(false);

        if (cookedSteakItem != null)
            cookedSteakItem.SetActive(false);

        if (burntSteakItem != null)
            burntSteakItem.SetActive(false);

        UpdateCookingBarColor();
    }

    void UpdateCookingBarColor()
    {
        if (cookingFillImage == null) return;

        if (burntSteakReady)
            cookingFillImage.color = Color.black;
        else if (cookedSteakReady)
            cookingFillImage.color = Color.green;
        else if (isCookingSteak)
            cookingFillImage.color = Color.yellow;
        else
            cookingFillImage.color = Color.white;
    }

    public void StartFryer()
    {
        if (isGameOver) return;
        if (friesCooking) return;
        if (friesPortionsLeft > 0) return;

        friesCooking = true;
        friesTimer = friesCookingTime;
        friesPortionsLeft = 0;

        if (normalBackground != null)
            normalBackground.SetActive(false);

        if (fryerReadyBackground != null)
            fryerReadyBackground.SetActive(false);

        if (fryerCookingBackground != null)
            fryerCookingBackground.SetActive(true);

        if (friesReadyItem != null)
            friesReadyItem.SetActive(false);

        ShowFeedback("Frites en cuisson", Color.yellow);
    }

    void UpdateFryer()
    {
        if (!friesCooking) return;

        friesTimer -= Time.deltaTime;

        if (friesTimer <= 0f)
        {
            friesCooking = false;
            friesPortionsLeft = maxFriesPortions;

            if (normalBackground != null)
                normalBackground.SetActive(false);

            if (fryerCookingBackground != null)
                fryerCookingBackground.SetActive(false);

            if (fryerReadyBackground != null)
                fryerReadyBackground.SetActive(true);

            if (friesReadyItem != null)
                friesReadyItem.SetActive(true);

            ShowFeedback("Frites pretes x" + friesPortionsLeft, Color.green);
        }
    }

    public void FriesTaken()
    {
        if (friesPortionsLeft <= 0) return;

        friesPortionsLeft--;

        ShowFeedback("Portion frites reste " + friesPortionsLeft, Color.yellow);

        if (friesPortionsLeft <= 0)
        {
            if (friesReadyItem != null)
                friesReadyItem.SetActive(false);

            if (fryerReadyBackground != null)
                fryerReadyBackground.SetActive(false);

            if (fryerCookingBackground != null)
                fryerCookingBackground.SetActive(false);

            if (normalBackground != null)
                normalBackground.SetActive(true);

            ShowFeedback("Bac frites vide", Color.yellow);
        }
    }

    public void TrashIngredient(string ingredient)
    {
        if (isGameOver) return;

        trashCount++;
        comboCount = 0;

        score = Mathf.Max(0, score - trashCount);
        scoreText.text = score.ToString();
        UpdateComboText();

        if (ingredient == "Steak Cuit")
            CookedSteakTaken();

        if (ingredient == "Steak Brûlé")
            BurntSteakTaken();

        if (ingredient == "Frites")
            FriesTaken();

        ShowFeedback("Jete -" + trashCount, Color.yellow);
    }

    void ShowFeedback(string message, Color color)
    {
        if (feedbackText == null) return;

        StopAllCoroutines();

        feedbackText.text = message;
        feedbackText.color = color;

        StartCoroutine(HideFeedback());
    }

    IEnumerator HideFeedback()
    {
        yield return new WaitForSeconds(1.5f);

        if (feedbackText != null)
            feedbackText.text = "";
    }

    void EndGame()
    {
        isGameOver = true;

        ResetSteak();

        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        if (gameOverText != null)
            gameOverText.text = "GAME OVER\n" + score;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}