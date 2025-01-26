using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Camera mainCamera;

    public BubbleShoot bubbleShoot;
    public PlatformMiniGameManager PlatformMiniGame;
    public GameObject m_character;
    public GameObject m_playerCamera;

    public BubbleWall bubbleWall;

    public int maxBubbles = 10;
    public int YellowBubbles = 0;
    public int RedBubbles = 0;
    public int BlueBubbles = 0;
    public int GreenBubbles = 0;

    public int BubblesInStorage = 1;

    public float neighborRadius = 1.05f; // Adjust this to match bubble spacing

    public int GameModeID = -1;

    public int FailedShoots = 0;

    public InventoryManager inventoryManager;

    public MainMenu mainMenu;

    public bool inputDisabled;

    public List<Bubble> bubblesToPop = new List<Bubble>();

    public AudioSource WallBubblePopAudio;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void SetCharacter(bool activate)
    { 
        m_character.SetActive(activate);
        m_playerCamera.SetActive(activate);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            mainMenu.ActivateMainMenu();
        }
        if (Input.GetKeyDown(KeyCode.M) && !inputDisabled)
        {
            bubbleWall.ResetBubbleWall();
        }
       
        BubblesInStorage = RedBubbles + BlueBubbles + GreenBubbles + YellowBubbles;
        inventoryManager.CheckBallCountToInventory(RedBubbles, GreenBubbles, BlueBubbles, YellowBubbles);

        if (FailedShoots >= 5)
        {
            FailedShoots = 0;
            bubbleWall.ResetBubbleWall();
        }
    }

    public void DecreaseBubbles(int bubbleID)
    {
        if (bubbleID == 0 && RedBubbles > 0)
        {
            RedBubbles--;
        }
        else if (bubbleID == 1 && GreenBubbles > 0)
        {

            GreenBubbles--;
        }
        else if (bubbleID == 2 && BlueBubbles > 0)
        {

            BlueBubbles--;
        }
        else if (bubbleID == 3 && YellowBubbles > 0)
        {

            YellowBubbles--;
        }
    }
    public void IncreaseBubbles(int bubbleID)
    {
        if (BubblesInStorage <= maxBubbles)
        {
            if (bubbleID == 0)
            {
                RedBubbles++;
            }
            else if (bubbleID == 1)
            {
                GreenBubbles++;
            }
            else if (bubbleID == 2)
            {
                BlueBubbles++;
            }
            else if (bubbleID == 3)
            {
                YellowBubbles++;
            }

        }
    }

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
        bubblesToPop.Add(bubble);

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

    public void StartPopBubbles()
    {
        StartCoroutine(PopBubbles(bubblesToPop));
        bubblesToPop = new List<Bubble>();
    }

    public IEnumerator PopBubbles(List<Bubble> connectedBubbles)
    {
        foreach (Bubble bubble in connectedBubbles)
        {
            bubble.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.5f);
        }
    }
}