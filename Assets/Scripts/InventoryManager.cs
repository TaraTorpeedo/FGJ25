using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public TextMeshProUGUI RedBallCount_UI;
    public TextMeshProUGUI GreenBallCount_UI;
    public TextMeshProUGUI BlueBallCount_UI;
    public TextMeshProUGUI YellowBallCount_UI;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void CheckBallCountToInventory(int Reds, int Greens, int Blues, int Yellows)
    {
        RedBallCount_UI.text = Reds.ToString();
        GreenBallCount_UI.text = Greens.ToString();
        BlueBallCount_UI.text = Blues.ToString();
        YellowBallCount_UI.text = Yellows.ToString();
    }
}
