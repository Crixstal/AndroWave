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
                Instantiate(bullet, new Vector3(playerPos.x, weaponPos.y + weaponLength, weaponPos.z), Quaternion.Euler(0f, 0f, 90f));
                bullet.GetComponent<BulletPlayer>().direction = Vector3.up;
            }

            // ---------- SHOOT DOWN ----------
            else if (verticalInput < -verticalInputSensitivity && canShoot)
            {
                Instantiate(bullet, new Vector3(playerPos.x, weaponPos.y - weaponLength, weaponPos.z), Quaternion.Euler(0f, 0f, -90f));
                bullet.GetComponent<BulletPlayer>().direction = Vector3.down;
            }

            // ---------- SHOOT RIGHT ----------
            if (playerRot == Mathf.Clamp(playerRot, -1f, 1f) && verticalInput == isInSensitivityRange)
            {
                for (int i = 0; i < 5; ++i)
                {
                    Vector3 rotatedVector = Quaternion.AngleAxis(coneAngle, Vector3.forward) * Vector3.right;

                    Instantiate(bullet, new Vector3(weaponPos.x + weaponLength, weaponPos.y, weaponPos.z), Quaternion.Euler(0f, playerRot, coneAngle));
                    bullet.GetComponent<BulletPlayer>().direction = rotatedVector.normalized;
                    coneAngle -= constConeAngle / 2;
                }
            }

            // ---------- SHOOT LEFT ----------
            else if (playerRot == Mathf.Clamp(playerRot, 179f, 181f) && verticalInput == isInSensitivityRange)
            {
                Instantiate(bullet, new Vector3(weaponPos.x - weaponLength, weaponPos.y, weaponPos.z), Quaternion.Euler(0f, playerRot, 0f));
                bullet.GetComponent<BulletPlayer>().direction = Vector3.left;
            }
        }

        coneAngle = constConeAngle;
    }
}
