using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineGun : WeaponPlayer
{
    [SerializeField] private float spawnRange = 0f;

   /* private void Visor()
    {
        inputs = new Vector3(horizontalInput, verticalInput, 0f);

        // ---------- SHOOT UP ----------
        if (verticalInput > verticalInputSensitivity)
            shootAngle = 90f;

        // ---------- SHOOT DOWN ----------
        if (verticalInput < -verticalInputSensitivity)
            shootAngle = -90f;

        // ---------- SHOOT RIGHT ----------
        if (playerRot == Mathf.Clamp(playerRot, -1f, 1f) && verticalInput == sensitivityRange)
        {
            shootAngle = Vector3.Angle(Vector3.right, inputs);

            if (verticalInput < 0)
                shootAngle = Vector3.Angle(Vector3.right, -inputs);
        }

        // ---------- SHOOT LEFT ----------
        if (playerRot == Mathf.Clamp(playerRot, 179f, 181f) && verticalInput == sensitivityRange)
        {
            shootAngle = Vector3.Angle(Vector3.left, -inputs);

            if (verticalInput < 0)
                shootAngle = Vector3.Angle(Vector3.left, inputs);
        }

        bulletRot = Quaternion.Euler(0f, 0f, shootAngle);
    }*/

    public override void Shoot()
    {
        if (shootInput == 1f && Time.time > shotTimer)
        {
            shotTimer = Time.time + delayPerShot;
            
            // ---------- SHOOT UP ----------
            if (verticalInput > verticalInputSensitivity)
            {
                currentBullet = Instantiate(bullet, new Vector3(Random.Range(playerPos.x - spawnRange, playerPos.x + spawnRange), weaponPos.y + weaponLength, playerPos.z), Quaternion.Euler(0f, 0f, 90f));
                currentBullet.GetComponent<BulletPlayer>().direction = Vector3.up;
            }
            
            // ---------- SHOOT DOWN ----------
            else if (verticalInput < -verticalInputSensitivity && canShoot)
            {
                currentBullet = Instantiate(bullet, new Vector3(Random.Range(playerPos.x - spawnRange, playerPos.x + spawnRange), weaponPos.y - weaponLength, playerPos.z), Quaternion.Euler(0f, 0f, -90f));
                currentBullet.GetComponent<BulletPlayer>().direction = Vector3.down;
            }
            
            // ---------- SHOOT RIGHT ----------
            if (playerRot == Mathf.Clamp(playerRot, -1f, 1f) && verticalInput == isInSensitivityRange)
            {
                currentBullet = Instantiate(bullet, new Vector3(weaponPos.x + weaponLength, Random.Range(weaponPos.y - spawnRange, weaponPos.y + spawnRange), playerPos.z), Quaternion.Euler(0f, playerRot, 0f));
                currentBullet.GetComponent<BulletPlayer>().direction = Vector3.right;
            }
            
            // ---------- SHOOT LEFT ----------
            else if (playerRot == Mathf.Clamp(playerRot, 179f, 181f) && verticalInput == isInSensitivityRange)
            {
                currentBullet = Instantiate(bullet, new Vector3(weaponPos.x - weaponLength, Random.Range(weaponPos.y - spawnRange, weaponPos.y + spawnRange), playerPos.z), Quaternion.Euler(0f, playerRot, 0f));
                currentBullet.GetComponent<BulletPlayer>().direction = Vector3.left;
            }
        }
    }
}