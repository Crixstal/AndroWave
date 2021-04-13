using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    [SerializeField] private float life = 0f;

    public void FixedUpdate()
    {
        if (life <= 0)
            Destroy(gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 14 || other.CompareTag("Barrel")) // 14 = grenade
            life = 0f;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 9) // 9 = BulletPlayer
            life -= collision.gameObject.GetComponent<BulletPlayer>().damage;

        if (collision.gameObject.layer == 13) // 13 = yak
            life = 0f;
    }
}
