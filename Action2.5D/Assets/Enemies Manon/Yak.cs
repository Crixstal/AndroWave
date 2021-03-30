using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Yak : MonoBehaviour
{
    [SerializeField] private Player player = null;
    [SerializeField] private int life = 0;
    [SerializeField] private float speed = 0f;
    public int damage = 0;
    [SerializeField] private float m_radius = 0f;
    [SerializeField] private float m_height = 0f;

    private Rigidbody rb;
    private Vector3 relativePos = Vector3.zero;
    private float foreground = 0f;
    private float background = 0f;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();

        GetComponent<CapsuleCollider>().radius = m_radius;
        GetComponent<CapsuleCollider>().height = m_height; // length -> direction on X-axis
        foreground = player.GetComponent<Player>().posForeground;
        background = player.GetComponent<Player>().posBackground;
    }

    private void FixedUpdate()
    {
        relativePos = player.transform.position - transform.position;
    }

    private void FirstRun()
    {
        if (relativePos.x < 0)
            rb.AddForce(Vector3.left * speed, ForceMode.VelocityChange);

        if (relativePos.x > 0)
            rb.AddForce(Vector3.right * speed, ForceMode.VelocityChange);
    }

    private void SecondRun()
    {
        if (relativePos.x < 0)
            rb.AddForce(Vector3.left * speed, ForceMode.VelocityChange);

        if (relativePos.x > 0)
            rb.AddForce(Vector3.right * speed, ForceMode.VelocityChange);

        Debug.Log(relativePos);
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

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 8) // 8 = Player
        {
            FirstRun();
            gameObject.layer = 13; // 13 = Ghost
        }
    }

    private void OnBecameInvisible()
    {
        gameObject.layer = 11; // 11 = Enemy
        SecondRun();
    }
}
