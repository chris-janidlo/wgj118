using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using crass;

[RequireComponent(typeof(Rigidbody2D), typeof(Health2D))]
public class Ship : Singleton<Ship>
{
    public int MaxLives, CurrentLives;
    public float DamageThreshold, InvulnTime, Thrust, ChangeDirectionThreshold;

    Rigidbody2D _rb;
    public Rigidbody2D Rigidbody => _rb ?? (_rb = GetComponent<Rigidbody2D>());

    Vector3 lastFramePosition;
    Vector2 facingDir;

    Health2D health;

    float invulnTimer;

    void Awake ()
    {
        SingletonSetInstance(this, true);
        
        health = GetComponent<Health2D>();
        health.HealthChanged.AddListener(onDamage);

        lastFramePosition = transform.position;
        facingDir = Vector2.up;

        CurrentLives = MaxLives;
    }

    void Update ()
    {
        var mouse = Input.mousePosition;
        mouse.z = -CameraCache.Main.transform.position.z;
        var realPos = CameraCache.Main.ScreenToWorldPoint(mouse);

        Rigidbody.velocity = (realPos - transform.position) * Thrust;

        if (Vector2.Distance(transform.position, lastFramePosition) >= ChangeDirectionThreshold)
        {
            facingDir = (transform.position - lastFramePosition).normalized;
            lastFramePosition = transform.position;
        }

        transform.up = Vector2.Lerp(transform.up, facingDir, 15 * Time.deltaTime);

        invulnTimer -= Time.deltaTime;
        if (invulnTimer <= 0) health.Invuln = false;
    }

    void onDamage (float amount)
    {
        if (Mathf.Abs(amount) >= DamageThreshold)
        {
            health.Invuln = true;
            invulnTimer = InvulnTime;

            CurrentLives += (int) Mathf.Sign(amount);
            if (CurrentLives <= 0) Debug.Log("gameover");
        }
    }
}