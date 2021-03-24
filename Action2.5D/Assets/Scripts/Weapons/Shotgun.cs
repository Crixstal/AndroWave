using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : WeaponPlayer
{
    public override void Visor2D()
    {       
        // ---------- SHOOT UP ----------
        if (verticalInput > verticalInputSensitivity)
            shootAngle = 90f;

        // ---------- SHOOT DOWN ----------
        if (verticalInput < -verticalInputSensitivity)
            shootAngle = -90f;

        // ---------- SHOOT RIGHT ----------
        if (playerRot != Mathf.Clamp(playerRot, -1f, 1f) && horizontalInput > horizontalInputSensitivity) // rotate if not already turned
            player.transform.Rotate(0f, 180f, 0f);

        if (playerRot == Mathf.Clamp(playerRot, -1f, 1f) && verticalInput == sensitivityRange)
            shootAngle = 0f;

        // ---------- SHOOT LEFT ----------
        if (playerRot != Mathf.Clamp(playerRot, 179f, 181f) && horizontalInput < -horizontalInputSensitivity) // rotate if not already turned
            player.transform.Rotate(0f, 180f, 0f);

        if (playerRot == Mathf.Clamp(playerRot, 179f, 181f) && verticalInput == sensitivityRange)
            shootAngle = 180f;

        bulletRot = Quaternion.Euler(0f, 0f, shootAngle);
    }

    public override void Shoot()
    {
        if (shootInput == 1f && Time.time > timestamp)
        {
            timestamp = Time.time + perShotDelay;

            //for (int i = 0; i < 5; ++i)
                Instantiate(bullet, player.transform.position, bulletRot);

            if (verticalInput > verticalInputSensitivity) // shoot up
                bullet.GetComponent<BulletPlayer>().direction = Vector3.Normalize(new Vector3(0f, player.transform.position.y, 0f));

            else if (verticalInput < -verticalInputSensitivity && canShoot) // shoot down
                bullet.GetComponent<BulletPlayer>().direction = Vector3.Normalize(new Vector3(0f, -player.transform.position.y, 0f));

            if (playerRot == Mathf.Clamp(playerRot, -1f, 1f) && verticalInput == sensitivityRange) // shoot right
                bullet.GetComponent<BulletPlayer>().direction = Vector3.Normalize(new Vector3(player.transform.position.x, 0f, 0f));

            else if (playerRot == Mathf.Clamp(playerRot, 179f, 181f) && verticalInput == sensitivityRange) // shoot left
                bullet.GetComponent<BulletPlayer>().direction = Vector3.Normalize(new Vector3(-player.transform.position.x, 0f, 0f));
        }
    }
}
