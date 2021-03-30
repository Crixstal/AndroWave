using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gorille : Enemy
{
    public override void Shoot()
    {
        if (Time.time > shotTimer)
        {
            shotTimer = Time.time + delayPerShot;

            //for (int i = 0; i < bulletPerSalve; ++i)
            //{
            GameObject currentBullet = Instantiate(bullet, transform.position, Quaternion.identity);
            currentBullet.GetComponent<BulletEnemy>().direction = Vector3.Normalize(relativePos);
            //}
        }
    }
}
