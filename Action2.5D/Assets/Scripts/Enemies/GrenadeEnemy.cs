using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeEnemy : MonoBehaviour
{
    public float blastDelay = 0f;
    public float destructionDelay = 0f;
    public float damage = 0f;

    private IEnumerator Explosion()
    {
        yield return new WaitForSecondsRealtime(blastDelay);

        gameObject.GetComponent<CapsuleCollider>().enabled = true;

        yield return new WaitForSecondsRealtime(destructionDelay);

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
