using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPlayer : MonoBehaviour
{
    [SerializeField]    protected GameObject bullet = null;
    [SerializeField]    protected Player player;
    [SerializeField]    protected float speed;
    [SerializeField]    protected float angleShoot2D;
    [SerializeField]    protected float angleShoot3D;
    [SerializeField]    protected float horizontalInputSensitivity;
    [SerializeField]    protected float verticalInputSensitivity;

    protected bool changeShootPlanIsInUse;
    protected float posForeground;
    protected float posBackground;
    protected bool isGrounded;

    void Start()
    {
        player = gameObject.GetComponentInParent<Player>();
    }

    void FixedUpdate()
    {
        posForeground = player.GetComponent<Player>().posForeground;
        posBackground = player.GetComponent<Player>().posBackground;
        isGrounded = player.GetComponent<Player>().isGrounded;

        bullet.GetComponent<BulletPlayer>().speed = speed;

        bullet.transform.position = new Vector3(player.transform.position.x + player.transform.localScale.x, player.transform.position.y, player.transform.position.z); // bullet same plane as player

        Visor2D();
        Visor3D();
        Shoot();
    }

    public virtual void Visor2D() {}

    public virtual void Visor3D() {}

    public virtual void Shoot() {}
}
