using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject creditsPanel;

    public AudioSource MusicSource;

    public Slider MusicSlider;
    public Slider SFXSlider;

    public CinemachineFreeLook CinemachineCamera;

  
    public void OpenCredits()
    {
        creditsPanel.SetActive(true);
    }

    public void CloseCredits()
    {
        creditsPanel.SetActive(false);
    }

    public void PlayButton()
    {
        SceneManager.LoadScene(1);
    }


    public void ChangeMusicVolume()
    {
        MusicSource.volume = MusicSlider.value;
    }

    public void QuitGame()
    {
        Application.Quit();
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
