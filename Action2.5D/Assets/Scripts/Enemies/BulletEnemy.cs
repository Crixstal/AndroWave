using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletEnemy : MonoBehaviour
{
    [HideInInspector] public float speed = 0f;
    [HideInInspector] public float destructionDelay = 0f;
    [HideInInspector] public float damage = 0f;
    [HideInInspector] public Vector3 direction = Vector3.zero;

    void FixedUpdate()
    {
        transform.Translate(direction * Time.deltaTime * speed, Space.World);

        Destroy(gameObject, destructionDelay);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 0 || other.gameObject.layer == 8) // 0 = default - 8 = player
            Destroy(gameObject);
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
