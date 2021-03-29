using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TDenemie : MonoBehaviour
{
    [SerializeField] private GameObject bullet = null;
    [SerializeField] private Player player = null;
    [SerializeField] private float m_speed = 0f;
    [SerializeField] private float m_damage = 0f;
    [SerializeField] private float m_destructionDelay = 0f;
    [SerializeField] private float delayPerShot = 0f;
    [SerializeField] private float detectionRadius = 0f;

    readonly private int Player = 8; // see Input Manager

    //[SerializeField] private float bulletPerSalve = 0f;

    private float shotTimer = 0f;

    void Start()
    {
        bullet.GetComponent<TDbullet>().speed = m_speed;
        bullet.GetComponent<TDbullet>().damage = m_damage;
        bullet.GetComponent<TDbullet>().destructionDelay = m_destructionDelay;

        GetComponent<SphereCollider>().radius = detectionRadius;
        GetComponent<SphereCollider>().center = Vector3.zero;
    }

    private void Shoot()
    {
        if (Time.time > shotTimer)
        {
            shotTimer = Time.time + delayPerShot;

            //for (int i = 0; i < bulletPerSalve; ++i)
            //{
                GameObject currentBullet = Instantiate(bullet, transform.position, Quaternion.identity);
                currentBullet.GetComponent<TDbullet>().direction = Vector3.Normalize(player.transform.position - transform.position);
            //}
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == Player)
            Shoot();
    }
}
