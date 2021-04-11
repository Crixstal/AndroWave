using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPlayer : MonoBehaviour
{
    [SerializeField] protected GameObject bullet = null;
    [SerializeField] protected float m_speed = 0f;
    [SerializeField] protected float m_damage = 0f;
    [SerializeField] protected float m_destructionDelay = 0f;
    [SerializeField] protected float delayPerShot = 0f;
    [SerializeField] protected AudioSource weaponSound = null;

    protected GameObject currentBullet;
    protected float shotTimer = 0f;
    protected float shootAngle = 0f;
    protected Vector3 inputs = Vector3.zero;
    protected Vector3 bulletSpawn = Vector3.zero;

    protected Player player = null;
    protected float playerRot = 0f;

    internal float shootInput = 0f;
    protected float horizontalInput = 0f;
    internal float verticalInput = 0f;

    void Start()
    {
        bullet.GetComponent<BulletPlayer>().speed = m_speed;
        bullet.GetComponent<BulletPlayer>().damage = m_damage;
        bullet.GetComponent<BulletPlayer>().destructionDelay = m_destructionDelay;

        player = gameObject.GetComponentInParent<Player>();
    }

    public void FixedUpdate()
    {
        playerRot = player.transform.rotation.eulerAngles.y;

        shootInput = Input.GetAxisRaw("Shoot");
        horizontalInput = Input.GetAxis("HorizontalInput");
        verticalInput = Input.GetAxis("VerticalInput");

        inputs = new Vector3(horizontalInput, verticalInput, 0f);

        bulletSpawn = transform.GetChild(0).position;

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
