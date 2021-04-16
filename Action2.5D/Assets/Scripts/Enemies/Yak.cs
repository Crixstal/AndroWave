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
    [SerializeField] private AudioClip hitSound = null;
    [SerializeField] private AudioClip runSound = null;
    [SerializeField] private AudioClip deathSound = null;
    [SerializeField] private AudioSource audioSource = null;
    [SerializeField] protected MeshRenderer meshRenderer;
    [SerializeField] private Color blinkingColor = Color.white;

    private Rigidbody rb;
    private Animator animator = null;
    private ParticleSystem deathParticle = null;
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

        animator = GetComponent<Animator>();
        deathParticle = GameObject.Find("Particles/SmokeyExplosion").GetComponent<ParticleSystem>();

        material = meshRenderer.materials[2];
        baseColor = material.GetColor("_BaseColor");

        Physics.IgnoreCollision(GetComponent<CapsuleCollider>(), player.GetComponent<Collider>(), true);

        if (berserk)
            transform.GetChild(1).GetComponent<Collider>().enabled = true; // enable back collider
    }

    private void FixedUpdate()
    {
        if (material.GetColor("_BaseColor") != baseColor)
            material.SetColor("_BaseColor", baseColor);

        if (audioSource.clip == hitSound)
            audioSource.volume = 0.1f;
        else
            audioSource.volume = 1f;

        if (life <= 0)
        {
            cam.GetComponent<ScreenShake>().StartShake();

            deathParticle.transform.position = transform.position;
            deathParticle.Play();

            if (getHeart != null)
                getHeart.ItemDrop();

            player.GetComponent<Player>().playerScore += score;

            if (!audioSource.isPlaying)
                audioSource.PlayOneShot(deathSound);

            Destroy(gameObject, deathSound.length);
        }
    }

    private IEnumerator TurnAround()
    {
        animator.SetTrigger("turnAround");

        yield return new WaitForSeconds(rotateDelay);

        transform.Rotate(0f, 180f, 0f);
        rb.velocity = Vector3.zero;

        if (transform.rotation.eulerAngles.y == Mathf.Clamp(transform.rotation.eulerAngles.y, -1f, 1f))
        {
            rb.AddForce(Vector3.right * speed, ForceMode.VelocityChange);
            audioSource.clip = runSound;
            audioSource.Play();
        }
        else
        {
            rb.AddForce(Vector3.left * speed, ForceMode.VelocityChange);
            audioSource.clip = runSound;
            audioSource.Play();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        frontTriggered = GetComponentInChildren<YakFrontCollider>().frontIsTriggered;
        backTriggered = GetComponentInChildren<YakBackCollider>().backIsTriggered;

        if (other.CompareTag("Barrel") && other.GetType() == typeof(BoxCollider) && !barrelHit)
        {
            life -= other.GetComponent<Barrel>().damage;

            audioSource.clip = hitSound;
            audioSource.Play();

            material.SetColor("_BaseColor", blinkingColor);
            barrelHit = true;
        }

        if (other.CompareTag("Lava"))
            life = 0f;

        if (backTriggered)
            StartCoroutine(TurnAround());

        if (frontTriggered)
        {
            if (transform.GetChild(0).GetComponent<Collider>().enabled == false)
                return;

            rb.AddForce(Vector3.left * speed, ForceMode.VelocityChange);
            transform.GetChild(0).GetComponent<Collider>().enabled = false; // disable front collider

            audioSource.clip = runSound;
            audioSource.Play();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
            Destroy(gameObject);

        if (collision.gameObject.layer == 9) // 9 = BulletPlayer
        {
            life -= collision.gameObject.GetComponent<BulletPlayer>().damage;

            audioSource.clip = hitSound;
            audioSource.volume = 0.5f;
            audioSource.Play();

            material.SetColor("_BaseColor", blinkingColor);
        }
    }
}
