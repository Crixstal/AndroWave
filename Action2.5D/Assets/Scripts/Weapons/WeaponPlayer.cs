using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPlayer : MonoBehaviour
{
    [SerializeField]    protected GameObject bullet = null;
    [SerializeField]    protected float m_speed;
    [SerializeField]    protected float m_damage;
    [SerializeField]    protected float m_destructionDelay;
    [SerializeField]    protected float horizontalInputSensitivity;
    [SerializeField]    protected float verticalInputSensitivity;
    [SerializeField]    protected float delayPerShot;

    protected float shotTimer = 0f;

    protected Player player;
    protected float playerRot;
    protected Vector3 playerPos;
    protected float posForeground;
    protected float posBackground;
    protected bool canShoot;

    protected float shootInput;
    protected float horizontalInput;
    protected float verticalInput;
    protected float isInSensitivityRange;
    protected bool rotating = false;
    protected Vector3 weaponPos;
    protected float weaponLength;

    void Start()
    {
        player = gameObject.GetComponentInParent<Player>();

        bullet.GetComponent<BulletPlayer>().speed = m_speed;
        bullet.GetComponent<BulletPlayer>().damage = m_damage;
        bullet.GetComponent<BulletPlayer>().destructionDelay = m_destructionDelay;
        bullet.GetComponent<SphereCollider>().enabled = false;

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
