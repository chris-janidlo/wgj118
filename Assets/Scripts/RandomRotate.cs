using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using crass;

public class RandomRotate : MonoBehaviour
{
    private static BagRandomizer<float> rotations = new BagRandomizer<float>
    {
        Items = new List<float> { 0, 90, 180, 270 },
        AvoidRepeats = true
    };

    void Start ()
    {
        transform.rotation = Quaternion.Euler(0, 0, rotations.GetNext());
    }
}
