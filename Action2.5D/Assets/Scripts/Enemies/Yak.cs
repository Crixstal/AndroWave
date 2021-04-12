using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Yak : MonoBehaviour
{
    [SerializeField] private DropHeart getHeart = null;
    [SerializeField] private Player player = null;
    [SerializeField] private float life = 0f;
    [SerializeField] private int score = 0;
    [SerializeField] private float speed = 0f;
    public float damage = 0f;
    [SerializeField] private bool berserk = false;
    [SerializeField] private float rotateDelay = 0f;
    [SerializeField] private AudioSource damageSound = null;

    private Rigidbody rb;
    private Material material = null;
    private Color baseColor = Color.black;
    private bool barrelHit = false;
    private bool frontTriggered = false;
    private bool backTriggered = false;

    private Camera cam;



    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();

        cam = Camera.main;

        material = GetComponent<Renderer>().material;
        baseColor = material.color;

        Physics.IgnoreCollision(GetComponent<CapsuleCollider>(), player.GetComponent<Collider>(), true);

        if (berserk)
            transform.GetChild(1).GetComponent<Collider>().enabled = true; // enable back collider
    }

    private void FixedUpdate()
    {
        if (material.GetColor("_BaseColor") != baseColor)
            material.SetColor("_BaseColor", baseColor);

        if (life <= 0)
        {
            cam.GetComponent<ScreenShake>().StartShake();

            if (getHeart != null)
                getHeart.ItemDrop();

            player.GetComponent<Player>().playerScore += score;
            Destroy(gameObject);
        }
    }

    private IEnumerator TurnAround()
    {
        yield return new WaitForSeconds(rotateDelay);

        transform.Rotate(0f, 180f, 0f);
        rb.velocity = Vector3.zero;

        if (transform.rotation.eulerAngles.y == Mathf.Clamp(transform.rotation.eulerAngles.y, -1f, 1f))
            rb.AddForce(Vector3.right * speed, ForceMode.VelocityChange);
        else
            rb.AddForce(Vector3.left * speed, ForceMode.VelocityChange);
    }

    void OnTriggerEnter(Collider other)
    {
        frontTriggered = GetComponentInChildren<YakFrontCollider>().frontIsTriggered;
        backTriggered = GetComponentInChildren<YakBackCollider>().backIsTriggered;

        if (other.CompareTag("Barrel") && !barrelHit)
        {
            life -= other.GetComponent<Barrel>().damage;
            damageSound.Play();
            material.SetColor("_BaseColor", new Color(255, 255, 255));
            barrelHit = true;
        }

        if (backTriggered)
            StartCoroutine(TurnAround());

        if (frontTriggered)
        {
            if (transform.GetChild(0).GetComponent<Collider>().enabled == false)
                return;

            rb.AddForce(Vector3.left * speed, ForceMode.VelocityChange);
            transform.GetChild(0).GetComponent<Collider>().enabled = false; // disable front collider
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
            material.SetColor("_BaseColor", new Color(255, 255, 255));
        }
    }
}
