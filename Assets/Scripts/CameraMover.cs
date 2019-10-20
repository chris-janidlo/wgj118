using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using crass;

public class CameraMover : MonoBehaviour
{
    public float Speed;
    public Vector2 MinPosition, MaxPosition;

    void Update ()
    {
        Vector3 movement = new Vector2
        (
            Input.GetAxis("Horizontal"),
            Input.GetAxis("Vertical")
        );

        transform.position += movement * Speed * Time.deltaTime;

        transform.position = new Vector3
        (
            Mathf.Clamp(transform.position.x, MinPosition.x, MaxPosition.x),
            Mathf.Clamp(transform.position.y, MinPosition.y, MaxPosition.y),
            transform.position.z
        );
    }
}
