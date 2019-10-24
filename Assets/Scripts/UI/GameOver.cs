using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameOver : MonoBehaviour
{
    public TextMeshProUGUI Display;

    void Update ()
    {
        Display.enabled = Ship.Instance.CurrentLives <= 0;
    }
}
