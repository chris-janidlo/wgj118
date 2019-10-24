using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreCounter : MonoBehaviour
{
    public TextMeshProUGUI Display;

    void Update ()
    {
        Display.text = Ship.Instance.SpaceDustCollected.ToString();
    }
}
