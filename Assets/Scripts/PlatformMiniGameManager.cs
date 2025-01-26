using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMiniGameManager : MonoBehaviour
{
    public GameManager gameManager;

    public GameObject[] Platforms;

    public float rotateSpeed = 1.0f;

    public GameObject bubblePrefab;
    public Transform spawnPoint;
    private GameObject currentBubble;

    public BubbleColorManager bubbleColorManager;
    public GameObject infoText;

    private float timer = 4f;

    private float delay = 2f;


    // Start is called before the first frame update
    protected void Start()
    {
        //StartCoroutine(DropNewBubble());
    }

    protected void OnEnable()
    {
        gameManager.GameModeID = 1;
        infoText.SetActive(true);
    }

    protected void OnDisable()
    {
        gameManager.GameModeID = -1;
        infoText.SetActive(false);
    }

    // Update is called once per frame
    protected void Update()
    {
        if (Input.GetKey(KeyCode.A) && !gameManager.inputDisabled)
        {
            foreach (GameObject platform in Platforms)
            {
                if (platform.transform.rotation.z < 0.16f)
                {
                    platform.transform.Rotate(0, 0, rotateSpeed * Time.deltaTime);
                }
            }
        }
        if (Input.GetKey(KeyCode.D) && !gameManager.inputDisabled)
        {
            foreach (GameObject platform in Platforms)
            {
                if (platform.transform.rotation.z > -0.16f)
                {
                    platform.transform.Rotate(0, 0, -rotateSpeed * Time.deltaTime);
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Q) && !gameManager.inputDisabled)
        {
            gameManager.SetCharacter(true);
            gameObject.SetActive(false);
        }

        timer += Time.deltaTime;
        if (timer >= 4f)
        {
            currentBubble = Instantiate(bubblePrefab, spawnPoint.position, Quaternion.identity);
            currentBubble.GetComponent<Rigidbody>().useGravity = true;

            currentBubble.GetComponent<Renderer>().material.color = bubbleColorManager.AssignRandomColorToWall();
            timer = 0f;
        }
    }
}
