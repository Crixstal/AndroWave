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
            
            // ---------- SHOOT UP ----------
            if (verticalInput > verticalInputSensitivity)
            {
                GameObject currentBullet = Instantiate(bullet, new Vector3(Random.Range(playerPos.x - spawnRange, playerPos.x + spawnRange), weaponPos.y + weaponLength, weaponPos.z), Quaternion.Euler(0f, 0f, 90f));
                currentBullet.GetComponent<BulletPlayer>().direction = Vector3.up;
            }
            
            // ---------- SHOOT DOWN ----------
            else if (verticalInput < -verticalInputSensitivity && canShoot)
            {
                GameObject currentBullet = Instantiate(bullet, new Vector3(Random.Range(playerPos.x - spawnRange, playerPos.x + spawnRange), weaponPos.y - weaponLength, weaponPos.z), Quaternion.Euler(0f, 0f, -90f));
                currentBullet.GetComponent<BulletPlayer>().direction = Vector3.down;
            }
            
            // ---------- SHOOT RIGHT ----------
            if (playerRot == Mathf.Clamp(playerRot, -1f, 1f) && verticalInput == isInSensitivityRange)
            {
                GameObject currentBullet = Instantiate(bullet, new Vector3(weaponPos.x + weaponLength, Random.Range(weaponPos.y - spawnRange, weaponPos.y + spawnRange), weaponPos.z), Quaternion.Euler(0f, playerRot, 0f));
                currentBullet.GetComponent<BulletPlayer>().direction = Vector3.right;
            }
            
            // ---------- SHOOT LEFT ----------
            else if (playerRot == Mathf.Clamp(playerRot, 179f, 181f) && verticalInput == isInSensitivityRange)
            {
                GameObject currentBullet = Instantiate(bullet, new Vector3(weaponPos.x - weaponLength, Random.Range(weaponPos.y - spawnRange, weaponPos.y + spawnRange), weaponPos.z), Quaternion.Euler(0f, playerRot, 0f));
                currentBullet.GetComponent<BulletPlayer>().direction = Vector3.left;
            }
        }
    }
}