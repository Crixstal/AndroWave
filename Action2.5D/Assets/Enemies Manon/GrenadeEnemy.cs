using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeEnemy : MonoBehaviour
{
    [HideInInspector] public float speed;
    [HideInInspector] public float destructionDelay;
    [HideInInspector] public int damage;

    void FixedUpdate()
    {
        Destroy(gameObject, destructionDelay);
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
