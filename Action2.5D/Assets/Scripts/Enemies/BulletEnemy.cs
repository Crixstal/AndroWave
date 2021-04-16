using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletEnemy : MonoBehaviour
{
    [HideInInspector] public float speed = 0f;
    [HideInInspector] public float damage = 0f;
    [HideInInspector] public Vector3 direction = Vector3.zero;

    private float calculSpeed = 0f;
    private Vector3 lastPosition = Vector3.zero;

    void FixedUpdate()
    {
        transform.Translate(direction * Time.deltaTime * 10f, Space.World);

        calculSpeed = (transform.position - lastPosition).magnitude * 100f;
        lastPosition = transform.position;
        if (calculSpeed == 0f)
            Destroy(gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 0 || other.gameObject.layer == 8) // 0 = default - 8 = player
            Destroy(gameObject);
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
