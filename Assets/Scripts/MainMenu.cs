using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject[] Windows;
    public GameObject MainMenuCanvas;
    public GameManager gameManager;

    public GameObject GameUI;
    public AudioSource MusicSource;

    public Slider MusicSlider;
    public Slider SFXSlider;

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
        GameUI.SetActive(true);
    }

    public void ActivateMainMenu()
    {
        gameManager.inputDisabled = true;
        MainMenuCanvas.SetActive(true);
        GameUI.SetActive(false);
    }

    public void ChangeMusicVolume()
    {
        MusicSource.volume = MusicSlider.value;
    }

    public void QuitGame()
    {

    }
}
