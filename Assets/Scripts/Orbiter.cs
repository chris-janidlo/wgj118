using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D), typeof(Health2D))]
public class Orbiter : MonoBehaviour, IDamager
{
    public const float SPAWN_INVULN_TIME = .5f;

    [System.Serializable]
    public class OrbitalMechanics
    {
        public float CloseForce, FarDistance, FarForce;

        public float GetForce (float distance)
        {
            return Mathf.Lerp(CloseForce, FarForce, distance / FarDistance);
        }
    }

    [System.Serializable]
    public class DamageMechanics
    {
        public float MaxDamage, MaxSpeed, MinDamage, MinSpeed;

        public float GetDamage (float speed)
        {
            float lerpAmt = (speed - MinSpeed) / (MaxSpeed - MinSpeed);
            return Mathf.Lerp(MinDamage, MaxDamage, lerpAmt);
        }
    }

    public bool IsSpaceDust;
    public DamageMechanics DamageForOthers, DamageForSelf;
    public OrbitalMechanics OrbitalStats;
    public Breakage BreakageStats;
    public float MaxBurstSpeed;
    public float TrueMaxSpeed; // should be experimentally derived based on orbit alone
    public bool TestForTrueMax;

    Rigidbody2D _rb;
    public Rigidbody2D Rigidbody => _rb ?? (_rb = GetComponent<Rigidbody2D>());

    Health2D health;

    Vector2 lastCollisionNormal;

    void Awake ()
    {
        health = GetComponent<Health2D>();

        health.Died.AddListener(onDied);

        health.Invuln = true;
    }

    IEnumerator Start ()
    {
        yield return new WaitForSeconds(SPAWN_INVULN_TIME);
        health.Invuln = false;
    }

    void FixedUpdate ()
    {
        Vector2 diffVector = Ship.Instance.OrbitalCenter.position - transform.position;
        Vector2 orbitalForce = OrbitalStats.GetForce(diffVector.magnitude) * diffVector.normalized * Time.deltaTime;
        Rigidbody.AddForce(orbitalForce, ForceMode2D.Force);

        if (Rigidbody.velocity.magnitude > TrueMaxSpeed)
        {
            if (TestForTrueMax)
            {
                TrueMaxSpeed = Rigidbody.velocity.magnitude;
                Debug.Log(TrueMaxSpeed);
            }
            else
            {
                Rigidbody.velocity = Rigidbody.velocity.normalized * TrueMaxSpeed;
            }
        }

        transform.up = Vector2.MoveTowards(transform.up, Rigidbody.velocity, Time.deltaTime);
    }

    void OnCollisionEnter2D (Collision2D other)
    {
        lastCollisionNormal = other.contacts[0].normal * other.relativeVelocity;

        var collisionSpeed = other.relativeVelocity.magnitude;
        var otherHealth = other.gameObject.GetComponent<Health2D>();
        var otherPlayer = other.gameObject.GetComponent<Ship>();

        if (otherHealth != null)
        {
            otherHealth.CurrentValue -= DamageForOthers.GetDamage(collisionSpeed);
        }

        if (other.gameObject.GetComponent<IDamager>() == null)
        {
            health.CurrentValue -= DamageForSelf.GetDamage(collisionSpeed);
        }

        if (otherPlayer != null && IsSpaceDust)
        {
            otherPlayer.SpaceDustCollected++;
        }
    }

    void onDied ()
    {
        StartCoroutine(deathRoutine());
    }

    IEnumerator deathRoutine ()
    {
        yield return null; // wait a frame to make sure we have the actual latest collision normal

        BreakageStats.Explode(transform.position, lastCollisionNormal);
        Destroy(gameObject);
    }
}
