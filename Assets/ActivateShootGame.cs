using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateShootGame : MonoBehaviour
{
    [SerializeField] private GameObject m_pressButtonInfo;
    [SerializeField] private GameObject m_miniGame;
    [SerializeField] private GameObject m_rootObject;
    [SerializeField] private GameManager m_manager;

    protected void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Handle the collision with the character
            Debug.Log("Character entered the trigger of the BoxCollider!");
        }
    }

    protected void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                m_miniGame.SetActive(true);
                m_manager.SetCharacter(false);
                m_rootObject.SetActive(false);
            }
        }
    }

    protected void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Handle the collision with the character
            Debug.Log("Character exit the trigger of the BoxCollider!");
        }
    }
}
