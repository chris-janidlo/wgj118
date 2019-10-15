using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using crass;

[System.Serializable]
public class Breakage
{
    public List<Orbiter> PartsList;
    public Vector2Int PartsReleasedRange;
    public float ArcRange;

    public void Explode (Vector2 center, Vector2 collisionVelocity)
    {
        int amount = RandomExtra.Range(PartsReleasedRange);

        for (int i = 0; i < amount; i++)
        {
            var fragment = Object.Instantiate(PartsList.PickRandom(), center, Quaternion.identity);
            var explosiveVelocity = Quaternion.AngleAxis(Random.Range(-ArcRange, ArcRange), Vector3.forward) * collisionVelocity;

            fragment.Rigidbody.velocity = explosiveVelocity;
        }
    }
}