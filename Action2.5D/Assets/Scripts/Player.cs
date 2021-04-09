using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [SerializeField] private float horizontalInputSensitivity = 0.5f;
    [SerializeField] private float speed = 0f;
    [SerializeField] private float drag = 6f;
    [SerializeField] private float gravityUp = 0f;
    [SerializeField] private float gravityDown = 0f;
    [SerializeField] private float invincibilityDuration = 0f;
    [SerializeField] private float invincibilityDeltaTime = 0f;
    [SerializeField] private float respawnDelay = 0f;
    [SerializeField] private float respawnHeight = 0f;
    [SerializeField] private GameObject enemyBullet = null;
    [SerializeField] private GameObject enemyGrenade = null;
    [SerializeField] private AudioSource damageSound = null;
    [SerializeField] private int losePoints = 0;

    [SerializeField] internal float generalLife = 0f;
    [SerializeField] internal float runLife = 0f;
    [SerializeField] internal float jump = 0f;
    [SerializeField] internal float teleportationDelay = 0f;
    [SerializeField] internal float posForeground = 0f;
    [SerializeField] internal float posBackground = 0f;
    [SerializeField] internal int playerScore;
    [SerializeField] internal int currentWeapon;

    internal bool isGrounded;
    internal bool isJumping;
    internal float jumpStartY;
    internal Ray groundCheck;
    internal RaycastHit hit;
    internal bool win = false;

    private Material material;
    private Rigidbody rb;
    private Ray groundCheckJump;
    private RaycastHit hitDown;
    private Color baseColor;
    private Camera cam;
    private bool isInvincible = false;
    private float startTimer;
    private float constRunLife;
    private float hitDownY;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        groundCheck = new Ray(new Vector3(transform.position.x, transform.position.y + 30, posBackground), Vector3.down);
        groundCheckJump = new Ray(transform.position, Vector3.down);
        material = GetComponent<Renderer>().material;
        baseColor = material.GetColor("_BaseColor");
        cam = Camera.main;
        constRunLife = runLife;

        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).gameObject.activeSelf == true)
                currentWeapon = i;
        }
    }

    void FixedUpdate()
    {
        rb.drag = drag;
        if (material.GetColor("_BaseColor") != baseColor)
            material.SetColor("_BaseColor", baseColor);

        Move();
        Teleport();
        Jump();
        Gravity();

        if (runLife <= 0)
            StartCoroutine(Respawn());
    }

    private IEnumerator Respawn()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y + respawnHeight, transform.position.z);
        Time.timeScale = 0f;
        --generalLife;
        runLife = constRunLife;
        playerScore -= losePoints;

        if (generalLife <= 0)
            yield break;

        yield return new WaitForSecondsRealtime(respawnDelay);

        Time.timeScale = 1f;
    }

    private IEnumerator BecomeInvincible()
    {
        isInvincible = true;

        for (float i = 0; i < invincibilityDuration; i += invincibilityDeltaTime)
        {
            if (material.GetColor("_BaseColor") == baseColor)
                material.SetColor("_BaseColor", new Color(255, 255, 255));

            else
                material.SetColor("_BaseColor", baseColor);

            yield return new WaitForSeconds(invincibilityDeltaTime);
        }

        isInvincible = false;
    }

    private void Move()
    {
        float horizontalInput = Input.GetAxis("HorizontalInput");
        float playerRot = transform.rotation.eulerAngles.y;

        if (playerRot != Mathf.Clamp(playerRot, -1f, 1f) && horizontalInput > horizontalInputSensitivity) // rotate right
            transform.Rotate(0f, 180f, 0f);
        else if (playerRot == Mathf.Clamp(playerRot, -1f, 1f) && horizontalInput > horizontalInputSensitivity) // move right
            rb.AddForce(speed * new Vector3(1, -hitDownY, 0), ForceMode.Acceleration);


        if (playerRot != Mathf.Clamp(playerRot, 179f, 181f) && horizontalInput < -horizontalInputSensitivity) // rotate left
            transform.Rotate(0f, 180f, 0f);
        else if (playerRot == Mathf.Clamp(playerRot, 179f, 181f) && horizontalInput < -horizontalInputSensitivity) // move left
            rb.AddForce(speed * new Vector3(-1, -hitDownY, 0), ForceMode.Acceleration);
    }

    private void Teleport()
    {
        startTimer += Time.deltaTime;

        if (transform.position.z == posForeground)
            groundCheck = new Ray(new Vector3(transform.position.x, transform.position.y + 30, posBackground), Vector3.down);

        if (transform.position.z == posBackground)
            groundCheck = new Ray(new Vector3(transform.position.x, transform.position.y + 30, posForeground), Vector3.down);

        if (Input.GetButton("Teleport") && Physics.Raycast(groundCheck, out hit, Mathf.Infinity, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore))
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

    public bool Jumping
    {
        get
        {
            return isJumping;
        }
        set
        {
            if (isJumping != value)
            {
                isJumping = value;
                cam.GetComponent<CameraFollowPlayer>().playerJumping = value;
            }
        }
    }

    private bool Grounded()
    {
        groundCheckJump = new Ray(new Vector3(transform.position.x, transform.position.y, transform.position.z), Vector3.down);
        LayerMask mask = LayerMask.GetMask("BulletPlayer");

        if (Physics.Raycast(groundCheckJump, out hitDown, 1.1f, ~mask))
        {
            Debug.DrawLine(groundCheckJump.origin, hitDown.point);
            Jumping = false;

            if (rb.velocity.y < 0)
            {
                if (hitDown.normal.y < 1)
                    hitDownY = hitDown.normal.y;
                else
                    hitDownY = 0;
            }
            return true;
        }

        return false;
    }

    private void Jump()
    {
        isGrounded = Grounded();

        if (Input.GetButton("Jump") && isGrounded)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            jumpStartY = transform.position.y;
            rb.AddForce(Vector3.up * jump, ForceMode.VelocityChange);
            Jumping = true;
        }
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

    private void ChangeWeapon(GameObject weapon, int weaponID)
    {
        if (!weapon.activeSelf)
        {
            weapon.SetActive(true);
            gameObject.transform.GetChild(currentWeapon).gameObject.SetActive(false);
            gameObject.GetComponent<Player>().currentWeapon = weaponID;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 14) // 14 = Grenade
        {
            if (isInvincible)
                return;

            runLife -= enemyGrenade.GetComponent<GrenadeEnemy>().damage;
            damageSound.Play();
            cam.GetComponent<ScreenShake>().StartShake();

            StartCoroutine(BecomeInvincible());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 10) // 10 = Platform
            Physics.IgnoreCollision(GetComponent<Collider>(), other.GetComponent<Collider>(), true);

        if (other.gameObject.layer == 12) // 12 = BulletEnemy
        {
            if (isInvincible)
                return;

            runLife -= enemyBullet.GetComponent<BulletEnemy>().damage;
            damageSound.Play();
            cam.GetComponent<ScreenShake>().StartShake();

            StartCoroutine(BecomeInvincible());
        }

        if (other.GetType() == typeof(CapsuleCollider) && other.gameObject.layer == 13) // 13 = Yak
        {
            if (isInvincible)
                return;

            runLife -= other.gameObject.GetComponent<Yak>().damage;
            damageSound.Play();
            cam.GetComponent<ScreenShake>().StartShake();

            StartCoroutine(BecomeInvincible());
        }

        if (other.gameObject.layer == 14) // 14 = Grenade
        {
            if (isInvincible)
                return;

            runLife -= enemyGrenade.GetComponent<GrenadeEnemy>().damage;
            damageSound.Play();
            cam.GetComponent<ScreenShake>().StartShake();

            StartCoroutine(BecomeInvincible());
        }

        if (other.gameObject.CompareTag("Heart"))
        {
            runLife++;
            Destroy(other.transform.parent.gameObject);
        }

        if (other.gameObject.CompareTag("MachineGun"))
        {
            GameObject weapon = transform.GetChild(0).gameObject;

            ChangeWeapon(weapon, 0);

            Destroy(other.transform.parent.gameObject);
        }

        if (other.gameObject.CompareTag("Shotgun"))
        {
            GameObject weapon = transform.GetChild(1).gameObject;

            ChangeWeapon(weapon, 1);

            Destroy(other.transform.parent.gameObject);
        }

        if (other.CompareTag("Finish"))
            win = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 10) // 10 = Platform
            Physics.IgnoreCollision(GetComponent<Collider>(), other.GetComponent<Collider>(), false);
    }
}
