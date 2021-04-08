using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Yak : MonoBehaviour
{
    [SerializeField] private Player player = null;
    [SerializeField] private float life = 0f;
    [SerializeField] private int score = 0;
    [SerializeField] private float speed = 0f;
    [SerializeField] protected GameObject playerBullet = null;
    [SerializeField] protected AudioSource damageSound = null;

    public float damage = 0f;
    private Rigidbody rb;
    private bool barrelHit = false;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (life <= 0)
        {
            player.GetComponent<Player>().playerScore += score;
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
            Destroy(gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 8) // 8 = Player
        {
            transform.Rotate(0f, 180f, 0f);
            rb.AddForce(Vector3.left * speed, ForceMode.VelocityChange);
            gameObject.layer = 13; // 13 = Yak
        }

        if (other.gameObject.layer == 9) // 9 = BulletPlayer
        {
            life -= playerBullet.GetComponent<BulletPlayer>().damage;
            damageSound.Play();
        }

        if (other.gameObject.CompareTag("Barrel") && other.GetType() == typeof(BoxCollider) && !barrelHit)
        {
            life -= other.gameObject.GetComponent<Barrel>().damage;
            damageSound.Play();
            //material.color = new Color(255, 255, 255);
            barrelHit = true;
        }
    }
}
