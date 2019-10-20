using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class EnemyLaser : MonoBehaviour
{
    public float Damage, Speed;

    public void ShootToward (Vector3 target)
    {
        var direction = (target - transform.position).normalized;
        transform.up = direction;
        GetComponent<Rigidbody2D>().velocity = direction * Speed;
    }

    void OnCollisionEnter2D (Collision2D other)
    {
        other.gameObject.GetComponent<Health2D>()?.Hurt(Damage);
        Destroy(gameObject);
    }
}
