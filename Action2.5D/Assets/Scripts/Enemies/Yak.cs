using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Yak : MonoBehaviour
{
    [SerializeField] private Player player = null;
    [SerializeField] private float life = 0f;
    [SerializeField] private int score = 0;
    [SerializeField] private float speed = 0f;
    [SerializeField] private bool berserk = false;
    [SerializeField] private float rotateDelay = 0f;
    [SerializeField] private AudioSource damageSound = null;

    private Material material = null;
    private Color baseColor = Color.black;

    public float damage = 0f;
    private Rigidbody rb;
    private bool barrelHit = false;

    private bool frontTriggered = false;
    private bool backTriggered = false;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();

        material = GetComponent<Renderer>().material;
        baseColor = material.color;

        Physics.IgnoreCollision(GetComponent<CapsuleCollider>(), player.GetComponent<Collider>(), true);

        if (berserk)
            transform.GetChild(1).GetComponent<Collider>().enabled = true;
    }

    private void FixedUpdate()
    {
        //if (material.color != baseColor)
        //    material.color = baseColor;

        if (life <= 0)
        {
            player.GetComponent<Player>().playerScore += score;
            Destroy(gameObject);
        }
    }

    private IEnumerator TurnAround()
    {
        yield return new WaitForSeconds(rotateDelay);

        //transform.eulerAngles = new Vector3(0f, 0f, 0f);

        transform.Rotate(0f, 180f, 0f);
        rb.velocity = Vector3.zero;
        rb.AddForce(Vector3.right * speed, ForceMode.VelocityChange);
    }

    void OnTriggerEnter(Collider other)
    {
        frontTriggered = GetComponentInChildren<YakFrontCollider>().frontIsTriggered;
        backTriggered = GetComponentInChildren<YakBackCollider>().backIsTriggered;

        if (frontTriggered)
        {
            StartCoroutine(TurnAround());
            //rb.AddForce(Vector3.left * speed, ForceMode.VelocityChange);
        }
            
        if (backTriggered)
        {
            StartCoroutine(TurnAround());
        }

            /*if (transform.rotation.y != Mathf.Clamp(transform.rotation.y, -1f, 1f)) // rotate right
                transform.Rotate(0f, 180f, 0f);
            else if (transform.rotation.y == Mathf.Clamp(transform.rotation.y, -1f, 1f)) // move right

            if (transform.rotation.y != Mathf.Clamp(transform.rotation.y, 179f, 181f)) // rotate left
                transform.Rotate(0f, 180f, 0f);
            else if (transform.rotation.y == Mathf.Clamp(transform.rotation.y, 179f, 181f)) // move left*/

        if (other.CompareTag("Barrel") && !barrelHit)
        {
            life -= other.GetComponent<Barrel>().damage;
            damageSound.Play();
            material.color = new Color(255, 255, 255);
            barrelHit = true;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
            Destroy(gameObject);

        if (collision.gameObject.layer == 9) // 9 = BulletPlayer
        {
            life -= collision.gameObject.GetComponent<BulletPlayer>().damage;
            damageSound.Play();
            material.color = new Color(255, 255, 255);
        }
    }
}
