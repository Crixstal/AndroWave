using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineGun : WeaponPlayer
{
    public override void Visor2D()
    {
        inputs = new Vector3(horizontalInput, verticalInput, 0f);

        if (playerRot == Mathf.Clamp(playerRot, 89f, 91f) || playerRot == Mathf.Clamp(playerRot, 269f, 271f)) // don't rotate if in 3D shooting
            return;

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
            shootAngle = Vector3.Angle(Vector3.right, inputs) * 2f;

            if (verticalInput < 0)
                shootAngle = Vector3.Angle(Vector3.right, -inputs) * 2f;
        }

      // ---------- SHOOT LEFT ----------
        if (playerRot != Mathf.Clamp(playerRot, 179f, 181f) && horizontalInput < -horizontalInputSensitivity) // if not already turned
            player.transform.Rotate(0f, 180f, 0f);

        if (playerRot == Mathf.Clamp(playerRot, 179f, 181f) && verticalInput == sensitivityRange)
        {
            shootAngle = Vector3.Angle(Vector3.left, -inputs) * 2f;

            if (verticalInput < 0)
                shootAngle = Vector3.Angle(Vector3.left, inputs) * 2f;
        }

        bulletRot = Quaternion.Euler(0f, 0f, shootAngle);
    }

    public override void Visor3D()
    {
        float shootInput = Input.GetAxisRaw("Shoot");

        if (shootInput == -1f)
        {
            if (rightTriggerIsInUse == false)
            {
                // ---------- SHOOT BACKGROUND ----------
                if (player.transform.position.z == posForeground)
                {
                    if (playerRot == Mathf.Clamp(playerRot, -1f, 1f)) // if look right
                        player.transform.Rotate(0f, -90f, 0f);      // rotate toward back

                    else if (playerRot == Mathf.Clamp(playerRot, 179f, 181f)) // if look left
                        player.transform.Rotate(0f, 90f, 0f);

                    else if (playerRot == Mathf.Clamp(playerRot, 269f, 271f)) // if look back
                        player.transform.Rotate(0f, 90f, 0f);

                    shootAngle = Vector3.Angle(Vector3.right, inputs) * 2f;
                    Debug.Log(shootAngle);

                    rightTriggerIsInUse = true;
                }

                // ---------- SHOOT FOREGROUND ----------
                if (player.transform.position.z == posBackground)
                {
                    if (playerRot == Mathf.Clamp(playerRot, -1f, 1f)) // if look right
                        player.transform.Rotate(0f, 90f, 0f);       // rotate toward front

                    else if (playerRot == Mathf.Clamp(playerRot, 179f, 181f)) // if look left
                        player.transform.Rotate(0f, -90f, 0f);

                    else if (playerRot == Mathf.Clamp(playerRot, 89f, 91f)) // if look front
                        player.transform.Rotate(0f, -90f, 0f);

                    shootAngle = Vector3.Angle(Vector3.left, inputs) * 2f;

                    rightTriggerIsInUse = true;
                }
            }

            bulletRot = Quaternion.Euler(0f, 90f, shootAngle);
        }

        else
            rightTriggerIsInUse = false;
    }

    public override void Shoot()
    {
        if (shootInput == 1f && Time.time > timestamp)
        {
            timestamp = Time.time + perShotDelay;

            Instantiate(bullet, player.transform.position, bulletRot);

            if (playerRot != Mathf.Clamp(playerRot, 89f, 91f) || playerRot != Mathf.Clamp(playerRot, 269f, 271f))
            {
                if (verticalInput > verticalInputSensitivity) // shoot up
                    bullet.GetComponent<BulletPlayer>().direction = Vector3.Normalize(new Vector3(0f, shootAngle, 0f));

                else if (verticalInput < -verticalInputSensitivity && canShoot) // shoot down
                    bullet.GetComponent<BulletPlayer>().direction = Vector3.Normalize(new Vector3(0f, shootAngle, 0f));
            }

            if (playerRot == Mathf.Clamp(playerRot, -1f, 1f) && verticalInput == sensitivityRange) // shoot right
                bullet.GetComponent<BulletPlayer>().direction = Vector3.Normalize(new Vector3(player.transform.position.x, verticalInput * shootAngle, 0f));

            else if (playerRot == Mathf.Clamp(playerRot, 179f, 181f) && verticalInput == sensitivityRange) // shoot left
                bullet.GetComponent<BulletPlayer>().direction = Vector3.Normalize(new Vector3(-player.transform.position.x, verticalInput * shootAngle, 0f));

            if (playerRot == Mathf.Clamp(playerRot, 89f, 91f)) // shoot foreground
                bullet.GetComponent<BulletPlayer>().direction = Vector3.Normalize(new Vector3(horizontalInput * angleShoot3D, verticalInput * angleShoot3D, -player.transform.position.z));

            else if (playerRot == Mathf.Clamp(playerRot, 269f, 271f)) // shoot background
                bullet.GetComponent<BulletPlayer>().direction = Vector3.Normalize(new Vector3(horizontalInput * angleShoot3D, verticalInput * angleShoot3D, player.transform.position.z));
        }
    }
}