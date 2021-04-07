using System.Collections;
using System.Collections.Generic;
using UnityEngine;

        //shootAngle = transform.eulerAngles.z;

public class MachineGun : WeaponPlayer
{
    [SerializeField] private float spawnRange = 0f;

    public override void Shoot()
    {
        if (shootInput == 1f && Time.time > shotTimer)
        {
            shotTimer = Time.time + delayPerShot;

            #region ---------- SHOOT UP & DOWN ----------
            if (horizontalInput == isInXRange)
            {
                if (verticalInput > verticalInputSensitivity) // shoot up
                {
                    currentBullet = Instantiate(bullet, new Vector3(Random.Range(playerPos.x - spawnRange, playerPos.x + spawnRange), weaponPos.y + weaponLength, playerPos.z), Quaternion.Euler(0f, 0f, shootAngle));
                    weaponSound.Play();
                    currentBullet.GetComponent<BulletPlayer>().direction = Vector3.up;
                }

                else if (verticalInput < -verticalInputSensitivity) // shoot down
                {
                    currentBullet = Instantiate(bullet, new Vector3(Random.Range(playerPos.x - spawnRange, playerPos.x + spawnRange), weaponPos.y - weaponLength, playerPos.z), Quaternion.Euler(0f, 0f, shootAngle));
                    weaponSound.Play();
                    currentBullet.GetComponent<BulletPlayer>().direction = Vector3.down;
                }
            }
            #endregion

            #region ---------- SHOOT RIGHT ----------
            if (playerRot == Mathf.Clamp(playerRot, -1f, 1f) && verticalInput == isInYRange) // shoot right
            {
                currentBullet = Instantiate(bullet, new Vector3(weaponPos.x + weaponLength, Random.Range(weaponPos.y - spawnRange, weaponPos.y + spawnRange), playerPos.z), Quaternion.Euler(0f, playerRot, shootAngle));
                weaponSound.Play();
                currentBullet.GetComponent<BulletPlayer>().direction = Quaternion.AngleAxis(shootAngle, Vector3.forward) * Vector3.right;
            }
            #endregion

            #region ---------- SHOOT LEFT ----------
            else if (playerRot == Mathf.Clamp(playerRot, 179f, 181f) && verticalInput == isInYRange) // shoot left
            {
                currentBullet = Instantiate(bullet, new Vector3(weaponPos.x - weaponLength, Random.Range(weaponPos.y - spawnRange, weaponPos.y + spawnRange), playerPos.z), Quaternion.Euler(0f, playerRot, shootAngle));
                weaponSound.Play();
                currentBullet.GetComponent<BulletPlayer>().direction = Quaternion.AngleAxis(shootAngle, Vector3.back) * Vector3.left;
            }
            #endregion
        }
    }
}