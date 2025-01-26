using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryTrigger : MonoBehaviour
{
    [SerializeField] private GameObject victoryUI;
    [SerializeField] private GameObject gameUI;
    [SerializeField] private GameObject player; 
    [SerializeField] private GameObject placeholderPlayer;
    [SerializeField] private GameObject playerCamera;
    [SerializeField] private ParticleSystem confettiParticle;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Voitto Perkele!");
            

            if (victoryUI != null)
            {
                victoryUI.SetActive(true);
            }

            if (gameUI != null)
            {
                gameUI.SetActive(false);
            }

            if (player != null && placeholderPlayer != null)
            {
                player.SetActive(false);
                playerCamera.SetActive(false);
                placeholderPlayer.SetActive(true);
            }
            
            if (confettiParticle != null)
            {
                confettiParticle.Play();
            }
        }
    }
}