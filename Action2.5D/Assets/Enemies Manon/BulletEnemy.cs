using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletEnemy : MonoBehaviour
{
    [HideInInspector] public float speed = 0f;
    [HideInInspector] public float destructionDelay = 0f;
    [HideInInspector] public int damage = 0;
    [HideInInspector] public Vector3 direction = Vector3.zero;

    void FixedUpdate()
    {
        transform.Translate(direction * Time.deltaTime * speed, Space.World);

        Destroy(gameObject, destructionDelay);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
