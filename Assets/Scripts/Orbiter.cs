using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D), typeof(Health2D))]
public class Orbiter : MonoBehaviour
{
    public float TopSpeed, OrbitalAccel, Damage;
    public Breakage BreakageStats;
    public Vector2 BurstSpeedRange;

    Rigidbody2D _rb;
    public Rigidbody2D Rigidbody => _rb ?? (_rb = GetComponent<Rigidbody2D>());

    void Awake ()
    {
        GetComponent<Health2D>().Died.AddListener(() => {
            BreakageStats.Explode(transform.position);
            Destroy(gameObject);
        });
    }

    void Update ()
    {
        Vector2 orbitalDirection = (Ship.Instance.transform.position - transform.position).normalized;

        Rigidbody.velocity += orbitalDirection * OrbitalAccel * Time.deltaTime;

        if (Rigidbody.velocity.magnitude > TopSpeed)
        {
            Rigidbody.velocity = Rigidbody.velocity.normalized * TopSpeed;
        }
    }

    void OnCollisionEnter2D (Collision2D other)
    {
        var health = other.gameObject.GetComponent<Health2D>();
        if (health != null) health.CurrentValue -= Damage;
    }
}
