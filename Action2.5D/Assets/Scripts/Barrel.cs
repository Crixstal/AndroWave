using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrel : MonoBehaviour
{
    [SerializeField] internal float life = 0f;
    [SerializeField] internal float damage = 0f;
    [SerializeField] internal float destructionDelay = 0f;


    void FixedUpdate()
    {
        if (life <= 0)
            StartCoroutine(Explosion());
    }

    private IEnumerator Explosion()
    {
        gameObject.GetComponent<BoxCollider>().enabled = true;

        yield return new WaitForSeconds(destructionDelay);

        Destroy(gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 13 || other.gameObject.layer == 14 || other.CompareTag("Barrel")) // 13 = yak / 14 = grenade
            life = 0f;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 9) // 9 = BulletPlayer
            --life;
    }
}
