using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPlayer : MonoBehaviour
{
    [SerializeField]    protected GameObject bullet = null;
    [SerializeField]    protected float m_speed;
    [SerializeField]    protected float m_damage;
    [SerializeField]    protected float m_destructionDelay;
    //[SerializeField]    protected float angleShoot3D;
    [SerializeField]    protected float horizontalInputSensitivity;
    [SerializeField]    protected float verticalInputSensitivity;
    [SerializeField]    protected float perShotDelay;
    
    protected float timestamp = 0f;

    protected Player player;
    protected float playerRot;
    protected float posForeground;
    protected float posBackground;
    protected bool canShoot;

    protected Quaternion bulletRot;
    protected Vector3 inputs;
    protected float shootAngle;
    protected float shootInput;
    protected float horizontalInput;
    protected float verticalInput;
    protected float sensitivityRange;
    protected bool rightTriggerIsInUse;

    void Start()
    {
        player = gameObject.GetComponentInParent<Player>();

        bullet.GetComponent<BulletPlayer>().speed = m_speed;
        bullet.GetComponent<BulletPlayer>().damage = m_damage;
        bullet.GetComponent<BulletPlayer>().destructionDelay = m_destructionDelay;
    }

    public virtual void FixedUpdate()
    {
        playerRot = player.transform.rotation.eulerAngles.y;
        posForeground = player.GetComponent<Player>().posForeground;
        posBackground = player.GetComponent<Player>().posBackground;
        canShoot = player.GetComponent<Player>().canShoot;

        shootInput = Input.GetAxisRaw("Shoot");

        horizontalInput = Input.GetAxis("HorizontalVisor");
        verticalInput = Input.GetAxis("VerticalVisor");
        inputs = new Vector3(horizontalInput, verticalInput, 0f);
        sensitivityRange = Mathf.Clamp(verticalInput, -verticalInputSensitivity, verticalInputSensitivity);
        Debug.Log(inputs);

        Visor2D();
        //Visor3D();
        Shoot();
    }

    public virtual void Visor2D() {}

    //public virtual void Visor3D() {}

    public virtual void Shoot() {}
}
