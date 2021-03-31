using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [SerializeField]    private float horizontalInputSensitivity = 0.5f;
    [SerializeField]    private float verticalInputSensitivity = 0.8f;
    [SerializeField]    private float life = 0;
    [SerializeField]    private float speed = 0f;
    [SerializeField]    private float drag = 6f;
    [SerializeField]    private float jump = 0f;
    [SerializeField]    private float gravityUp = 0f;
    [SerializeField]    private float gravityDown = 0f;
    [SerializeField]    private float invincibilityDuration = 1.5f;
    [SerializeField]    private float invincibilityDeltaTime = 0.15f;

    [SerializeField] private GameObject enemyBullet = null;
    [SerializeField] private GameObject enemyGrenade = null;
    [SerializeField] private Material material = null;

    public float teleportationDelay = 0f;
    public float posForeground = 0f;
    public float posBackground = 0f;
    public int playerScore;

    [HideInInspector]   public bool isGrounded;
    [HideInInspector]   public bool canShoot;

    private Rigidbody rb;
    private float startTimer;
    private Ray groundCheck;
    private RaycastHit hit;
    private Color baseColor;
    private Camera cam;
    private bool isInvincible = false;

    private IEnumerator BecomeInvincible()
    {
        Debug.Log("Player invincible");
        isInvincible = true;

        for (float i = 0; i < invincibilityDuration; i += invincibilityDeltaTime)
        {
            if (material.color == baseColor)
                material.color = new Color(255, 255, 255);

            else
                material.color = baseColor; 

            yield return new WaitForSeconds(invincibilityDeltaTime);
        }

        isInvincible = false;
        Debug.Log("Player not invincible");
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        groundCheck = new Ray(new Vector3(transform.position.x, transform.position.y + 30, posBackground), Vector3.down);
        baseColor = material.color;
        cam = Camera.main;
    }

    void FixedUpdate()
    {
        rb.drag = drag;
        if (material.color != baseColor)
            material.color = baseColor;

        Move();
        Teleport();
        Jump();
        Gravity();

        if (life <= 0)
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void Move()
    {
        float horizontalInput = Input.GetAxis("HorizontalInput");
        float verticalInput = Input.GetAxis("VerticalInput");
        float playerRot = transform.rotation.eulerAngles.y;

        if (verticalInput == Mathf.Clamp(verticalInput, -verticalInputSensitivity, verticalInputSensitivity))
        {
            if (playerRot != Mathf.Clamp(playerRot, -1f, 1f) && horizontalInput > horizontalInputSensitivity) // rotate right
                transform.Rotate(0f, 180f, 0f);
            else if (playerRot == Mathf.Clamp(playerRot, -1f, 1f) && horizontalInput > horizontalInputSensitivity) // move right
                rb.AddForce(Vector3.right * speed, ForceMode.Acceleration);


            if (playerRot != Mathf.Clamp(playerRot, 179f, 181f) && horizontalInput < -horizontalInputSensitivity) // rotate left
                transform.Rotate(0f, 180f, 0f);
            else if (playerRot == Mathf.Clamp(playerRot, 179f, 181f) && horizontalInput < -horizontalInputSensitivity) // move left
                rb.AddForce(Vector3.left * speed, ForceMode.Acceleration);
        }
    }

    private void Teleport()
    {
        startTimer += Time.deltaTime;

        if (transform.position.z == posForeground)
            groundCheck = new Ray(new Vector3(transform.position.x, transform.position.y + 30, posBackground), Vector3.down);

        if (transform.position.z == posBackground)
            groundCheck = new Ray(new Vector3(transform.position.x, transform.position.y + 30, posForeground), Vector3.down);

        if (Input.GetButton("Teleport") && isGrounded && Physics.Raycast(groundCheck, out hit, Mathf.Infinity, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore))
        {
            if (transform.position.z == posForeground && startTimer >= teleportationDelay)
            {
                transform.position = new Vector3(hit.point.x, hit.point.y + transform.localScale.y, posBackground);
                startTimer = 0f;
            }

            if (transform.position.z == posBackground && startTimer >= teleportationDelay)
            {
                transform.position = new Vector3(hit.point.x, hit.point.y + transform.localScale.y, posForeground);
                startTimer = 0f;
            }
        }
    }

    private void Jump()
    {
        if (Input.GetButton("Jump") && isGrounded)
            rb.AddForce(Vector3.up * jump, ForceMode.VelocityChange);
    }

    private void Gravity()
    {
        if (isGrounded)
            return;

        if (rb.velocity.y < 0)
            rb.AddForce(gravityDown * Physics.gravity, ForceMode.Acceleration);

        if (rb.velocity.y > 0)
            rb.AddForce(gravityUp * Physics.gravity, ForceMode.Acceleration);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 11) // 11 = Enemy
        {
            if (isInvincible)
                return;

            --life;
            cam.GetComponent<ScreenShake>().StartShake();

            StartCoroutine(BecomeInvincible());
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        isGrounded = true;
        canShoot = false;

        if (collision.gameObject.layer == 10) // 10 = Platform
            canShoot = true;

        if (collision.GetContact(0).normal.x == -1f || collision.GetContact(0).normal.x == 1f)
            isGrounded = false;
    }

    private void OnCollisionExit(Collision collision)
    {
        isGrounded = false;
        canShoot = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 10) // 10 = Platform
            Physics.IgnoreCollision(GetComponent<Collider>(), other.GetComponent<Collider>(), true);

        if (other.gameObject.layer == 12) // 12 = BulletEnemy
        {
            if (isInvincible)
                return;

            life -= enemyBullet.GetComponent<BulletEnemy>().damage;
            cam.GetComponent<ScreenShake>().StartShake();

            StartCoroutine(BecomeInvincible());
        }

        if (other.gameObject.layer == 14) // 14 = Grenade
        {
            if (isInvincible)
                return;

            life -= enemyGrenade.GetComponent<GrenadeEnemy>().damage;
            cam.GetComponent<ScreenShake>().StartShake();

            StartCoroutine(BecomeInvincible());
        }

        if (other.gameObject.CompareTag("Heart"))
        {
            Debug.Log("Heart");
            life++;
            Destroy(other.transform.parent.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 10) // 10 = Platform
            Physics.IgnoreCollision(GetComponent<Collider>(), other.GetComponent<Collider>(), false);
    }
}