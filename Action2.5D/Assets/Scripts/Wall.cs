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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 9) // 9 = BulletPlayer
            life -= collision.gameObject.GetComponent<BulletPlayer>().damage;

        //if (collision.gameObject.layer == 13) // 13 = Yak         SANGLIER
        //    Destroy(gameObject);
    }
}
