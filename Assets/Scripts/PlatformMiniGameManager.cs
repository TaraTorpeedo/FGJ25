using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMiniGameManager : MonoBehaviour
{
    public GameManager gameManager;
    public Transform cameraTransform;

    public GameObject[] Platforms;

    public float rotateSpeed = 1.0f;

    public GameObject bubblePrefab;
    public Transform spawnPoint;
    private GameObject currentBubble;

    public BubbleColorManager bubbleColorManager;


    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(DropNewBubble());
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.GameModeID != 1) return;

        if(Input.GetKey(KeyCode.A)) { 
            foreach(GameObject platform in Platforms)
            {
                if (platform.transform.rotation.z < 0.16f)
                {
                    platform.transform.Rotate(0, 0, rotateSpeed * Time.deltaTime);
                }
            }
        }
        if (Input.GetKey(KeyCode.D))
        {
            foreach (GameObject platform in Platforms)
            {
                if (platform.transform.rotation.z > -0.16f)
                {
                    platform.transform.Rotate(0, 0, -rotateSpeed * Time.deltaTime);
                }
            }
        }

    }

    public IEnumerator DropNewBubble()
    {
        if (gameManager.GameModeID == 1)
        {
            currentBubble = Instantiate(bubblePrefab, spawnPoint.position, Quaternion.identity);
            currentBubble.GetComponent<Rigidbody>().useGravity = true;

            currentBubble.GetComponent<Renderer>().material.color = bubbleColorManager.AssignRandomColorToWall();
            yield return new WaitForSeconds(4);

            StartCoroutine(DropNewBubble());
        }
        else
        {
            yield return new WaitForSeconds(4);
            StartCoroutine(DropNewBubble());
        }
    }
}
