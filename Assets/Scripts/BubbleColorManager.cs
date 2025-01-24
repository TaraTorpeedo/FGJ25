using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleColorManager : MonoBehaviour
{
    public Color[] bubbleColors;

    void Start()
    {
        AssignRandomColor();
    }

    public Color AssignRandomColor()
    {
        int rnd = Random.Range(0, bubbleColors.Length);

        return bubbleColors[rnd];
    }
}
