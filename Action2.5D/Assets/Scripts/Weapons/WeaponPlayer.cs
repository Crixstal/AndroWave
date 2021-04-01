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
    protected float shootAngle = 0f;

    protected float shootInput = 0f;
    protected float horizontalInput = 0f;
    protected float isInXRange = 0f;
    protected float verticalInput = 0f;
    protected float isInYRange = 0f;
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

        shootInput = Input.GetAxisRaw("Shoot");
        horizontalInput = Input.GetAxis("HorizontalInput");
        verticalInput = Input.GetAxis("VerticalInput");
        isInYRange = Mathf.Clamp(verticalInput, -verticalInputSensitivity, verticalInputSensitivity);
        isInXRange = Mathf.Clamp(horizontalInput, -horizontalInputSensitivity, horizontalInputSensitivity);

        RotateWeapon();
        Shoot();
    }

    public void RotateWeapon()
    {
        // ---------- ROTATE RIGHT ----------
        if (horizontalInput > horizontalInputSensitivity)
        {
            transform.eulerAngles = new Vector3(0f, 0f, 0f); // RIGHT

            if (verticalInput > verticalInputSensitivity) // UP
                transform.eulerAngles = new Vector3(0f, 0f, 45f);

            else if (verticalInput < -verticalInputSensitivity) // DOWN
                transform.eulerAngles = new Vector3(0f, 0f, -45f);
        }

        // ---------- ROTATE LEFT ----------
        else if (horizontalInput < -horizontalInputSensitivity)
        {
            transform.eulerAngles = new Vector3(0f, 180f, 0f); // LEFT

            if (verticalInput > verticalInputSensitivity) // UP
                transform.eulerAngles = new Vector3(0f, 180f, 45f);

            else if (verticalInput < -verticalInputSensitivity) // DOWN
                transform.eulerAngles = new Vector3(0f, 180f, -45f);
        }
        
        // ---------- ROTATE UP & DOWN ----------
        else if (horizontalInput == isInXRange)
        {
            if (verticalInput > verticalInputSensitivity) // UP
                transform.eulerAngles = new Vector3(0f, 0f, 90f);

            else if (verticalInput < -verticalInputSensitivity) // DOWN
                transform.eulerAngles = new Vector3(0f, 0f, -90f);
        }

        if (horizontalInput == Mathf.Clamp(horizontalInput, -0.1f, 0.1f) && verticalInput == Mathf.Clamp(verticalInput, -0.1f, 0.1f))
            transform.eulerAngles = new Vector3(0f, 0f, 0f);

        shootAngle = transform.eulerAngles.z;
    }

    public virtual void Shoot() { }
}
