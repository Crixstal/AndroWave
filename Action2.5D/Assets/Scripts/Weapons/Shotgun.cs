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

            #region ---------- SHOOT RIGHT ----------
            if (playerRot == Mathf.Clamp(playerRot, -1f, 1f))
            {
                for (int i = 0; i < 5; ++i)
                {
                    animator.SetTrigger("shotgunShoot");
                    animator.SetTrigger("mitrapompeShoot");
                    shootParticle.transform.position = bulletSpawn;
                    shootParticle.Play();

                    currentBullet = Instantiate(bullet, bulletSpawn, Quaternion.Euler(0f, playerRot, shootAngle));
                    weaponSound.Play();
                    currentBullet.GetComponent<BulletPlayer>().direction = Quaternion.AngleAxis(coneAngle + shootAngle, Vector3.forward) * Vector3.right;
                    coneAngle -= constConeAngle / 2;
                }
            }
            #endregion

            #region ---------- SHOOT LEFT ----------
            else if (playerRot == Mathf.Clamp(playerRot, 179f, 181f))
            {
                for (int i = 0; i < 5 ; ++i)
                {
                    animator.SetTrigger("shotgunShoot");
                    animator.SetTrigger("mitrapompeShoot");
                    shootParticle.transform.position = bulletSpawn;
                    shootParticle.Play();

                    currentBullet = Instantiate(bullet, bulletSpawn, Quaternion.Euler(0f, playerRot, shootAngle));
                    weaponSound.Play();
                    currentBullet.GetComponent<BulletPlayer>().direction = Quaternion.AngleAxis(coneAngle - shootAngle, Vector3.forward) * Vector3.left;
                    coneAngle -= constConeAngle / 2;
                }
            }
            #endregion
        }

        coneAngle = constConeAngle;
    }
}
