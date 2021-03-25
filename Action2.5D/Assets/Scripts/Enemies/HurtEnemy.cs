using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtEnemy : MonoBehaviour
{
    [SerializeField]
    private float HP = 3;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 9) //9 = layer BulletPlayer
        {
            HP--;
            Destroy(collision.gameObject);

            if (HP <= 0)
                Destroy(gameObject);
        }
    }
}
