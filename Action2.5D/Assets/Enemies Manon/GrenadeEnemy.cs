using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeEnemy : MonoBehaviour
{
    [HideInInspector] public float speed = 0f;
    [HideInInspector] public float destructionDelay = 0f;
    [HideInInspector] public float damage = 0f;

    void FixedUpdate()
    {
        Destroy(gameObject, destructionDelay);
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
