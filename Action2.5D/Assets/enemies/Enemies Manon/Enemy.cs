using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] protected Player player = null;
    [SerializeField] protected GameObject weapon = null;
    [SerializeField] protected GameObject bullet = null;
    [SerializeField] protected int life = 0;
    [SerializeField] protected float m_speed = 0f;
    [SerializeField] protected int m_damage = 0;
    [SerializeField] protected float m_destructionDelay = 0f;
    [SerializeField] protected float delayPerShot = 0f;
    [SerializeField] protected Vector3 m_size = Vector3.zero;
    //[SerializeField] protected float bulletPerSalve = 0f;

    protected float shotTimer = 0f;

    protected Vector3 relativePos = Vector3.zero;
    protected Vector3 enemyPos = Vector3.zero;
    protected float enemyRot = 0f;

    protected Vector3 weaponPos = Vector3.zero;
    protected float weaponLength = 0f;
    protected float weaponRot = 0f;
    void Start()
    {
        bullet.GetComponent<BulletEnemy>().speed = m_speed;
        bullet.GetComponent<BulletEnemy>().damage = m_damage;
        bullet.GetComponent<BulletEnemy>().destructionDelay = m_destructionDelay;

        GetComponent<BoxCollider>().size = m_size;
    }

    public void FixedUpdate()
    {
        enemyPos = transform.position;
        enemyRot = transform.rotation.eulerAngles.y;

        weaponPos = weapon.transform.position;
        weaponLength = weapon.transform.localScale.x;
        weaponRot = weapon.transform.rotation.eulerAngles.z;

        relativePos = player.transform.position - transform.position;
    }

    public virtual void Shoot() { }

    public void Rotate()
    {
        if (relativePos.x < 0f && enemyRot == Mathf.Clamp(enemyRot, -1f, 1f)) // look left
            transform.Rotate(0f, 180f, 0f);

        else if (relativePos.x > 0f && enemyRot == Mathf.Clamp(enemyRot, 179f, 181f)) // look right
            transform.Rotate(0f, 180f, 0f);

        if (relativePos.y > transform.localScale.y && relativePos.z == Mathf.Clamp(relativePos.z, -1f, 1f) && weaponRot != Mathf.Clamp(weaponRot, 89f, 91f)) // rotate up
            weapon.transform.Rotate(0f, 0f, 90f);
        else if (relativePos.y < transform.localScale.y && relativePos.z == Mathf.Clamp(relativePos.z, -1f, 1f) && weaponRot == Mathf.Clamp(weaponRot, 89f, 91f))
            weapon.transform.Rotate(0f, 0f, -90f);

        if (relativePos.y < -transform.localScale.y && relativePos.z == Mathf.Clamp(relativePos.z, -1f, 1f) && weaponRot != Mathf.Clamp(weaponRot, 269f, 271f)) // rotate down
            weapon.transform.Rotate(0f, 0f, -90f);
        else if (relativePos.y > -transform.localScale.y && relativePos.z == Mathf.Clamp(relativePos.z, -1f, 1f) && weaponRot == Mathf.Clamp(weaponRot, 269f, 271f))
            weapon.transform.Rotate(0f, 0f, 90f);
    }

    public void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == 8) // 8 = Player
        {
            Shoot();
            Rotate();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 9) // 9 = BulletPlayer
        {
            --life;

            if (life <= 0)
                Destroy(gameObject);
        }
    }
}