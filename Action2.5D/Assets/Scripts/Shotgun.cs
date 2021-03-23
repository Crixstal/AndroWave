using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : WeaponPlayer
{
    public override void Visor2D()
    {
        float horizontalInput = Input.GetAxis("HorizontalVisor");
        float verticalInput = Input.GetAxis("VerticalVisor");
        float rotation = player.transform.rotation.eulerAngles.y;
        float sensitivityRange = Mathf.Clamp(verticalInput, -verticalInputSensitivity, verticalInputSensitivity);

        if (rotation == Mathf.Clamp(rotation, 89f, 91f) || rotation == Mathf.Clamp(rotation, 269f, 271f)) // don't rotate when in 3D shooting
            return;

        // ----- SHOOT UP -----
        if (verticalInput > verticalInputSensitivity)
            bullet.transform.position = new Vector3(player.transform.position.x, player.transform.position.y + player.transform.localScale.y, player.transform.position.z);

        // ----- SHOOT DOWN -----
        if (verticalInput < -verticalInputSensitivity)
            bullet.transform.position = new Vector3(player.transform.position.x, player.transform.position.y - player.transform.localScale.y, player.transform.position.z);

        // ----- SHOOT RIGHT -----
        if (rotation != Mathf.Clamp(rotation, -1f, 1f) && verticalInput == sensitivityRange && horizontalInput > horizontalInputSensitivity) // if not already turned
            player.transform.Rotate(0f, 180f, 0f);
        if (rotation == Mathf.Clamp(rotation, -1f, 1f))
            bullet.transform.position = new Vector3(player.transform.position.x + player.transform.localScale.x, player.transform.position.y, player.transform.position.z);

        // ----- SHOOT LEFT -----
        if (rotation != Mathf.Clamp(rotation, 179f, 181f) && verticalInput == sensitivityRange && horizontalInput < -horizontalInputSensitivity) // if not already turned
            player.transform.Rotate(0f, 180f, 0f);
        if (rotation == Mathf.Clamp(rotation, 179f, 181f))
            bullet.transform.position = new Vector3(player.transform.position.x - player.transform.localScale.x, player.transform.position.y, player.transform.position.z);
    }

    public override void Visor3D()
    {
        float shootInput = Input.GetAxisRaw("Shoot");
        float rotation = player.transform.rotation.eulerAngles.y;

        if (shootInput == -1f)
        {
            if (rightTriggerIsInUse == false)
            {
                // ----- SHOOT BACKGROUND -----
                if (player.transform.position.z == posForeground)
                {
                    if (rotation == Mathf.Clamp(rotation, -1f, 1f))
                        player.transform.Rotate(0f, -90f, 0f);

                    else if (rotation == Mathf.Clamp(rotation, 179f, 181f))
                        player.transform.Rotate(0f, 90f, 0f);

                    else if (rotation == Mathf.Clamp(rotation, 269f, 271f))
                        player.transform.Rotate(0f, 90f, 0f);

                    bullet.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z + player.transform.localScale.z);

                    rightTriggerIsInUse = true;
                }

                // ----- SHOOT FOREGROUND -----
                if (player.transform.position.z == posBackground)
                {
                    if (rotation == Mathf.Clamp(rotation, -1f, 1f))
                        player.transform.Rotate(0f, 90f, 0f);

                    else if (rotation == Mathf.Clamp(rotation, 179f, 181f))
                        player.transform.Rotate(0f, -90f, 0f);

                    else if (rotation == Mathf.Clamp(rotation, 89f, 91f))
                        player.transform.Rotate(0f, -90f, 0f);

                    bullet.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z - player.transform.localScale.z);

                    rightTriggerIsInUse = true;
                }
            }
        }

        else
            rightTriggerIsInUse = false;
    }

    /*public override void Shoot()
    {
        float shootInput = Input.GetAxisRaw("Shoot");
        float horizontalInput = Input.GetAxis("HorizontalVisor");
        float verticalInput = Input.GetAxis("VerticalVisor");
        float rotation = player.transform.rotation.eulerAngles.y;
        float sensitivityRange = Mathf.Clamp(verticalInput, -verticalInputSensitivity, verticalInputSensitivity);

        if (shootInput == 1f && Time.time > timestamp)
        {
            timestamp = Time.time + perShotDelay;

            Instantiate(bullet);

            if (rotation != Mathf.Clamp(rotation, 89f, 91f) || rotation != Mathf.Clamp(rotation, 269f, 271f))
            {
                if (verticalInput > verticalInputSensitivity) // shoot up
                    bullet.GetComponent<BulletPlayer>().direction = Vector3.Normalize(new Vector3(0f, player.transform.position.y, 0f));

                else if (verticalInput < -verticalInputSensitivity && canShoot) // shoot down
                    bullet.GetComponent<BulletPlayer>().direction = Vector3.Normalize(new Vector3(0f, -player.transform.position.y, 0f));
            }

            if (rotation == Mathf.Clamp(rotation, -1f, 1f) && verticalInput == sensitivityRange) // shoot right
                bullet.GetComponent<BulletPlayer>().direction = Vector3.Normalize(new Vector3(player.transform.position.x, verticalInput * angleShoot2D, 0f));

            else if (rotation == Mathf.Clamp(rotation, 179f, 181f) && verticalInput == sensitivityRange) // shoot left
                bullet.GetComponent<BulletPlayer>().direction = Vector3.Normalize(new Vector3(-player.transform.position.x, verticalInput * angleShoot2D, 0f));

            if (rotation == Mathf.Clamp(rotation, 89f, 91f)) // shoot foreground
                bullet.GetComponent<BulletPlayer>().direction = Vector3.Normalize(new Vector3(horizontalInput * angleShoot3D, verticalInput * angleShoot3D, -player.transform.position.z));

            else if (rotation == Mathf.Clamp(rotation, 269f, 271f)) // shoot background
                bullet.GetComponent<BulletPlayer>().direction = Vector3.Normalize(new Vector3(horizontalInput * angleShoot3D, verticalInput * angleShoot3D, player.transform.position.z));
        }
    }*/
}