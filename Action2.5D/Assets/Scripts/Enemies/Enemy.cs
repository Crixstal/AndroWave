using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] protected DropHeart getHeart = null;
    [SerializeField] protected Player player = null;
    [SerializeField] protected GameObject bullet = null;
    [SerializeField] protected GameObject grenade = null;
    [SerializeField] protected float life = 0f;
    [SerializeField] protected int score = 0;
    [SerializeField] protected float bulletSpeed = 0f;
    [SerializeField] protected float bulletDamage = 0f;
    [SerializeField] protected float bulletDestructionDelay = 0f;
    [SerializeField] protected float grenadeDamage = 0f;
    [SerializeField] protected float grenadeDestructionDelay = 0f;
    [SerializeField] protected float grenadeBlastDelay = 0f;
    [SerializeField] protected float delayPerShot = 0f;
    [SerializeField] protected float delayBeforeShoot = 0f;
    [SerializeField] protected AudioSource damageSound = null;
    [SerializeField] protected bool dropGrenade = false;
    [SerializeField] protected MeshRenderer meshRenderer;
    [SerializeField] private Color blinkingColor = new Color(255, 255, 255);

    protected GameObject currentBullet = null;
    protected GameObject currentGrenade = null;

    protected Material material;
    protected Color baseColor;

    protected float shotTimer = 0f;

    protected Vector3 relativePos = Vector3.zero;
    protected Vector3 playerPos = Vector3.zero;
    protected Vector3 enemyPos = Vector3.zero;

    protected bool barrelHit = false;
    protected Vector3 bulletSpawn = Vector3.zero;

    protected Camera cam;

    public void Start()
    {
        bullet.GetComponent<BulletEnemy>().speed = bulletSpeed;
        bullet.GetComponent<BulletEnemy>().damage = bulletDamage;
        bullet.GetComponent<BulletEnemy>().destructionDelay = bulletDestructionDelay;

        grenade.GetComponent<GrenadeEnemy>().damage = grenadeDamage;
        grenade.GetComponent<GrenadeEnemy>().destructionDelay = grenadeDestructionDelay;
        grenade.GetComponent<GrenadeEnemy>().blastDelay = grenadeBlastDelay;

        setRenderer();

        cam = Camera.main;
    }

    protected virtual void setRenderer()
    {
        material = meshRenderer.materials[2];
        baseColor = material.GetColor("_BaseColor");
    }

    public void FixedUpdate()
    {
        if (material.GetColor("_BaseColor") != baseColor)
            material.SetColor("_BaseColor", baseColor);

        enemyPos = transform.position;
        playerPos = player.transform.position;
        relativePos = playerPos - enemyPos;

        bulletSpawn = transform.GetChild(0).GetChild(0).position;

        if (life <= 0)
        {
            cam.GetComponent<ScreenShake>().StartShake();

            if (dropGrenade)
                currentGrenade = Instantiate(grenade, enemyPos, Quaternion.identity);

            if (getHeart != null)
                getHeart.ItemDrop();

            player.GetComponent<Player>().playerScore += score;

            Destroy(gameObject);
        }
    }

    public virtual void Shoot() {}

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Barrel") && other.GetType() == typeof(BoxCollider) && !barrelHit)
        {
            life -= other.gameObject.GetComponent<Barrel>().damage;
            damageSound.Play();
            material.SetColor("_BaseColor", blinkingColor);
            barrelHit = true;
        }
    }

    public virtual void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == 8) // 8 = Player
        {
            delayBeforeShoot -= Time.deltaTime;

            if (delayBeforeShoot <= 0f)
                Shoot();
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 9) // 9 = BulletPlayer
        {
            life -= collision.gameObject.GetComponent<BulletPlayer>().damage;
            damageSound.Play();
            material.SetColor("_BaseColor", blinkingColor);
        }
    }
}
