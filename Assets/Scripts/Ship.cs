using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using crass;

[RequireComponent(typeof(Collider2D), typeof(Health2D))]
public class Ship : Singleton<Ship>
{
    public float ContactDamage, ChangeDirectionThreshold;

    Vector3 lastFramePosition;
    Vector2 facingDir;

    void Awake ()
    {
        SingletonSetInstance(this, true);
        GetComponent<Health2D>().Died.AddListener(() => Debug.Log("gameover"));

        lastFramePosition = transform.position;
        facingDir = Vector2.up;
    }

    void Update ()
    {
        var mouse = Input.mousePosition;
        mouse.z = -CameraCache.Main.transform.position.z;
        transform.position = CameraCache.Main.ScreenToWorldPoint(mouse);

        if (Vector2.Distance(transform.position, lastFramePosition) >= ChangeDirectionThreshold)
        {
            facingDir = (transform.position - lastFramePosition).normalized;
            lastFramePosition = transform.position;
        }

        transform.up = Vector2.Lerp(transform.up, facingDir, 15 * Time.deltaTime);
    }
}