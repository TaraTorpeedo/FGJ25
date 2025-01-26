using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnMainMenu : MonoBehaviour
{
    private void OnEnable()
    {
        StartCoroutine(ReturnDelay());
    }

    IEnumerator ReturnDelay()
    {
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene(0);
    }
}
