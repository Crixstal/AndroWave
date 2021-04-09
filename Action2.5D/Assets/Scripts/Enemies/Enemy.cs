using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] protected Player player = null;
    [SerializeField] protected GameObject weapon = null;
    [SerializeField] protected GameObject bullet = null;
    [SerializeField] protected float life = 0f;
    [SerializeField] protected int score = 0;
    [SerializeField] protected float m_speed = 0f;
    [SerializeField] protected float m_damage = 0f;
    [SerializeField] protected float m_destructionDelay = 0f;
    [SerializeField] protected float delayPerShot = 0f;
    [SerializeField] protected float delayBeforeShoot = 0f;
    //[SerializeField] protected float bulletPerSalve = 0f;
    [SerializeField] protected GameObject playerBullet = null;
    [SerializeField] protected AudioSource damageSound = null;

    private DropHeart getHeart = null;
    private Material material;
    private Color baseColor;

    protected float shotTimer = 0f;

    protected Vector3 relativePos = Vector3.zero;
    protected Vector3 enemyPos = Vector3.zero;
    protected float enemyRot = 0f;
    protected float bulletRot = 0f;

    protected Vector3 weaponPos = Vector3.zero;
    protected float weaponLength = 0f;
    protected float weaponRot = 0f;
    private Color baseColor;
    private bool barrelHit = false;

    void Start()
    {
        getHeart = GetComponent<DropHeart>();
        bullet.GetComponent<BulletEnemy>().speed = m_speed;
        bullet.GetComponent<BulletEnemy>().damage = m_damage;
        bullet.GetComponent<BulletEnemy>().destructionDelay = m_destructionDelay;
        material = GetComponent<Renderer>().material;
        baseColor = material.color;
    }

    public void FixedUpdate()
    {
        if (material.color != baseColor)
            material.color = baseColor;

        enemyPos = transform.position;
        enemyRot = transform.rotation.eulerAngles.y;

        weaponPos = weapon.transform.position;
        weaponLength = weapon.transform.localScale.x;
        weaponRot = weapon.transform.rotation.eulerAngles.z;

        relativePos = player.transform.position - transform.position;

        if (life <= 0)
        {
            if (getHeart != null)
            {
                getHeart.ItemDrop();
            }

            player.GetComponent<Player>().playerScore += score;
            Destroy(gameObject);
        }
    }

    public virtual void Shoot() {}

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Barrel") && other.GetType() == typeof(BoxCollider) && !barrelHit)
        {
            life -= other.gameObject.GetComponent<Barrel>().damage;
            damageSound.Play();
            material.color = new Color(255, 255, 255);
            barrelHit = true;
        }
    }

    public void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == 8) // 8 = Player
        {
            delayBeforeShoot -= Time.deltaTime;

            if (delayBeforeShoot <= 0f)
                Shoot();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 9) // 9 = BulletPlayer
        {
            life -= playerBullet.GetComponent<BulletPlayer>().damage;
            damageSound.Play();
            material.color = new Color(255, 255, 255);
        }
    }
}
