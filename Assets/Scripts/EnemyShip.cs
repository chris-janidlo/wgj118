using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using crass;

[RequireComponent(typeof(Rigidbody2D), typeof(Breakage))]
public class EnemyShip : MonoBehaviour
{
    public enum MovementType { Cardinal, Diagonal, Follow }

    public MovementType Movement;
    public float Speed, ShootRange, TimePerShot;
    public EnemyLaser LaserPrefab;

    float shotTimer;

    Rigidbody2D _rb;
    public Rigidbody2D Rigidbody => _rb ?? (_rb = GetComponent<Rigidbody2D>());

    void Start ()
    {
        Vector2 velocity;

        switch (Movement)
        {
            case MovementType.Cardinal:
                bool x = RandomExtra.Chance(.5f), p = RandomExtra.Chance(.5f);
                velocity = new Vector2(x ? p ? 1 : -1 : 0, !x ? p ? 1 : -1 : 0) * Speed;
                break;
            
            case MovementType.Diagonal:
                List<Vector2> directions = new List<Vector2>
                {
                    new Vector2(1, 1),
                    new Vector2(1, -1),
                    new Vector2(-1, 1),
                    new Vector2(-1, -1)
                };
                velocity = directions.PickRandom() * Speed;
                break;

            case MovementType.Follow:
                // do nothing
                break;

            default:
                throw new System.InvalidOperationException($"unexpected MovementType {Movement}");
        }
    }

    void Update ()
    {
        if (Movement == MovementType.Follow)
        {
            var diff = Ship.Instance.transform.position - transform.position;

            if (diff.magnitude > Speed * Time.deltaTime)
            {
                Rigidbody.velocity = diff.normalized * Speed;
            }
            else
            {
                Rigidbody.velocity = diff;
            }
        }

        transform.up = -Rigidbody.velocity;

        shotTimer -= Time.deltaTime;

        Vector2 playerPos = Ship.Instance.transform.position;

        if (Vector2.Distance(transform.position, playerPos) <= ShootRange && shotTimer <= 0)
        {
            shotTimer = TimePerShot;
            Instantiate(LaserPrefab, transform.position, Quaternion.identity).ShootToward(playerPos);
        }
    }

    void OnCollisionEnter2D (Collision2D other)
    {
        if (Movement == MovementType.Follow) return;

        if (other.contacts[0].normal.x != 0)
        {
            Rigidbody.velocity = new Vector2(-Rigidbody.velocity.x, Rigidbody.velocity.y);
        }
        else
        {
            Rigidbody.velocity = new Vector2(Rigidbody.velocity.x, -Rigidbody.velocity.y);            
        }
    }
}
