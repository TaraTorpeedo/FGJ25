using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BubbleColorManager : MonoBehaviour
{
    public List<Color> bubbleColors;
    List <Color> emptyColors = new List<Color>();
    public int currentColorID;

    public GameManager gameManager;

    void Start()
    {
        //AssignRandomColor();
    }

    public Color AssignRandomColor()
    {
        //Red
        if (gameManager.RedBubbles <= 0)
        {
            emptyColors.Add(bubbleColors[0]);
        }
        else
            emptyColors.Remove(bubbleColors[0]);
        //Green
        if (gameManager.GreenBubbles <= 0)
        {
            emptyColors.Add(bubbleColors[1]);
        }
        else
            emptyColors.Remove(bubbleColors[1]);
        //Blue
        if (gameManager.BlueBubbles <= 0)
        {
            emptyColors.Add(bubbleColors[2]);
        }
        else
            emptyColors.Remove(bubbleColors[2]);
        //Yellow
        if (gameManager.YellowBubbles <= 0)
        {
            emptyColors.Add(bubbleColors[3]);
        }
        else
            emptyColors.Remove(bubbleColors[3]);

        Color RandomColor = GetRandomColor(bubbleColors, emptyColors);

        //Onks tähä muuta ratkasuu? xDD
        //Red
        if (RandomColor == new Color(1, 0, 0, 0))
        {
            currentColorID = 0;
        }
        //Green
        else if (RandomColor == new Color(0, 1, 0, 0))
        {
            currentColorID = 1;
        }
        //Blue
        else if (RandomColor == new Color(0, 0, 1, 0))
        {
            currentColorID = 2;
        }
        //Yellow
        else if (RandomColor == new Color(1, 1, 0, 0))
        {
            currentColorID = 3;
        }
        else
        {
            Debug.Log("Unknown color");
        }

        return RandomColor;
    }

    Color GetRandomColor(List<Color> colorArray, List<Color> emptyColors)
    {
        List<Color> filteredArray = colorArray.Where(color => !emptyColors.Contains(color)).ToList();

        if(filteredArray.Count == 0) {
            return Color.clear;
        }

        return filteredArray[Random.Range(0, filteredArray.Count)];
    }

    public Color AssignRandomColorToWall()
    {
        int rnd = Random.Range(0, bubbleColors.Count);

        return bubbleColors[rnd];
    }

}
