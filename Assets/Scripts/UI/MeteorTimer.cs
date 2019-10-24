using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MeteorTimer : MonoBehaviour
{
    public MeteorSpawner Spawner;
    public TextMeshProUGUI Display;

    void Update ()
    {
        var time = Mathf.CeilToInt(Spawner.TimeUntilNextSpawn);
        if (time < 1) time = 1;
        Display.text = time.ToString();
    }
}
