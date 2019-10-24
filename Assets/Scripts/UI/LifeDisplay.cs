using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeDisplay : MonoBehaviour
{
    public List<Image> Lives;

    void Update ()
    {
        var lives = Ship.Instance.CurrentLives;

        for (int i = 0; i < Lives.Count; i++)
        {
            Lives[i].enabled = lives >= i + 1;
        }
    }
}
