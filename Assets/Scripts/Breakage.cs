﻿
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using crass;

[System.Serializable]
public class Breakage
{
    public List<Orbiter> PartsList;
    public Vector2Int PartsReleasedRange;
    public float VariabilityFromCircle;

    public void Explode (Vector2 center)
    {
        int amount = RandomExtra.Range(PartsReleasedRange);
        float angle = 0;

        for (int i = 0; i < amount; i++)
        {
            var effectiveAngle = angle + Random.Range(-VariabilityFromCircle, VariabilityFromCircle);
            var fragment = Object.Instantiate(PartsList.PickRandom(), center, Quaternion.AngleAxis(effectiveAngle, Vector3.forward));
            var explosiveForce = fragment.transform.forward * RandomExtra.Range(fragment.BurstForceRange);
            fragment.Rigidbody.AddForce(explosiveForce);

            angle += 360f / amount;
        }
    }
}