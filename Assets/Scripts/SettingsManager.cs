using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsManager : MonoBehaviour
{
    [Header("Main")]
    public GameObject settingsPanel;

    [Header("Tabs")]
    public GameObject audioPanel;
    public GameObject displayPanel;
    public GameObject gamePanel;

    [Header("Audio")]
    public Slider masterVolumeSlider;
    public Slider musicVolumeSlider;
    public Slider sfxVolumeSlider;

    [Header("Display")]
    public Toggle fullscreenToggle;
    public TMP_Dropdown qualityDropdown;

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
        SaveSettings();
    }

    public void ShowAudioPanel()
    {
        audioPanel.SetActive(true);
        displayPanel.SetActive(false);
        gamePanel.SetActive(false);
    }

    public void ShowDisplayPanel()
    {
        audioPanel.SetActive(false);
        displayPanel.SetActive(true);
        gamePanel.SetActive(false);
    }

    public void ShowGamePanel()
    {
        audioPanel.SetActive(false);
        displayPanel.SetActive(false);
        gamePanel.SetActive(true);
    }

    public void SetMasterVolume(float value)
    {
        AudioListener.volume = value;
        PlayerPrefs.SetFloat("MasterVolume", value);
    }

    public void SetMusicVolume(float value)
    {
        PlayerPrefs.SetFloat("MusicVolume", value);
        // On branchera le MusicManager après si besoin
    }

    public void SetSfxVolume(float value)
    {
        PlayerPrefs.SetFloat("SfxVolume", value);
        // On branchera l'AudioManager après si besoin
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        PlayerPrefs.SetInt("Fullscreen", isFullscreen ? 1 : 0);
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
        PlayerPrefs.SetInt("Quality", qualityIndex);
    }

    public void ResetProgress()
    {
        PlayerPrefs.DeleteKey("BestScore");
        PlayerPrefs.DeleteKey("UnlockedLevel");
        Debug.Log("Progression réinitialisée");
    }

    void SaveSettings()
    {
        PlayerPrefs.Save();
    }

    void LoadSettings()
    {
        float master = PlayerPrefs.GetFloat("MasterVolume", 1f);
        float music = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
        float sfx = PlayerPrefs.GetFloat("SfxVolume", 0.7f);
        bool fullscreen = PlayerPrefs.GetInt("Fullscreen", 1) == 1;
        int quality = PlayerPrefs.GetInt("Quality", QualitySettings.GetQualityLevel());

        AudioListener.volume = master;
        Screen.fullScreen = fullscreen;
        QualitySettings.SetQualityLevel(quality);

        if (masterVolumeSlider != null) masterVolumeSlider.value = master;
        if (musicVolumeSlider != null) musicVolumeSlider.value = music;
        if (sfxVolumeSlider != null) sfxVolumeSlider.value = sfx;
        if (fullscreenToggle != null) fullscreenToggle.isOn = fullscreen;
        if (qualityDropdown != null) qualityDropdown.value = quality;
    }
}