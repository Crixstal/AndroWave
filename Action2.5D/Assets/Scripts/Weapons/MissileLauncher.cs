using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileLauncher : WeaponPlayer
{
    public override void Shoot()
    {
        if (shootInput == 1f && Time.time > shotTimer)
        {
            shotTimer = Time.time + delayPerShot;

            // ---------- SHOOT UP ----------
            if (verticalInput > verticalInputSensitivity)
            {
                currentBullet = Instantiate(bullet, new Vector3(playerPos.x, weaponPos.y + weaponLength, playerPos.z), Quaternion.Euler(0f, 0f, 90f));
                currentBullet.GetComponent<BulletPlayer>().direction = Vector3.up;
            }

            // ---------- SHOOT DOWN ----------
            else if (verticalInput < -verticalInputSensitivity && canShoot)
            {
                currentBullet = Instantiate(bullet, new Vector3(playerPos.x, weaponPos.y - weaponLength, playerPos.z), Quaternion.Euler(0f, 0f, 90f));
                currentBullet.GetComponent<BulletPlayer>().direction = Vector3.down;
            }

            // ---------- SHOOT RIGHT ----------
            if (playerRot == Mathf.Clamp(playerRot, -1f, 1f) && verticalInput == isInSensitivityRange)
            {
                currentBullet = Instantiate(bullet, new Vector3(weaponPos.x + weaponLength, weaponPos.y, playerPos.z), Quaternion.Euler(0f, playerRot, 0f));
                currentBullet.GetComponent<BulletPlayer>().direction = Vector3.right;
            }

            // ---------- SHOOT LEFT ----------
            else if (playerRot == Mathf.Clamp(playerRot, 179f, 181f) && verticalInput == isInSensitivityRange)
            {
                currentBullet = Instantiate(bullet, new Vector3(weaponPos.x - weaponLength, weaponPos.y, playerPos.z), Quaternion.Euler(0f, playerRot, 0f));
                currentBullet.GetComponent<BulletPlayer>().direction = Vector3.left;
            }
        }
    }
}
