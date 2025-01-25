using UnityEngine;

public class SoapBubbleSpawner : MonoBehaviour
{
    [SerializeField] private GameObject bubblePrefab;  
    [SerializeField] private Transform spawnPosition;

    private float spawnInterval = 2f;

    protected void Start()
    {
        InvokeRepeating("SpawnBubble", 0f, spawnInterval);
    }

    private void SpawnBubble() => Instantiate(bubblePrefab, spawnPosition.position, Quaternion.identity);
}