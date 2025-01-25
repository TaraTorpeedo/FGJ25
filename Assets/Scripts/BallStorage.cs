using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallStorage : MonoBehaviour
{
    public bool isStorageUnit;

    int BubbleColorID = 0;

    public GameManager gameManager;

    private void OnCollisionEnter(Collision collision)
    {
        if (isStorageUnit)
        {
            Debug.Log(collision.gameObject.GetComponent<Renderer>().material.color);

            if(collision.gameObject.GetComponent<Renderer>().material.color == new Color(1, 0, 0, 0))
            {
                BubbleColorID = 0;
            }

            else if (collision.gameObject.GetComponent<Renderer>().material.color == new Color(0, 1, 0, 0))
            {
                BubbleColorID = 1;
            }

            else if (collision.gameObject.GetComponent<Renderer>().material.color == new Color(0, 0, 1, 0))
            {
                BubbleColorID = 2;
            }

            else if (collision.gameObject.GetComponent<Renderer>().material.color == new Color(1, 1, 0, 0))
            {
                BubbleColorID = 3;
            }

            gameManager.IncreaseBubbles(BubbleColorID);


            Destroy(collision.gameObject);
        }
        else
        {
            Destroy(collision.gameObject);
        }
    }
}