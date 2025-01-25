using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public GameObject[] Windows;
    public GameObject MainMenuCanvas;
    public GameManager gameManager;

    public void ActivateWindow(GameObject Window)
    {
        foreach (GameObject window in Windows)
        {
            window.SetActive(false);
        }
        Window.SetActive(true);
    }
    public void PlayButton(GameObject Window)
    {
        gameManager.inputDisabled = false;
        Window.SetActive(false);
    }

    public void ActivateMainMenu()
    {
        gameManager.inputDisabled = true;
        MainMenuCanvas.SetActive(true);
    }

    public void QuitGame()
    {

    }
}
