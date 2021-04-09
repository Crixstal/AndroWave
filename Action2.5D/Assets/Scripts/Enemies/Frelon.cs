using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frelon : MonoBehaviour
{
    [SerializeField] private Player player = null;
    [SerializeField] private GameObject bullet = null;
    [SerializeField] private float life = 0f;
    [SerializeField] private int score = 0;
    [SerializeField] protected float m_speed = 0f;
    [SerializeField] private float m_damage = 0f;
    [SerializeField] private float coneAngle = 0f;
    [SerializeField] private float m_destructionDelay = 0f;
    [SerializeField] protected float delayPerShot = 0f;
    [SerializeField] protected AudioSource damageSound = null;

    private Material material = null;
    private Color baseColor = Color.black;

    private GameObject currentBullet;
    private float shotTimer = 0f;

    private Vector3 enemyPos = Vector3.zero;
    private bool barrelHit = false;

    void Start()
    {
        bullet.GetComponent<BulletEnemy>().speed = m_speed;
        bullet.GetComponent<BulletEnemy>().damage = m_damage;
        bullet.GetComponent<BulletEnemy>().destructionDelay = m_destructionDelay;

        material = GetComponent<Renderer>().material;
        baseColor = material.color;
    }

    public void FixedUpdate()
    {
        //if (material.color != baseColor)
        //    material.color = baseColor;

        enemyPos = transform.position;

        if (life <= 0)
        {
            player.GetComponent<Player>().playerScore += score;
            Destroy(gameObject);
        }
    }

    private void Shoot()
    {
        float constConeAngle = coneAngle;

        if (Time.time > shotTimer)
        {
            shotTimer = Time.time + delayPerShot;

            for (int i = 0; i < 3; ++i)
            {
                currentBullet = Instantiate(bullet, new Vector3(enemyPos.x, enemyPos.y - transform.localScale.y, enemyPos.z), Quaternion.Euler(0f, 0f, coneAngle - 90f));
                currentBullet.GetComponent<BulletEnemy>().direction = Quaternion.AngleAxis(coneAngle, Vector3.forward) * Vector3.down;
                coneAngle -= constConeAngle;
            }
        }

        coneAngle = constConeAngle;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Barrel") && !barrelHit)
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
            Shoot();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 9) // 9 = BulletPlayer
        {
            life -= collision.gameObject.GetComponent<BulletPlayer>().damage;
            damageSound.Play();
            material.color = new Color(255, 255, 255);
        }

    }
}