using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleShoot : MonoBehaviour
{
    public GameObject bubblePrefab;    // The bubble prefab to shoot
    public Transform spawnPoint;      // Position where the bubble spawns
    public float shootSpeed = 20f;    // Speed of the shot bubble
    public Camera mainCamera;         // Reference to the main camera

    private GameObject currentBubble;

    public BubbleColorManager bubbleColorManager;

    public GameObject GunRotator;
    public LayerMask aimLayer;

    void Start()
    {
        SpawnNewBubble();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Left mouse click
        {
            ShootBubble();
        }

        AimGun();


        if (currentBubble == null) return;
            currentBubble.transform.position = spawnPoint.position;
    }

    void SpawnNewBubble()
    {
        if (currentBubble != null) return;

        currentBubble = Instantiate(bubblePrefab, spawnPoint.position, Quaternion.identity);

        currentBubble.GetComponent<Renderer>().material.color = bubbleColorManager.AssignRandomColor();
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

            Vector3 direction = (hit.point - spawnPoint.position).normalized;

            // Add Rigidbody force to the bubble
            Rigidbody rb = currentBubble.GetComponent<Rigidbody>();

            rb.useGravity = true;

            rb.velocity = direction * shootSpeed;

            currentBubble = null;
            Invoke(nameof(SpawnNewBubble), 1f); // Spawn next bubble after a delay
        }
    }
}
