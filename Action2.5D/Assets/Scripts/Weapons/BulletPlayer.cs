using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPlayer : MonoBehaviour
{
    [HideInInspector] public float speed;
    [HideInInspector] public float damage;
    [HideInInspector] public float destructionDelay;
    [HideInInspector] public Vector3 direction;

    private ParticleSystem deathParticle = null;


    void Start()
    {
        deathParticle = GameObject.Find("Particles/MuzzleFlash").GetComponent<ParticleSystem>();
    }

    void FixedUpdate()
    {
        transform.Translate(direction * Time.deltaTime * speed, Space.World);
        
        Destroy(gameObject, destructionDelay);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 0 || collision.gameObject.layer == 11 || collision.gameObject.layer == 13) // 0 = default - 11 = enemy - 13 = yak
        {
            deathParticle.transform.position = transform.position;
            deathParticle.Play();
            Destroy(gameObject);
        }
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
