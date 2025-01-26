using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BubbleShoot : MonoBehaviour
{
    public GameObject bubblePrefab;    // The bubble prefab to shoot
    public Transform spawnPoint;      // Position where the bubble spawns
    public float shootSpeed = 20f;    // Speed of the shot bubble
    public Camera mainCamera;
    public GameObject placeholderGun;
    public GameObject rootObject;
    public Transform bubbleShoot;

    private GameObject currentBubble;

    public BubbleColorManager bubbleColorManager;

    public GameObject GunRotator;
    public LayerMask aimLayer;

    public GameManager gameManager;

    public bool isLoaded;

    public AudioSource audioSource;

    protected void OnEnable()
    {
        //mainCamera.GetComponent<CinemachineBrain>().enabled = false;
        //mainCamera.transform.position = bubbleShoot.position;
        gameManager.GameModeID = 0;
        SpawnNewBubble();
    }

    protected void OnDisable()
    {
        //mainCamera.GetComponent<CinemachineBrain>().enabled = true;
        gameManager.GameModeID = -1;
    }


    void Update()
    {
        if (gameManager.GameModeID != 0) return;

        if (Input.GetMouseButtonDown(0) && !gameManager.inputDisabled) // Left mouse click
        {
            ShootBubble();
        }
        if (!gameManager.inputDisabled)
        {
            AimGun();
        }

        if (Input.GetKeyDown(KeyCode.Q) && !gameManager.inputDisabled)
        {
            gameManager.SetCharacter(true);
            placeholderGun.SetActive(true);
            rootObject.SetActive(false);
        }

        if (currentBubble == null) return;
            currentBubble.transform.position = spawnPoint.position;
    }


    public void SpawnNewBubble()
    {

        if (currentBubble != null) return;
        if (gameManager.BubblesInStorage <= 0) return;

        bubblePrefab.GetComponent<BubbleCollisionHandler>().gameManager = gameManager;

        currentBubble = Instantiate(bubblePrefab, spawnPoint.position, Quaternion.identity);

        currentBubble.GetComponent<Renderer>().material.color = bubbleColorManager.AssignRandomColor();

        isLoaded = true;
    }

    void AimGun()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit_, Mathf.Infinity, aimLayer))
        {
            Vector3 aimDirection = (hit_.point - GunRotator.transform.position).normalized;
            GunRotator.transform.rotation = Quaternion.LookRotation(aimDirection);
        }
    }

    void ShootBubble()
    {
        if (currentBubble == null) return;


        // Raycast from the mouse position to determine shooting direction
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            StartCoroutine(currentBubble.GetComponent<BubbleCollisionHandler>().DestroyBubble());

            Vector3 direction = (hit.point - spawnPoint.position).normalized;

            gameManager.DecreaseBubbles(bubbleColorManager.currentColorID);

            isLoaded = false;
            PlayPopSound();
            // Add Rigidbody force to the bubble
            Rigidbody rb = currentBubble.GetComponent<Rigidbody>();

            rb.useGravity = true;

            rb.velocity = direction * shootSpeed;

            currentBubble = null;
            Invoke(nameof(SpawnNewBubble), 1f); // Spawn next bubble after a delay
        }
    }

    void PlayPopSound()
    {
        float rndPitch = Random.Range(0.8f, 1.2f);
        audioSource.pitch = rndPitch;
        audioSource.Play();
    }

}
