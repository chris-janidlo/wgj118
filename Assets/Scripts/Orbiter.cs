using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using crass;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D), typeof(Health2D))]
public class Orbiter : MonoBehaviour
{
    [System.Serializable]
    public class Breakage
    {
        public List<Orbiter> PartsList;
        public Vector2Int PartsReleasedRange;
        public float VariabilityFromCircle;

        public void Explode (Vector2 center)
        {
            int amount = RandomExtra.Range(PartsReleasedRange);
            float angle = 0;

            for (int i = 0; i < amount; i++)
            {
                var effectiveAngle = angle + Random.Range(-VariabilityFromCircle, VariabilityFromCircle);
                var fragment = Instantiate(PartsList.PickRandom(), center, Quaternion.AngleAxis(effectiveAngle, Vector3.forward));
                var explosiveForce = fragment.transform.forward * RandomExtra.Range(fragment.BurstForceRange);
                fragment.Rigidbody.AddForce(explosiveForce);

                angle += 360f / amount;
            }
        }
    }

    public float TopSpeed, OrbitalAccel, Damage;
    public Breakage BreakageStats;
    public Vector2 BurstForceRange;

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
