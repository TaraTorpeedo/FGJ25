using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleCollisionHandler : Bubble
{
    public int minMatchCount = 3;
    public GameManager gameManager;

    bool isSnapped = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "WallBubble")
        {
            SnapToWall(collision.gameObject);

            if (collision.gameObject.GetComponent<Renderer>().material.color == GetComponent<Renderer>().material.color)
            {
                Bubble shotBubble = collision.gameObject.GetComponent<Bubble>();
                if (shotBubble != null)
                {
                    List<Bubble> connectedBubbles = gameManager.FindConnectedBubbles(shotBubble);

                    if (connectedBubbles.Count >= 3)
                    {
                        foreach (Bubble bubble in connectedBubbles)
                        {
                            bubble.gameObject.SetActive(false);
                        }
                    }

                }
            }
        }
    }

    void SnapToWall(GameObject WallBubble)
    {
        isSnapped = true;


        Destroy(GetComponent<Rigidbody>());
        transform.SetParent(WallBubble.transform);
        gameObject.tag = WallBubble.gameObject.tag;

    }

    public IEnumerator DestroyBubble()
    {
        yield return new WaitForSeconds(5);
        if(!isSnapped)
        {
            Destroy(gameObject);

        }
    }
}
