using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleWall : MonoBehaviour
{
    public GameObject bubblePrefab; // Assign the bubble prefab in the Inspector
    public int rows = 10;           // Number of rows in the bubble wall
    public int columns = 10;        // Number of columns in the bubble wall
    public float bubbleSpacing = 1.1f; // Spacing between bubbles

    public float randomness = 0.1f; // Random offset for bubble positions

    public BubbleColorManager bubbleColorManager;

    List<GameObject> bubbles = new List<GameObject>();

    int bubblesCount;
    int bubblesCountNew;


    void Start()
    {
        GenerateBubbleWall();

        bubblesCount = bubbles.Count;
    }

    void GenerateBubbleWall()
    {
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                // Calculate base position
                Vector3 position = new Vector3(
                    transform.position.x + col * bubbleSpacing,
                    transform.position.y - row * bubbleSpacing,
                    transform.position.z
                );

                // Add randomness to the position
                position.x += Random.Range(-randomness, randomness);
                position.y += Random.Range(-randomness, randomness);
                position.z += Random.Range(-randomness, randomness);


                // Instantiate the bubble
                GameObject bubble = Instantiate(bubblePrefab, position, Quaternion.identity, transform);

                bubbles.Add(bubble);

                // Assign a random color
                AssignRandomColor(bubble);
            }
        }
    }

    void AssignRandomColor(GameObject bubble)
    {
        // Get the renderer component
        Renderer renderer = bubble.GetComponent<Renderer>();
        if (renderer != null)
        {
            // Assign a random color from the array
            renderer.material.color = bubbleColorManager.AssignRandomColorToWall();
        }
    }


    private void OnDrawGizmos()
    {
        Vector3 Size = new Vector3(rows, columns, 1);
        Vector3 Pos = new Vector3(transform.position.x + rows/2, transform.position.y - rows / 2, transform.position.z);
        Quaternion Rot = new Quaternion(transform.rotation.x, transform.rotation.y, transform.rotation.z, 0);

        Gizmos.color = new Color(0,0.8f,1f,.2f);
        Gizmos.DrawCube(Pos, Size);
    }

    public void ResetBubbleWall()
    {
        foreach (var bubble in bubbles)
        {
            Destroy(bubble);
        }

        GenerateBubbleWall();
    }

}
