using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineGun : WeaponPlayer
{
    public override void Visor2D()
    {
        inputs = new Vector3(horizontalInput, verticalInput, 0f);

      // ---------- SHOOT UP ----------
        if (verticalInput > verticalInputSensitivity)
            shootAngle = 90f;

      // ---------- SHOOT DOWN ----------
        if (verticalInput < -verticalInputSensitivity)
            shootAngle = -90f;

      // ---------- SHOOT RIGHT ----------
        if (playerRot != Mathf.Clamp(playerRot, -1f, 1f) && horizontalInput > horizontalInputSensitivity) // if not already turned
            player.transform.Rotate(0f, 180f, 0f);

        if (playerRot == Mathf.Clamp(playerRot, -1f, 1f) && verticalInput == sensitivityRange)
        {
            shootAngle = Vector3.Angle(Vector3.right, inputs);

            if (verticalInput < 0)
                shootAngle = Vector3.Angle(Vector3.right, -inputs);
        }

      // ---------- SHOOT LEFT ----------
        if (playerRot != Mathf.Clamp(playerRot, 179f, 181f) && horizontalInput < -horizontalInputSensitivity) // if not already turned
            player.transform.Rotate(0f, 180f, 0f);

        if (playerRot == Mathf.Clamp(playerRot, 179f, 181f) && verticalInput == sensitivityRange)
        {
            shootAngle = Vector3.Angle(Vector3.left, -inputs);

            if (verticalInput < 0)
                shootAngle = Vector3.Angle(Vector3.left, inputs);
        }

        bulletRot = Quaternion.Euler(0f, 0f, shootAngle);
    }

    public override void Shoot()
    {
        if (shootInput == 1f && Time.time > timestamp)
        {
            timestamp = Time.time + perShotDelay;

            //Instantiate(bullet, player.transform.position, bulletRot);

            if (verticalInput > verticalInputSensitivity) // shoot up
            {
                Instantiate(bullet, new Vector3 (0f, player.transform.position.y + player.transform.localScale.y, 0f), bulletRot);
                bullet.GetComponent<BulletPlayer>().direction = Vector3.up;
            }

            else if (verticalInput < -verticalInputSensitivity && canShoot) // shoot down
            {
                Instantiate(bullet, player.transform.position, bulletRot);
                bullet.GetComponent<BulletPlayer>().direction = Vector3.down;
            }

            if (playerRot == Mathf.Clamp(playerRot, -1f, 1f) && verticalInput == sensitivityRange) // shoot right
            {
                Instantiate(bullet, player.transform.position, bulletRot);
                bullet.GetComponent<BulletPlayer>().direction = Vector3.right;
            }

            else if (playerRot == Mathf.Clamp(playerRot, 179f, 181f) && verticalInput == sensitivityRange) // shoot left
            {
                Instantiate(bullet, player.transform.position, bulletRot);
                bullet.GetComponent<BulletPlayer>().direction = Vector3.left;
            }
        }
    }
}