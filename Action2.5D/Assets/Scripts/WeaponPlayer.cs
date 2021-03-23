using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPlayer : MonoBehaviour
{
    [SerializeField]    protected GameObject bullet = null;
    [SerializeField]    protected float m_speed;
    [SerializeField]    protected float m_damage;
    [SerializeField]    protected float m_destructionDelay;
    [SerializeField]    protected float angleShoot2D;
    [SerializeField]    protected float angleShoot3D;
    [SerializeField]    protected float horizontalInputSensitivity;
    [SerializeField]    protected float verticalInputSensitivity;
    [SerializeField]    protected float perShotDelay;
    
    protected Player player;
    protected float timestamp = 0f;
    protected bool changeShootPlanIsInUse;
    protected float posForeground;
    protected float posBackground;
    protected bool canShoot;

    void Start()
    {
        player = gameObject.GetComponentInParent<Player>();

        bullet.GetComponent<BulletPlayer>().speed = m_speed;
        bullet.GetComponent<BulletPlayer>().damage = m_damage;
        bullet.GetComponent<BulletPlayer>().destructionDelay = m_destructionDelay;
    }

    public virtual void FixedUpdate()
    {
        canShoot = player.GetComponent<Player>().canShoot;
        posForeground = player.GetComponent<Player>().posForeground;
        posBackground = player.GetComponent<Player>().posBackground;

        bullet.transform.position = new Vector3(player.transform.position.x + player.transform.localScale.x, player.transform.position.y, player.transform.position.z); // bullet same plane as player

        Visor2D();
        Visor3D();
        Shoot();
    }

    public virtual void Visor2D() {}

    public virtual void Visor3D() {}

    public virtual void Shoot() {}
}
