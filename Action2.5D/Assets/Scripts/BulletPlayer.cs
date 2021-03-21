using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPlayer : MonoBehaviour
{
    [SerializeField]
    private float speed = 5f;
    [SerializeField]
    private float destructionDelay = 3f;
    [HideInInspector]
    public Vector3 direction;

    private float startTimer = 0f;

    void FixedUpdate()
    {
        transform.Translate(direction * Time.deltaTime * speed, Space.World);

        startTimer += Time.deltaTime;
        if (startTimer >= destructionDelay)
        {
            Destroy(gameObject);
            startTimer = 0f;
        }
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
