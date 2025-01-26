using Cinemachine;
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

    public CinemachineFreeLook CinemachineCamera;

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

    public void ChangeCameraYSpeed(Slider slider)
    {
        CinemachineCamera.m_YAxis.m_MaxSpeed *= slider.value;
    }
    public void ChangeCameraXSpeed(Slider slider)
    {
        CinemachineCamera.m_XAxis.m_MaxSpeed *= slider.value;
    }

    public void InvertXAxis(Toggle toggle)
    {
        CinemachineCamera.m_XAxis.m_InvertInput = toggle.isOn;
    }
    public void InvertYAxis(Toggle toggle)
    {
        CinemachineCamera.m_YAxis.m_InvertInput = toggle.isOn;
    }
}
