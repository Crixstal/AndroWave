using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : WeaponPlayer
{
    [SerializeField] private float coneAngle = 0f;

    public override void Shoot()
    {
        float constConeAngle = coneAngle;

        if (shootInput == 1f && Time.time > shotTimer)
        {
            shotTimer = Time.time + delayPerShot;

            // ---------- SHOOT UP ----------
            if (verticalInput > verticalInputSensitivity)
            {
                for (int i = 0; i < 5; ++i)
                {
                    GameObject currentBullet = Instantiate(bullet, new Vector3(playerPos.x, weaponPos.y + weaponLength, playerPos.z), Quaternion.Euler(0f, 0f, coneAngle + 90f));
                    currentBullet.GetComponent<BulletPlayer>().direction = Quaternion.AngleAxis(coneAngle, Vector3.forward) * Vector3.up;
                    coneAngle -= constConeAngle / 2;
                }
            }

            // ---------- SHOOT DOWN ----------
            else if (verticalInput < -verticalInputSensitivity && canShoot)
            {
                for (int i = 0; i < 5; ++i)
                {
                    GameObject currentBullet = Instantiate(bullet, new Vector3(playerPos.x, weaponPos.y - weaponLength, playerPos.z), Quaternion.Euler(0f, 0f, coneAngle - 90f));
                    currentBullet.GetComponent<BulletPlayer>().direction = Quaternion.AngleAxis(coneAngle, Vector3.forward) * Vector3.down;
                    coneAngle -= constConeAngle / 2;
                }
            }

            // ---------- SHOOT RIGHT ----------
            if (playerRot == Mathf.Clamp(playerRot, -1f, 1f) && verticalInput == isInSensitivityRange)
            {
                for (int i = 0; i < 5; ++i)
                {
                    GameObject currentBullet = Instantiate(bullet, new Vector3(weaponPos.x + weaponLength, weaponPos.y, playerPos.z), Quaternion.Euler(0f, playerRot, coneAngle));
                    currentBullet.GetComponent<BulletPlayer>().direction = Quaternion.AngleAxis(coneAngle, Vector3.forward) * Vector3.right;
                    coneAngle -= constConeAngle / 2;
                }
            }

            // ---------- SHOOT LEFT ----------
            else if (playerRot == Mathf.Clamp(playerRot, 179f, 181f) && verticalInput == isInSensitivityRange)
            {
                for (int i = 0; i < 5; ++i)
                {
                    GameObject currentBullet = Instantiate(bullet, new Vector3(weaponPos.x - weaponLength, weaponPos.y, playerPos.z), Quaternion.Euler(0f, playerRot, -coneAngle));
                    currentBullet.GetComponent<BulletPlayer>().direction = Quaternion.AngleAxis(coneAngle, Vector3.forward) * Vector3.left;
                    coneAngle -= constConeAngle / 2;
                }
            }
        }

        coneAngle = constConeAngle;
    }
}
