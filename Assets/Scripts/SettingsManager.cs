using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsManager : MonoBehaviour
{
    [Header("Panel")]
    public GameObject settingsPanel;

    [Header("Panels")]
    public GameObject audioPanel;
    public GameObject displayPanel;
    public GameObject gamePanel;

    [Header("Tabs Buttons")]
    public Image audioButtonImage;
    public Image displayButtonImage;
    public Image gameButtonImage;

    [Header("Tabs Text")]
    public TMP_Text audioButtonText;
    public TMP_Text displayButtonText;
    public TMP_Text gameButtonText;

    [Header("Audio")]
    public Slider masterVolumeSlider;
    public Slider musicVolumeSlider;
    public Slider sfxVolumeSlider;

    [Header("Display")]
    public TMP_Dropdown displayModeDropdown;
    public TMP_Dropdown qualityDropdown;

    [Header("Colors")]
    public Color activeButtonColor = new Color(0.9f, 0.65f, 0.15f, 1f);
    public Color inactiveButtonColor = new Color(0.25f, 0.30f, 0.48f, 1f);
    public Color activeTextColor = Color.white;
    public Color inactiveTextColor = Color.white;

    void Start()
    {
        LoadSettings();

        if (settingsPanel != null)
            settingsPanel.SetActive(false);

        ShowAudioPanel();
    }

    public void OpenSettings()
    {
        settingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
        PlayerPrefs.Save();
    }

    public void ShowAudioPanel()
    {
        audioPanel.SetActive(true);
        displayPanel.SetActive(false);
        gamePanel.SetActive(false);
        SetActiveTab(audioButtonImage, audioButtonText);
    }

    public void ShowDisplayPanel()
    {
        audioPanel.SetActive(false);
        displayPanel.SetActive(true);
        gamePanel.SetActive(false);
        SetActiveTab(displayButtonImage, displayButtonText);
    }

    public void ShowGamePanel()
    {
        audioPanel.SetActive(false);
        displayPanel.SetActive(false);
        gamePanel.SetActive(true);
        SetActiveTab(gameButtonImage, gameButtonText);
    }

    void SetActiveTab(Image activeImage, TMP_Text activeText)
    {
        audioButtonImage.color = inactiveButtonColor;
        displayButtonImage.color = inactiveButtonColor;
        gameButtonImage.color = inactiveButtonColor;

        audioButtonText.color = inactiveTextColor;
        displayButtonText.color = inactiveTextColor;
        gameButtonText.color = inactiveTextColor;

        activeImage.color = activeButtonColor;
        activeText.color = activeTextColor;
    }

    public void SetMasterVolume(float value)
    {
        AudioListener.volume = value;
        PlayerPrefs.SetFloat("MasterVolume", value);
    }

    public void SetMusicVolume(float value)
    {
        PlayerPrefs.SetFloat("MusicVolume", value);
    }

    public void SetSfxVolume(float value)
    {
        PlayerPrefs.SetFloat("SfxVolume", value);
    }

    public void OnDisplayModeDropdownChanged()
    {
        if (displayModeDropdown == null) return;

        int mode = displayModeDropdown.value;
        Debug.Log("DISPLAY MODE DROPDOWN VALUE = " + mode);

        SetDisplayMode(mode);
    }

    public void SetDisplayMode(int mode)
    {
        PlayerPrefs.SetInt("DisplayMode", mode);
        PlayerPrefs.Save();

        StartCoroutine(ApplyDisplayMode(mode));
    }

    IEnumerator ApplyDisplayMode(int mode)
    {
        yield return null;

        if (mode == 0)
        {
            Screen.fullScreenMode = FullScreenMode.Windowed;
            yield return null;
            Screen.SetResolution(1280, 720, false);
        }
        else if (mode == 1)
        {
            Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
            yield return null;
            Screen.SetResolution(Display.main.systemWidth, Display.main.systemHeight, FullScreenMode.FullScreenWindow);
        }
        else if (mode == 2)
        {
            Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
            yield return null;
            Screen.SetResolution(Display.main.systemWidth, Display.main.systemHeight, FullScreenMode.ExclusiveFullScreen);
        }

        Debug.Log("Mode applique : " + mode + " / " + Screen.fullScreenMode);
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
        PlayerPrefs.SetInt("Quality", qualityIndex);
        PlayerPrefs.Save();
    }

    public void ResetProgress()
    {
        PlayerPrefs.DeleteKey("BestScore");
        PlayerPrefs.DeleteKey("UnlockedLevel");
        PlayerPrefs.Save();

        Debug.Log("Progression reinitialisee");
    }

    void LoadSettings()
    {
        float master = PlayerPrefs.GetFloat("MasterVolume", 1f);
        float music = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
        float sfx = PlayerPrefs.GetFloat("SfxVolume", 0.7f);

        AudioListener.volume = master;

        if (masterVolumeSlider != null) masterVolumeSlider.value = master;
        if (musicVolumeSlider != null) musicVolumeSlider.value = music;
        if (sfxVolumeSlider != null) sfxVolumeSlider.value = sfx;

        int mode = PlayerPrefs.GetInt("DisplayMode", 1);

        if (displayModeDropdown != null)
            displayModeDropdown.value = mode;

        SetDisplayMode(mode);

        int quality = PlayerPrefs.GetInt("Quality", QualitySettings.GetQualityLevel());

        QualitySettings.SetQualityLevel(quality);

        if (qualityDropdown != null)
            qualityDropdown.value = quality;
    }
}