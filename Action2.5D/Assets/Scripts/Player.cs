using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [SerializeField] private float horizontalInputSensitivity = 0.5f;

    [SerializeField] internal float generalLife = 0f;
    [SerializeField] internal float runLife = 0f;

    [SerializeField] private float speed = 0f;

    [SerializeField] internal float jump = 0f;
    [SerializeField] private float drag = 6f;
    [SerializeField] private float gravityUp = 0f;
    [SerializeField] private float gravityDown = 0f;

    [SerializeField] private float invincibilityDuration = 0f;
    [SerializeField] private float invincibilityDeltaTime = 0f;
    [SerializeField] private float respawnDelay = 0f;
    [SerializeField] private float respawnHeight = 0f;
    [SerializeField] private int losePoints = 0;

    [SerializeField] internal float waitBeforeTP = 0f;
    [SerializeField] internal float teleportationDelay = 0f;
    [SerializeField] internal float teleportationHeight = 0f;
    [SerializeField] internal float posForeground = 0f;
    [SerializeField] internal float posBackground = 0f;

    [SerializeField] private float delayBetweenDamage = 0f;
    [SerializeField] private float delayBeforeDamage = 0f;

    [SerializeField] private AudioSource damageSound = null;
    [SerializeField] private Color blinkingColor = Color.white; 

    internal Rigidbody rb = null;
    internal int playerScore = 0;
    internal int currentWeapon = 0;
    internal bool isGrounded;
    internal Ray groundCheck;
    internal RaycastHit hit;
    internal bool win = false;

    private Animator animator;
    private Material material = null;
    private float startTimer = 0f;
    private Color baseColor;
    private Camera cam;
    private bool isInvincible = false;
    private float constRunLife = 0f;

    private float constdelayBeforeDamage = 0f;
    private float timestamp = 0f;
    private bool isInVoid;
    private Vector3 respawnVoidPoint = Vector3.zero;

    internal bool isJumping;
    internal float jumpStartY;
    private Ray groundCheckJump;
    private RaycastHit hitDown;
    private float hitDownY;
    private float baseTeleportDelay;

    void Start()
    {      
        rb = GetComponent<Rigidbody>();
        animator = gameObject.GetComponentInParent<Animator>();

        groundCheck = new Ray(new Vector3(transform.position.x, transform.position.y + 30, posBackground), Vector3.down);
        groundCheckJump = new Ray(transform.position, Vector3.down);

        //Transform childTransform = transform.Find("SM_KIWI/SM_Body");
        //if (childTransform == null)
        //    Debug.Log("Can't find child");
        //
        //GameObject child = childTransform.gameObject;
        //material = child.GetComponent<Renderer>().material;
        //baseColor = material.GetColor("_BaseColor");

        cam = Camera.main;
        constRunLife = runLife;

        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(0).GetChild(i).gameObject.activeSelf == true)
            {
                currentWeapon = i;
                break;
            }
        }

        baseTeleportDelay = teleportationDelay;
        constdelayBeforeDamage = delayBeforeDamage;
    }

    void FixedUpdate()
    {
        rb.drag = drag;

        //if (material.GetColor("_BaseColor") != baseColor)
        //    material.SetColor("_BaseColor", baseColor);

        Move();
        Teleport();
        Jump();
        Gravity();

        if (runLife <= 0)
            StartCoroutine(Respawn());
    }

    private IEnumerator Respawn()
    {
        if (isInVoid)
            transform.position = respawnVoidPoint;
        else
            transform.position = new Vector3(transform.position.x, transform.position.y + respawnHeight, transform.position.z);

        isInVoid = false;

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
                material.SetColor("_BaseColor", blinkingColor);

            else
                material.SetColor("_BaseColor", baseColor);

            yield return new WaitForSeconds(invincibilityDeltaTime);
        }

        isInvincible = false;
    }

    private IEnumerator WaitBeforeTeleportation()
    {
        yield return new WaitForSeconds(waitBeforeTP);

        if (transform.position.z == posForeground)
            transform.position = new Vector3(hit.point.x, hit.point.y + transform.localScale.y, posBackground);

        else if (transform.position.z == posBackground)
            transform.position = new Vector3(hit.point.x, hit.point.y + transform.localScale.y, posForeground);
    }

    private void Move()
    {
        float horizontalInput = Input.GetAxis("HorizontalInput");
        float playerRot = transform.rotation.eulerAngles.y;

        if (playerRot != Mathf.Clamp(playerRot, -1f, 1f) && horizontalInput > 0) // rotate right
            transform.Rotate(0f, 180f, 0f);
        else if (playerRot == Mathf.Clamp(playerRot, -1f, 1f) && horizontalInput > horizontalInputSensitivity) // move right
        {
            animator.SetBool("idle", false);
            animator.SetBool("moving", true);
            rb.AddForce(speed * new Vector3(1, -hitDownY, 0), ForceMode.Acceleration);
        }


        if (playerRot != Mathf.Clamp(playerRot, 179f, 181f) && horizontalInput < 0) // rotate left
            transform.Rotate(0f, 180f, 0f);
        else if (playerRot == Mathf.Clamp(playerRot, 179f, 181f) && horizontalInput < -horizontalInputSensitivity) // move left
        {
            animator.SetBool("idle", false);
            animator.SetBool("moving", true);
            rb.AddForce(speed * new Vector3(-1, -hitDownY, 0), ForceMode.Acceleration);
        }

        if (horizontalInput == 0f)
        {
            animator.SetBool("moving", false);
            animator.SetBool("idle", true);
        }
    }

    private void Teleport()
    {
        startTimer += Time.deltaTime;

        if (transform.position.z == posForeground)
            groundCheck = new Ray(new Vector3(transform.position.x, transform.position.y + teleportationHeight, posBackground), Vector3.down);

        else if (transform.position.z == posBackground)
            groundCheck = new Ray(new Vector3(transform.position.x, transform.position.y + teleportationHeight, posForeground), Vector3.down);

        if (Input.GetButton("Teleport") && Physics.Raycast(groundCheck, out hit, Mathf.Infinity, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore))
        {
            cam.GetComponent<CameraFollowPlayer>().OnTeleport = cam.transform.position;
            isInvincible = true;
            animator.SetTrigger("teleport");

            if (startTimer >= teleportationDelay)
                StartCoroutine(WaitBeforeTeleportation());

            startTimer = 0f;
            isInvincible = false;
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

    private void Jump()
    {
        groundCheckJump = new Ray(new Vector3(transform.position.x, transform.position.y, transform.position.z), Vector3.down);
        LayerMask mask = LayerMask.GetMask("BulletPlayer");

        if (Physics.Raycast(groundCheckJump, out hitDown, 1.5f * transform.lossyScale.y, ~mask))
        {
            isGrounded = true;
            Jumping = false;

            if (rb.velocity.y < 0)
            {
                if (hitDown.normal.y < 1)
                    hitDownY = hitDown.normal.y;
                else
                    hitDownY = 0;
            }
        }

        else
            isGrounded = false;

        if (Input.GetButton("Jump") && isGrounded)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            jumpStartY = transform.position.y;
            rb.AddForce(Vector3.up * jump, ForceMode.VelocityChange);
            Jumping = true;

            animator.SetTrigger("jump");
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
            gameObject.transform.GetChild(0).GetChild(currentWeapon).gameObject.SetActive(false);
            gameObject.GetComponent<Player>().currentWeapon = weaponID;
        }
    }

    public void Hurt(float damage)
    {
        if (isInvincible)
            return;

        runLife -= damage;
        damageSound.Play();
        cam.GetComponent<ScreenShake>().StartShake();

        StartCoroutine(BecomeInvincible());
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 10) // 10 = Platform
            Physics.IgnoreCollision(GetComponent<Collider>(), other.GetComponent<Collider>(), true);

        if (other.gameObject.layer == 12) // 12 = BulletEnemy
        {
            if (isInvincible)
                return;

            runLife -= other.GetComponent<BulletEnemy>().damage;
            damageSound.Play();
            cam.GetComponent<ScreenShake>().StartShake();

            StartCoroutine(BecomeInvincible());
        }

        if (other.GetType() == typeof(CapsuleCollider) && other.gameObject.layer == 13) // 13 = Yak
        {
            if (isInvincible)
                return;

            runLife -= other.GetComponent<Yak>().damage;
            damageSound.Play();
            cam.GetComponent<ScreenShake>().StartShake();

            StartCoroutine(BecomeInvincible());
        }

        if (other.gameObject.layer == 14) // 14 = Grenade
        {
            if (isInvincible)
                return;

            runLife -= other.GetComponent<GrenadeEnemy>().damage;
            damageSound.Play();
            cam.GetComponent<ScreenShake>().StartShake();

            StartCoroutine(BecomeInvincible());
        }

        if (other.CompareTag("Trap"))
        {
            if (isInvincible)
                return;

            runLife -= other.GetComponent<Trap>().damage;
            damageSound.Play();
            cam.GetComponent<ScreenShake>().StartShake();

            StartCoroutine(BecomeInvincible());
        }

        if (other.CompareTag("Void"))
        {
            isInVoid = true;
            --runLife;
            respawnVoidPoint = other.transform.GetChild(0).position;
            transform.position = respawnVoidPoint;
        }

        if (other.CompareTag("Heart"))
        {
            runLife += 2;
            Destroy(other.gameObject);
        }

        if (other.CompareTag("MachineGun"))
        {
            GameObject weapon = transform.GetChild(0).GetChild(0).gameObject;

            ChangeWeapon(weapon, 0);

            Destroy(other.transform.parent.gameObject);
        }

        if (other.CompareTag("Shotgun"))
        {
            GameObject weapon = transform.GetChild(0).GetChild(1).gameObject;

            ChangeWeapon(weapon, 1);

            Destroy(other.transform.parent.gameObject);
        }

        if (other.CompareTag("Mitrapompe"))
        {
            GameObject weapon = transform.GetChild(0).GetChild(2).gameObject;

            ChangeWeapon(weapon, 2);

            Destroy(other.transform.parent.gameObject);
        }

        if (other.CompareTag("Finish"))
            win = true;
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Poison") || other.CompareTag("Tide"))
        {
            delayBeforeDamage -= Time.deltaTime;

            if (delayBeforeDamage <= 0f)
            {
                if (Time.time > timestamp)
                {
                    timestamp = Time.time + delayBetweenDamage;
                    --runLife;
                    cam.GetComponent<ScreenShake>().StartShake();
                }
            }
        }

        if (other.CompareTag("Lava") || other.CompareTag("Laser"))
        {
            if (Time.time > timestamp)
            {
                timestamp = Time.time + delayBetweenDamage;
                --runLife;
                cam.GetComponent<ScreenShake>().StartShake();
            }
        }

        if (other.CompareTag("Teleport"))
        {
            teleportationDelay = 1000;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 10) // 10 = Platform
            Physics.IgnoreCollision(GetComponent<Collider>(), other.GetComponent<Collider>(), false);

        if (other.CompareTag("Poison") || other.CompareTag("Tide"))
            delayBeforeDamage = constdelayBeforeDamage;

        if (other.CompareTag("Teleport"))
            teleportationDelay = baseTeleportDelay;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 14) // 14 = Grenade
        {
            if (isInvincible)
                return;

            runLife -= collision.gameObject.GetComponent<GrenadeEnemy>().damage;
            damageSound.Play();
            cam.GetComponent<ScreenShake>().StartShake();

            StartCoroutine(BecomeInvincible());
        }
    }
}
