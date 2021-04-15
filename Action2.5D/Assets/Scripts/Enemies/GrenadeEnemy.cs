using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeEnemy : MonoBehaviour
{
    [HideInInspector] public float damage = 0f;
    [HideInInspector] public float blastDelay = 0f;
    [HideInInspector] public float destructionDelay = 0f;

    private ParticleSystem deathParticle = null;

    void Start()
    {
        deathParticle = GameObject.Find("Particles/CosmicReversal").GetComponent<ParticleSystem>();
    }

    private IEnumerator Explosion()
    {
        yield return new WaitForSeconds(blastDelay);

        gameObject.GetComponent<CapsuleCollider>().enabled = true;

        deathParticle.transform.position = transform.position;
        deathParticle.Play();

        yield return new WaitForSeconds(destructionDelay);

        Destroy(gameObject);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 8) // 8 = Player
            Destroy(gameObject);
        else
            StartCoroutine(Explosion());
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}