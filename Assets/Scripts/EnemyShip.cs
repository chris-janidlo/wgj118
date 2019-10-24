using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using crass;

[RequireComponent(typeof(Rigidbody2D), typeof(Breakage))]
public class EnemyShip : MonoBehaviour
{
    public enum MovementType { Cardinal, Diagonal, Follow }

    public MovementType Movement;
    public float Speed, Acceleration, ShootRange, TimePerShot;
    [Range(0, 1)]
    public float ShootSlowPercent;
    public EnemyLaser LaserPrefab;
    public SpriteRenderer Visuals;

    float shotTimer;

    Rigidbody2D _rb;
    public Rigidbody2D Rigidbody => _rb ?? (_rb = GetComponent<Rigidbody2D>());

    Vector2 desiredVelocity;

    void Start ()
    {
        switch (Movement)
        {
            case MovementType.Cardinal:
                bool x = RandomExtra.Chance(.5f), p = RandomExtra.Chance(.5f);
                desiredVelocity = new Vector2(x ? p ? 1 : -1 : 0, !x ? p ? 1 : -1 : 0) * Speed;
                break;
            
            case MovementType.Diagonal:
                List<Vector2> directions = new List<Vector2>
                {
                    new Vector2(1, 1),
                    new Vector2(1, -1),
                    new Vector2(-1, 1),
                    new Vector2(-1, -1)
                };
                desiredVelocity = directions.PickRandom() * Speed;
                break;

            case MovementType.Follow:
                // do nothing
                break;

            default:
                throw new System.InvalidOperationException($"unexpected MovementType {Movement}");
        }

        Rigidbody.velocity = desiredVelocity;
    }

    void Update ()
    {
        shotTimer -= Time.deltaTime;

        Vector2 playerPos = Ship.Instance.transform.position;

        bool inRange = Visuals.isVisible && Vector2.Distance(transform.position, playerPos) <= ShootRange;
        
        if (inRange && shotTimer <= 0)
        {
            shotTimer = TimePerShot;
            Instantiate(LaserPrefab, transform.position, Quaternion.identity).ShootToward(playerPos);
        }

        if (Movement == MovementType.Follow)
        {
            var diff = Ship.Instance.transform.position - transform.position;

            if (diff.magnitude > Speed * Time.deltaTime)
            {
                desiredVelocity = diff.normalized * Speed;
            }
            else
            {
                desiredVelocity = diff;
            }
        }

        Rigidbody.velocity += (desiredVelocity * (inRange ? ShootSlowPercent : 1) - Rigidbody.velocity) * Acceleration * Time.deltaTime;

        Transform visual = Visuals.transform;
        Vector2 lookDir = inRange ? ((Vector2) transform.position - playerPos) : -Rigidbody.velocity;
        visual.up = Vector2.Lerp(visual.up, lookDir, 15 * Time.deltaTime);
    }

    void OnCollisionEnter2D (Collision2D other)
    {
        if (Movement == MovementType.Follow || other.gameObject.tag != "Wall") return;

        // use round because normals against tilemap collider sometimes show up as really tiny but nonzero values
        if (Mathf.Round(other.GetContact(0).normal.x) == 0)
        {
            desiredVelocity = new Vector2(desiredVelocity.x, -desiredVelocity.y);
        }
        else
        {
            desiredVelocity = new Vector2(-desiredVelocity.x, desiredVelocity.y);
        }
    }
}
