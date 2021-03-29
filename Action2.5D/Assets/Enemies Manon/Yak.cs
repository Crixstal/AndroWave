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

    public float damage = 0f;

    private Rigidbody rb;
    private Vector3 relativePos = Vector3.zero;
    private float foreground = 0f;
    private float background = 0f;
    private Ray groundCheck;
    private RaycastHit hit;

    private bool secondRunDone = false;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();

        foreground = player.GetComponent<Player>().posForeground;
        background = player.GetComponent<Player>().posBackground;
    }

    private void FixedUpdate()
    {
        relativePos = player.transform.position - transform.position;

        if (life <= 0)
        {
            player.GetComponent<Player>().playerScore += score;
            Destroy(gameObject);
        }
    }

    private void SecondRun()
    {
        transform.Rotate(0f, 180f, 0f);

        if (transform.position.z == foreground)
            groundCheck = new Ray(new Vector3(transform.position.x, transform.position.y + 30, background), Vector3.down);

        if (transform.position.z == background)
            groundCheck = new Ray(new Vector3(transform.position.x, transform.position.y + 30, foreground), Vector3.down);

        if (Physics.Raycast(groundCheck, out hit, Mathf.Infinity, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore))
        {
            if (transform.position.z == foreground)
            {
                transform.position = new Vector3(player.transform.position.x - 50, hit.point.y + transform.localScale.y, background);
                rb.velocity = Vector3.zero;
                rb.AddForce(Vector3.right * speed, ForceMode.VelocityChange);
            }

            else if (transform.position.z == background)
            {
                transform.position = new Vector3(player.transform.position.x - 30, hit.point.y + transform.localScale.y, foreground);
                rb.velocity = Vector3.zero;
                rb.AddForce(Vector3.right * speed, ForceMode.VelocityChange);
            }
        }

        secondRunDone = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 9) // 9 = BulletPlayer
            life -= playerBullet.GetComponent<BulletPlayer>().damage;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 8) // 8 = Player
        {
            transform.Rotate(0f, 180f, 0f);
            rb.AddForce(Vector3.left * speed, ForceMode.VelocityChange);
            gameObject.layer = 13; // 13 = Ghost
        }
    }

    private void OnBecameInvisible()
    {
        if (relativePos.x > 0)
            SecondRun();

        gameObject.layer = 11; // 11 = Enemy

        if (secondRunDone && relativePos.x < 0)
            Destroy(gameObject);
    }
}
