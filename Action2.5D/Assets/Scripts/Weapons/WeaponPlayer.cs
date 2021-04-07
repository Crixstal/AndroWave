using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPlayer : MonoBehaviour
{
    [SerializeField] protected GameObject bullet = null;
    [SerializeField] protected float m_speed = 0f;
    [SerializeField] protected float m_damage = 0f;
    [SerializeField] protected float m_destructionDelay = 0f;
    [SerializeField] protected float horizontalInputSensitivity = 0f;
    [SerializeField] protected float verticalInputSensitivity = 0f;
    [SerializeField] protected float delayPerShot = 0f;
    [SerializeField] protected AudioSource weaponSound = null;

    protected GameObject currentBullet;
    protected float shotTimer = 0f;

    protected Player player = null;
    protected float playerRot = 0f;
    protected Vector3 playerPos = Vector3.zero;
    protected float shootAngle = 0f;

    protected float shootInput = 0f;
    protected float horizontalInput = 0f;
    protected float isInXRange = 0f;
    protected float verticalInput = 0f;
    protected float isInYRange = 0f;
    protected Vector3 weaponPos = Vector3.zero;
    protected float weaponLength = 0f;
    protected Vector3 inputs = Vector3.zero;

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

        weaponPos = transform.position;

        shootInput = Input.GetAxisRaw("Shoot");
        horizontalInput = Input.GetAxis("HorizontalInput");
        verticalInput = Input.GetAxis("VerticalInput");
        isInYRange = Mathf.Clamp(verticalInput, -verticalInputSensitivity, verticalInputSensitivity);
        isInXRange = Mathf.Clamp(horizontalInput, -horizontalInputSensitivity, horizontalInputSensitivity);

        inputs = new Vector3(horizontalInput, verticalInput, 0f);

        RotateWeapon();
        Shoot();
    }

    public void RotateWeapon()
    {
        if (playerRot == Mathf.Clamp(playerRot, -1f, 1f)) // shoot right
        {
            shootAngle = Vector3.Angle(Vector3.right, inputs);

            if (verticalInput < 0f)
                shootAngle = -shootAngle;
        }

        else if (playerRot == Mathf.Clamp(playerRot, 179f, 181f)) // shoot left
        {
            shootAngle = Vector3.Angle(Vector3.left, inputs);

            if (verticalInput < 0f)
                shootAngle = -shootAngle;
        }

        transform.eulerAngles = new Vector3(0f, playerRot, shootAngle);
    }

    public virtual void Shoot() { }
}
