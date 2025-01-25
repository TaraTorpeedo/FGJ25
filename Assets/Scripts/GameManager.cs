using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public int maxBubbles = 10;
    public int YellowBubbles = 0;
    public int RedBubbles = 0;
    public int BlueBubbles = 0;
    public int GreenBubbles = 0;

    public int BubblesInStorage = 0;

    public GameObject WallBubblePrefab;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (BubblesInStorage < maxBubbles)
        {

            if (Input.GetKeyUp(KeyCode.U))
            {
                YellowBubbles++;
            }
            if (Input.GetKeyUp(KeyCode.I))
            {
                RedBubbles++;
            }
            if (Input.GetKeyUp(KeyCode.O))
            {
                BlueBubbles++;
            }
            if (Input.GetKeyUp(KeyCode.P))
            {
                GreenBubbles++;
            }

            //Maybew some other way :D

            BubblesInStorage = RedBubbles + BlueBubbles + GreenBubbles + YellowBubbles;

        }
    }

    public float neighborRadius = 1.05f; // Adjust this to match bubble spacing

    public List<Bubble> FindConnectedBubbles(Bubble startBubble)
    {
        List<Bubble> connectedBubbles = new List<Bubble>();
        HashSet<Bubble> visited = new HashSet<Bubble>(); // Track visited bubbles

        // Start flood fill
        FloodFill(startBubble, connectedBubbles, visited);

        return connectedBubbles;
    }
    void FloodFill(Bubble bubble, List<Bubble> connectedBubbles, HashSet<Bubble> visited)
    {
        // Base case: If bubble is null or already visited, return
        if (bubble == null || visited.Contains(bubble)) return;

        // Mark this bubble as visited
        visited.Add(bubble);

        // Add this bubble to the connected list
        connectedBubbles.Add(bubble);

        // Find neighbors of the bubble
        Collider[] hits = Physics.OverlapSphere(bubble.transform.position, neighborRadius);

        foreach (Collider hit in hits)
        {
            Bubble neighbor = hit.GetComponent<Bubble>();

            // If the neighbor has the same color, recursively flood fill
            if (neighbor != null && neighbor.GetComponent<Renderer>().material.color == bubble.GetComponent<Renderer>().material.color)
            {
                FloodFill(neighbor, connectedBubbles, visited);
            }
        }
    }
}
