using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frelon : Enemy
{
    [SerializeField] private float coneAngle = 0f;


    public override void Shoot()
    {
        float constConeAngle = coneAngle;

        if (Time.time > shotTimer)
        {
            shotTimer = Time.time + delayPerShot;

            for (int i = 0; i < 3; ++i)
            {
                currentBullet = Instantiate(bullet, bulletSpawn, Quaternion.Euler(0f, 0f, coneAngle - 90f));
                currentBullet.GetComponent<BulletEnemy>().direction = Quaternion.AngleAxis(coneAngle, Vector3.forward) * Vector3.down;
                coneAngle -= constConeAngle;
            }
        }

        coneAngle = constConeAngle;
    }

    public override void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == 8) // 8 = Player
            Shoot();
    }
}