using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPlayer : MonoBehaviour
{
    [SerializeField]    protected GameObject bullet = null;
    [SerializeField]    protected float m_speed = 0f;
    [SerializeField]    protected float m_damage = 0f;
    [SerializeField]    protected float m_destructionDelay = 0f;
    [SerializeField]    protected float horizontalInputSensitivity = 0f;
    [SerializeField]    protected float verticalInputSensitivity = 0f;
    [SerializeField]    protected float delayPerShot = 0f;

    protected GameObject currentBullet;
    protected float shotTimer = 0f;

    protected Player player = null;
    protected float playerRot = 0f;
    protected Vector3 playerPos = Vector3.zero;
    protected float posForeground = 0f;
    protected float posBackground = 0f;
    protected bool canShoot = true;
    protected float shootAngle = 0f;

    protected float shootInput = 0f;
    protected float horizontalInput = 0f;
    protected float isInXRange = 0f;
    protected float verticalInput = 0f;
    protected float isInSensitivityRange = 0f;
    protected bool rotating = false;
    protected Vector3 weaponPos = Vector3.zero;
    protected float weaponLength = 0f;

    void Start()
    {
        bullet.GetComponent<BulletPlayer>().speed = m_speed;
        bullet.GetComponent<BulletPlayer>().damage = m_damage;
        bullet.GetComponent<BulletPlayer>().destructionDelay = m_destructionDelay;

        player = gameObject.GetComponentInParent<Player>();

        weaponLength = transform.localScale.x;
    }

    public void FixedUpdate()
    {
        playerRot = player.transform.rotation.eulerAngles.y;
        playerPos = player.transform.position;
        posForeground = player.GetComponent<Player>().posForeground;
        posBackground = player.GetComponent<Player>().posBackground;

        weaponPos = transform.position;
        canShoot = player.GetComponent<Player>().canShoot;

        shootInput = Input.GetAxisRaw("Shoot");
        horizontalInput = Input.GetAxis("HorizontalInput");
        verticalInput = Input.GetAxis("VerticalInput");
        isInSensitivityRange = Mathf.Clamp(verticalInput, -verticalInputSensitivity, verticalInputSensitivity);
        isInXRange = Mathf.Clamp(horizontalInput, -horizontalInputSensitivity, horizontalInputSensitivity);

        RotateWeapon();
        Shoot();
    }

    public void RotateWeapon()
    {
        if (verticalInput > verticalInputSensitivity) // rotate up
        {
            if (!rotating)
            {
                transform.Rotate(0f, 0f, 90f);
                rotating = true;
            }
        }

        else if (verticalInput < -verticalInputSensitivity) // rotate down
        {
            if (!rotating)
            {
                transform.Rotate(0f, 0f, -90f);
                rotating = true;
            }
        }

        else if (verticalInput == Mathf.Clamp(verticalInput, -0.5f, 0.5f)) // go back to initial rotation
        {
            if (transform.rotation.eulerAngles.z == Mathf.Clamp(transform.rotation.eulerAngles.z, 89f, 91f)) // if oriented up
                transform.Rotate(0f, 0f, -90f);

            else if (transform.rotation.eulerAngles.z == Mathf.Clamp(transform.rotation.eulerAngles.z, 269f, 271f)) // if oriented down
                transform.Rotate(0f, 0f, 90f);
        }

        else
            rotating = false;
    }

    public virtual void Shoot() { }
}
