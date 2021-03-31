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

            #region // ---------- SHOOT RIGHT & LEFT ----------
            if (verticalInput == isInYRange)
            {
                if (playerRot == Mathf.Clamp(playerRot, -1f, 1f)) // RIGHT
                {
                    for (int i = 0; i < 5; ++i)
                    {
                        currentBullet = Instantiate(bullet, new Vector3(weaponPos.x + weaponLength, weaponPos.y, playerPos.z), Quaternion.Euler(0f, playerRot, coneAngle + shootAngle));
                        currentBullet.GetComponent<BulletPlayer>().direction = Quaternion.AngleAxis(coneAngle, Vector3.forward) * Vector3.right;
                        coneAngle -= constConeAngle / 2;
                    }
                }

                else if (playerRot == Mathf.Clamp(playerRot, 179f, 181f)) // LEFT
                {
                    for (int i = 0; i < 5; ++i)
                    {
                        currentBullet = Instantiate(bullet, new Vector3(weaponPos.x - weaponLength, weaponPos.y, playerPos.z), Quaternion.Euler(0f, playerRot, -coneAngle + shootAngle));
                        currentBullet.GetComponent<BulletPlayer>().direction = Quaternion.AngleAxis(coneAngle, Vector3.forward) * Vector3.left;
                        coneAngle -= constConeAngle / 2;
                    }
                }
            }
            #endregion

            #region // -----------SHOOT UP ----------
            if (verticalInput > verticalInputSensitivity) // UP
            {
                if (horizontalInput == isInXRange) // UP
                {
                    for (int i = 0; i < 5; ++i)
                    {
                        currentBullet = Instantiate(bullet, new Vector3(playerPos.x, weaponPos.y + weaponLength, playerPos.z), Quaternion.Euler(0f, 0f, coneAngle + shootAngle));
                        currentBullet.GetComponent<BulletPlayer>().direction = Quaternion.AngleAxis(coneAngle, Vector3.forward) * Vector3.up;
                        coneAngle -= constConeAngle / 2;
                    }
                }

                if (horizontalInput > horizontalInputSensitivity) // RIGHT
                {
                    for (int i = 0; i < 5; ++i)
                    {
                        currentBullet = Instantiate(bullet, new Vector3(weaponPos.x + weaponLength, weaponPos.y, playerPos.z), Quaternion.Euler(0f, playerRot, coneAngle + shootAngle));
                        currentBullet.GetComponent<BulletPlayer>().direction = Quaternion.AngleAxis(coneAngle + shootAngle, Vector3.forward) * Vector3.right;
                        coneAngle -= constConeAngle / 2;
                    }
                }

                if (horizontalInput < -horizontalInputSensitivity) // LEFT
                {
                    for (int i = 0; i < 5; ++i)
                    {
                        currentBullet = Instantiate(bullet, new Vector3(weaponPos.x - weaponLength, weaponPos.y, playerPos.z), Quaternion.Euler(0f, playerRot, -coneAngle + shootAngle));
                        currentBullet.GetComponent<BulletPlayer>().direction = Quaternion.AngleAxis(coneAngle - shootAngle, Vector3.forward) * Vector3.left;
                        coneAngle -= constConeAngle / 2;
                    }
                }
            }
            #endregion

            #region // ---------- SHOOT DOWN ----------
            else if (verticalInput < -verticalInputSensitivity && canShoot) // DOWN
            {
                if (horizontalInput == isInXRange) // DOWN
                {
                    for (int i = 0; i < 5; ++i)
                    {
                        currentBullet = Instantiate(bullet, new Vector3(playerPos.x, weaponPos.y - weaponLength, playerPos.z), Quaternion.Euler(0f, 0f, coneAngle + shootAngle));
                        currentBullet.GetComponent<BulletPlayer>().direction = Quaternion.AngleAxis(coneAngle, Vector3.forward) * Vector3.down;
                        coneAngle -= constConeAngle / 2;
                    }
                }

                if (horizontalInput > horizontalInputSensitivity) // RIGHT
                {
                    for (int i = 0; i < 5; ++i)
                    {
                        currentBullet = Instantiate(bullet, new Vector3(weaponPos.x + weaponLength, weaponPos.y, playerPos.z), Quaternion.Euler(0f, playerRot, coneAngle + shootAngle));
                        currentBullet.GetComponent<BulletPlayer>().direction = Quaternion.AngleAxis(coneAngle + shootAngle, Vector3.forward) * Vector3.right;
                        coneAngle -= constConeAngle / 2;
                    }
                }

                if (horizontalInput < -horizontalInputSensitivity) // LEFT
                {
                    for (int i = 0; i < 5; ++i)
                    {
                        currentBullet = Instantiate(bullet, new Vector3(weaponPos.x - weaponLength, weaponPos.y, playerPos.z), Quaternion.Euler(0f, playerRot, -coneAngle + shootAngle));
                        currentBullet.GetComponent<BulletPlayer>().direction = Quaternion.AngleAxis(coneAngle - shootAngle, Vector3.forward) * Vector3.left;
                        coneAngle -= constConeAngle / 2;
                    }
                }
            }
            #endregion
        }

        coneAngle = constConeAngle;
    }
}
