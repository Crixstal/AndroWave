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

            animator.SetTrigger("machineGunShoot");
            shootParticle.transform.position = bulletSpawn;
            shootParticle.Play();
            currentBullet = Instantiate(bullet, 
                                        new Vector3(Random.Range(bulletSpawn.x - spawnRange, bulletSpawn.x + spawnRange), 
                                                    Random.Range(bulletSpawn.y - spawnRange, bulletSpawn.y + spawnRange), bulletSpawn.z), 
                                        Quaternion.Euler(0f, playerRot, shootAngle));

            #region ---------- SHOOT RIGHT ----------
            if (playerRot == Mathf.Clamp(playerRot, -1f, 1f))
            {
                weaponSound.Play();
                currentBullet.GetComponent<BulletPlayer>().direction = Quaternion.AngleAxis(shootAngle, Vector3.forward) * Vector3.right;
            }
            #endregion

            #region ---------- SHOOT LEFT ----------
            else if (playerRot == Mathf.Clamp(playerRot, 179f, 181f))
            {
                weaponSound.Play();
                currentBullet.GetComponent<BulletPlayer>().direction = Quaternion.AngleAxis(shootAngle, Vector3.back) * Vector3.left;
            }
            #endregion
        }
    }
}