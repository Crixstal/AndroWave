using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TDbullet : MonoBehaviour
{
    [HideInInspector] public float speed;
    [HideInInspector] public float destructionDelay;
    [HideInInspector] public float damage;
    [HideInInspector] public Vector3 direction;

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
