using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineGun : WeaponPlayer
{
    [SerializeField] private float spawnRange = 0f;

    public override void Shoot()
    {
        if (shootInput == 1f && Time.time > shotTimer)
        {
            shotTimer = Time.time + delayPerShot;

            #region // ---------- SHOOT RIGHT & LEFT ----------
            if (verticalInput == isInYRange)
            {
                if (playerRot == Mathf.Clamp(playerRot, -1f, 1f)) // RIGHT
                {
                    currentBullet = Instantiate(bullet, new Vector3(weaponPos.x + weaponLength, Random.Range(weaponPos.y - spawnRange, weaponPos.y + spawnRange), playerPos.z), Quaternion.Euler(0f, playerRot, shootAngle));
                    currentBullet.GetComponent<BulletPlayer>().direction = Vector3.right;
                }

                else if (playerRot == Mathf.Clamp(playerRot, 179f, 181f)) // LEFT
                {
                    currentBullet = Instantiate(bullet, new Vector3(weaponPos.x - weaponLength, Random.Range(weaponPos.y - spawnRange, weaponPos.y + spawnRange), playerPos.z), Quaternion.Euler(0f, playerRot, shootAngle));
                    currentBullet.GetComponent<BulletPlayer>().direction = Vector3.left;
                }
            }
            #endregion

            #region // -----------SHOOT UP ----------
            if (verticalInput > verticalInputSensitivity) // UP
            {
                if (horizontalInput == isInXRange) // UP
                {
                    currentBullet = Instantiate(bullet, new Vector3(Random.Range(playerPos.x - spawnRange, playerPos.x + spawnRange), weaponPos.y + weaponLength, playerPos.z), Quaternion.Euler(0f, 0f, shootAngle));
                    currentBullet.GetComponent<BulletPlayer>().direction = Vector3.up;
                }

                if (horizontalInput > horizontalInputSensitivity) // RIGHT
                {
                    currentBullet = Instantiate(bullet, new Vector3(weaponPos.x + weaponLength, Random.Range(weaponPos.y - spawnRange, weaponPos.y + spawnRange), playerPos.z), Quaternion.Euler(0f, playerRot, shootAngle));
                    currentBullet.GetComponent<BulletPlayer>().direction = Quaternion.AngleAxis(shootAngle, Vector3.forward) * Vector3.right;
                }

                if (horizontalInput < -horizontalInputSensitivity) // LEFT
                {
                    currentBullet = Instantiate(bullet, new Vector3(weaponPos.x - weaponLength, Random.Range(weaponPos.y - spawnRange, weaponPos.y + spawnRange), playerPos.z), Quaternion.Euler(0f, playerRot, shootAngle));
                    currentBullet.GetComponent<BulletPlayer>().direction = Quaternion.AngleAxis(shootAngle, Vector3.back) * Vector3.left;
                }
            }
            #endregion

            #region // ---------- SHOOT DOWN ----------
            else if (verticalInput < -verticalInputSensitivity && canShoot) // DOWN
            {
                if (horizontalInput == isInXRange) // DOWN
                {
                    currentBullet = Instantiate(bullet, new Vector3(Random.Range(playerPos.x - spawnRange, playerPos.x + spawnRange), weaponPos.y - weaponLength, playerPos.z), Quaternion.Euler(0f, 0f, shootAngle));
                    currentBullet.GetComponent<BulletPlayer>().direction = Vector3.down;
                }

                if (horizontalInput > horizontalInputSensitivity) // RIGHT
                {
                    currentBullet = Instantiate(bullet, new Vector3(weaponPos.x + weaponLength, Random.Range(weaponPos.y - spawnRange, weaponPos.y + spawnRange), playerPos.z), Quaternion.Euler(0f, playerRot, shootAngle));
                    currentBullet.GetComponent<BulletPlayer>().direction = Quaternion.AngleAxis(shootAngle, Vector3.forward) * Vector3.right;
                }

                if (horizontalInput < -horizontalInputSensitivity) // LEFT
                {
                    currentBullet = Instantiate(bullet, new Vector3(weaponPos.x - weaponLength, Random.Range(weaponPos.y - spawnRange, weaponPos.y + spawnRange), playerPos.z), Quaternion.Euler(0f, playerRot, shootAngle));
                    currentBullet.GetComponent<BulletPlayer>().direction = Quaternion.AngleAxis(shootAngle, Vector3.back) * Vector3.left;
                }
            }
            #endregion
        }
    }
}