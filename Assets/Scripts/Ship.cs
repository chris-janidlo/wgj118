using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{
    public PlayerBullet BulletPrefab;
    public float BulletsPerSecond, MaxFireRange;

    float timePerBullet => 1.0f / BulletsPerSecond;

    float fireTimer;

    void Update ()
    {
        var target = acquireTarget();
        
        if (target != null)
        {
            Vector2 dir = (target.transform.position - transform.position).normalized;
            transform.GetChild(0).up = dir;

            if (fireTimer <= 0)
            {
                fireTimer = timePerBullet;
                Instantiate(BulletPrefab, transform.position, Quaternion.identity).Initialize(dir);
            }
        }

        fireTimer -= Time.deltaTime;
    }

    EnemyHealth acquireTarget () => FindObjectsOfType<EnemyHealth>()
        .Select(eh => new { eh, distance = Vector2.Distance(transform.position, eh.transform.position) })
        .Where(e => e.distance <= MaxFireRange)
        .OrderBy(e => e.distance)
        .FirstOrDefault()
        ?.eh;
}
