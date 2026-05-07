using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuNavigation : MonoBehaviour
{
    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void GoToWorldMap()
    {
        SceneManager.LoadScene("WorldMap");
    }

    public void GoToCH1()
    {
        SceneManager.LoadScene("CH1map");
    }

    public void GoToCH2()
    {
        SceneManager.LoadScene("CH2map");
    }

    public void GoToCH3()
    {
        SceneManager.LoadScene("CH3map");
    }

    public void GoToCH4()
    {
        SceneManager.LoadScene("CH4map");
    }

    public void GoToGame()
    {
        SceneManager.LoadScene("GameSceneCH1-1");
    }

    public void QuitGame()
    {
        Debug.Log("Quitter le jeu");
        Application.Quit();
    }

    public void BackToWorldMap()
    {
        SceneManager.LoadScene("WorldMap");
    }
}