using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class PlayerBullet : MonoBehaviour
{
    public float Speed, Damage;

    public void Initialize (Vector2 direction)
    {
        transform.up = direction;
        GetComponent<Rigidbody2D>().velocity = direction * Speed;
    }

    void OnCollisionEnter2D (Collision2D other)
    {
        try
        {
            other.gameObject.GetComponent<EnemyHealth>().Damage(Damage);
        }
        finally
        {
            Destroy(gameObject);
        }
    }

    void OnBecameInvisible ()
    {
        Destroy(gameObject);
    }
}
