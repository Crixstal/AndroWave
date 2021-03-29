using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Railgun : WeaponPlayer
{
    [SerializeField] private Color startLaserColor = Color.blue;
    [SerializeField] private Color endLaserColor = Color.blue;
    [SerializeField] private float startLaserWidth = 0f;
    [SerializeField] private float endLaserWidth = 0f;

    private LineRenderer laser;
    private Ray rayOrigin;
    private RaycastHit rayHit;

    private Vector3 end;
    readonly private int layerMask = ~(1 << 10); // ignore layer 10 : Platform

    void Start()
    {
        player = gameObject.GetComponentInParent<Player>();
        weaponLength = transform.localScale.x;

        laser = gameObject.AddComponent<LineRenderer>();

        laser.startWidth = startLaserWidth;
        laser.endWidth = endLaserWidth;

        laser.material = new Material(Shader.Find("Sprites/Default"));

        //laser.colorGradient = gradient;
        laser.startColor = startLaserColor;
        laser.endColor = endLaserColor;
    }

    private void DrawLaserNoHit(Vector3 origin, Vector3 end)
    {
        laser.SetPosition(0, origin);

        if (verticalInput > verticalInputSensitivity)
            laser.SetPosition(1, new Vector3(playerPos.x, end.y, playerPos.z));

        else if (verticalInput < -verticalInputSensitivity && canShoot)
            laser.SetPosition(1, new Vector3(playerPos.x, end.y, playerPos.z));

        if (playerRot == Mathf.Clamp(playerRot, -1f, 1f) || playerRot == Mathf.Clamp(playerRot, 179f, 181f) && verticalInput == isInSensitivityRange)
            laser.SetPosition(1, new Vector3(end.x, weaponPos.y, playerPos.z));

        laser.enabled = true;
    }
    private void DrawLaserHit(Vector3 origin, Vector3 end)
    {
        laser.SetPosition(0, origin);
        laser.SetPosition(1, end);
        laser.enabled = true;
    }

    public override void Shoot()
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(new Vector3(weaponPos.x + Camera.main.nearClipPlane, weaponPos.y + Camera.main.nearClipPlane, playerPos.z + Camera.main.nearClipPlane));

        laser.enabled = false;

        if (shootInput == 1f)
        {
            shotTimer += Time.deltaTime;

            // ---------- SHOOT UP ----------
            if (verticalInput > verticalInputSensitivity)
            {
                rayOrigin = new Ray(new Vector3(playerPos.x, weaponPos.y + weaponLength, playerPos.z), Vector3.up);
                end = Camera.main.ScreenToWorldPoint(new Vector3(playerPos.x, Screen.height, screenPos.z));
            }

            // ---------- SHOOT DOWN ----------
            else if (verticalInput < -verticalInputSensitivity && canShoot)
            {
                rayOrigin = new Ray(new Vector3(playerPos.x, weaponPos.y - weaponLength, playerPos.z), Vector3.down);
                end = Camera.main.ScreenToWorldPoint(new Vector3(playerPos.x, 0, screenPos.z));
            }

            // ---------- SHOOT RIGHT ----------
            if (playerRot == Mathf.Clamp(playerRot, -1f, 1f) && verticalInput == isInSensitivityRange)
            {
                rayOrigin = new Ray(new Vector3(weaponPos.x + weaponLength, weaponPos.y, playerPos.z), Vector3.right);
                end = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, weaponPos.y, screenPos.z));
            }

            // ---------- SHOOT LEFT ----------
            else if (playerRot == Mathf.Clamp(playerRot, 179f, 181f) && verticalInput == isInSensitivityRange)
            {
                rayOrigin = new Ray(new Vector3(weaponPos.x - weaponLength, weaponPos.y, playerPos.z), Vector3.left);
                end = Camera.main.ScreenToWorldPoint(new Vector3(0, weaponPos.y, screenPos.z));
            }
            
            if (shotTimer > delayPerShot)
            {
                if (Physics.Raycast(rayOrigin.origin, rayOrigin.direction, out rayHit, Mathf.Infinity, layerMask, QueryTriggerInteraction.Ignore))
                {
                    DrawLaserHit(rayOrigin.origin, rayHit.point);
                    //GameObject currentBullet = Instantiate(bullet, new Vector3(rayHit.point.x - 5, rayHit.point.y, rayHit.point.z), Quaternion.identity);
                }
                else
                    DrawLaserNoHit(rayOrigin.origin, end);

                //shotTimer = 0f;
            }
        }

        else
            shotTimer = 0f;
    }
}