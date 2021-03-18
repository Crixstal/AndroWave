using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPlayer : MonoBehaviour
{
    public float speed = 5f;
    [HideInInspector]
    public Vector3 direction;

    void Update()
    {
        transform.Translate(direction * Time.deltaTime * speed, Space.World);
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
