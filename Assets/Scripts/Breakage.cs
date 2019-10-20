using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using crass;

[RequireComponent(typeof(Collider2D), typeof(Health2D))]
public class Breakage : MonoBehaviour
{
    public List<Orbiter> PartsList;
    public Vector2Int PartsReleasedRange;
    public float ArcRange;

    Vector2 lastCollisionNormal;

    void Start ()
    {
        GetComponent<Health2D>().Died.AddListener(onDied);
    }

    void OnCollisionEnter2D (Collision2D other)
    {
        lastCollisionNormal = other.contacts[0].normal * other.relativeVelocity;
    }

    void explode ()
    {
        int amount = RandomExtra.Range(PartsReleasedRange);

        for (int i = 0; i < amount; i++)
        {
            var fragment = Object.Instantiate(PartsList.PickRandom(), transform.position, Quaternion.identity);
            var explosiveVelocity = Quaternion.AngleAxis(Random.Range(-ArcRange, ArcRange), Vector3.forward) * lastCollisionNormal;

            if (explosiveVelocity.magnitude > fragment.MaxBurstSpeed)
            {
                explosiveVelocity = explosiveVelocity.normalized * fragment.MaxBurstSpeed;
            }

            fragment.Rigidbody.velocity = explosiveVelocity;
        }
    }

    void onDied ()
    {
        StartCoroutine(deathRoutine());
    }

    IEnumerator deathRoutine ()
    {
        yield return null; // wait a frame to make sure we have the actual latest collision normal

        explode();
        Destroy(gameObject);
    }
}