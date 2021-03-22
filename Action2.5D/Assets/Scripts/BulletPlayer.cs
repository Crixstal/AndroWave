using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPlayer : MonoBehaviour
{
    [HideInInspector]   public float speed;
    [HideInInspector]   public float destructionDelay;
    [HideInInspector]   public Vector3 direction;

    void FixedUpdate()
    {
        transform.Translate(direction * Time.deltaTime * speed, Space.World);

        Destroy(gameObject, destructionDelay);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
