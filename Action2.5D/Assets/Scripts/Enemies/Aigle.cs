using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aigle : MonoBehaviour
{
    [SerializeField] private Player player = null;
    [SerializeField] private GameObject grenade = null;
    [SerializeField] private float height = 0f;
    [SerializeField] private float life = 0f;
    [SerializeField] private int score = 0;
    [SerializeField] private float speed = 0f;
    [SerializeField] private float m_damage = 0f;
    [SerializeField] private float m_blastDelay = 0f;
    [SerializeField] private float m_destructionDelay = 0f;
    [SerializeField] protected float delayPerShot = 0f;
    [SerializeField] protected GameObject playerBullet = null;
    [SerializeField] protected AudioSource damageSound = null;

    private float shotTimer = 0f;

    private Vector3 relativePos = Vector3.zero;
    private Vector3 playerPos = Vector3.zero;
    private Vector3 enemyPos = Vector3.zero;
    private float enemyRotY = 0f;
    private bool barrelHit = false;

    void Start()
    {
        grenade.GetComponent<GrenadeEnemy>().damage = m_damage;
        grenade.GetComponent<GrenadeEnemy>().blastDelay = m_blastDelay;
        grenade.GetComponent<GrenadeEnemy>().destructionDelay = m_destructionDelay;
    }

    public void FixedUpdate()
    {
        enemyPos = transform.position;
        enemyRotY = transform.rotation.eulerAngles.y;
        playerPos = player.transform.position;
        relativePos = player.transform.position - transform.position;

        if (life <= 0)
        {
            player.GetComponent<Player>().playerScore += score;
            Destroy(gameObject);
        }
    }

    private void Shoot()
    {
        if (Time.time > shotTimer)
        {
            shotTimer = Time.time + delayPerShot;

            Instantiate(grenade, new Vector3(enemyPos.x, enemyPos.y - transform.localScale.y, enemyPos.z), Quaternion.identity);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Barrel") && other.GetType() == typeof(BoxCollider) && !barrelHit)
        {
            life -= other.gameObject.GetComponent<Barrel>().damage;
            damageSound.Play();
            //material.color = new Color(255, 255, 255);
            barrelHit = true;
        }
    }

    public void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == 8) // 8 = Player
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(playerPos.x, playerPos.y + height, playerPos.z), speed * Time.deltaTime);
            
            if (enemyPos.z == playerPos.z)
                Shoot();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 9) // 9 = BulletPlayer
        {
            life -= playerBullet.GetComponent<BulletPlayer>().damage;
            damageSound.Play();
        }
    }
}