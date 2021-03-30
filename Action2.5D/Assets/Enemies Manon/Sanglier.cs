using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sanglier : Enemy
{
    private GameObject currentBullet;

    public override void Shoot()
    {
        if (Time.time > shotTimer)
        {
            shotTimer = Time.time + delayPerShot;

            // ---------- SHOOT UP ----------
            if (weaponRot == Mathf.Clamp(weaponRot, 89f, 91f))
                currentBullet = Instantiate(bullet, new Vector3(enemyPos.x, weaponPos.y + weaponLength, enemyPos.z), Quaternion.Euler(0f, enemyRot, weapon.transform.rotation.eulerAngles.z));

            // ---------- SHOOT DOWN ----------
            else if (weaponRot == Mathf.Clamp(weaponRot, 269f, 271f))
                currentBullet = Instantiate(bullet, new Vector3(enemyPos.x, weaponPos.y - weaponLength, enemyPos.z), Quaternion.Euler(0f, enemyRot, weapon.transform.rotation.eulerAngles.z));


            // ---------- SHOOT RIGHT ----------
            if (enemyRot == Mathf.Clamp(enemyRot, -1f, 1f))
                currentBullet = Instantiate(bullet, new Vector3(weaponPos.x + weaponLength, weaponPos.y, enemyPos.z), Quaternion.Euler(0f, enemyRot, weapon.transform.rotation.eulerAngles.z));

            // ---------- SHOOT LEFT ----------
            else if (enemyRot == Mathf.Clamp(enemyRot, 179f, 181f))
                currentBullet = Instantiate(bullet, new Vector3(weaponPos.x - weaponLength, weaponPos.y, enemyPos.z), Quaternion.Euler(0f, enemyRot, weapon.transform.rotation.eulerAngles.z));


            currentBullet.GetComponent<BulletEnemy>().direction = Vector3.Normalize(relativePos);
        }
    }
}
