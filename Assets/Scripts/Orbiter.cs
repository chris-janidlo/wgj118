using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D), typeof(Health2D))]
public class Orbiter : MonoBehaviour
{
    [System.Serializable]
    public class OrbitalMechanics
    {
        public float CloseForce, FarDistance, FarForce;

        public float GetForce (float distance)
        {
            return Mathf.Lerp(CloseForce, FarForce, distance / FarDistance);
        }
    }

    public float Damage;
    public OrbitalMechanics OrbitalStats;
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

    void FixedUpdate ()
    {
        Vector2 diffVector = Ship.Instance.transform.position - transform.position;
        Vector2 orbitalForce = OrbitalStats.GetForce(diffVector.magnitude) * diffVector.normalized * Time.deltaTime;
        Rigidbody.AddForce(orbitalForce, ForceMode2D.Force);
    }

    void OnCollisionEnter2D (Collision2D other)
    {
        var health = other.gameObject.GetComponent<Health2D>();
        if (health != null) health.CurrentValue -= Damage;
    }
}
