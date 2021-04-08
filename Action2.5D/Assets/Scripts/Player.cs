﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [SerializeField] private float horizontalInputSensitivity = 0.5f;
    [SerializeField] internal float generalLife = 0f;
    [SerializeField] internal float runLife = 0f;
    [SerializeField] private float speed = 0f;
    [SerializeField] private float drag = 6f;
    [SerializeField] private float jump = 0f;
    [SerializeField] private float gravityUp = 0f;
    [SerializeField] private float gravityDown = 0f;
    [SerializeField] private float invincibilityDuration = 0f;
    [SerializeField] private float invincibilityDeltaTime = 0f;
    [SerializeField] private float respawnDelay = 0f;
    [SerializeField] private float respawnHeight = 0f;
    [SerializeField] private AudioSource damageSound = null;
    [SerializeField] private int losePoints = 0;

    [SerializeField] internal float teleportationDelay = 0f;
    [SerializeField] internal float teleportationHeight = 0f;
    [SerializeField] internal float posForeground = 0f;
    [SerializeField] internal float posBackground = 0f;

    internal int playerScore;
    internal int currentWeapon = 0;

    internal bool isGrounded;
    internal bool win = false;
    internal Rigidbody rb;

    private Material material = null;
    private float startTimer;
    private Ray groundCheck;
    private RaycastHit hit;
    private Color baseColor;
    private Camera cam;
    private bool isInvincible = false;
    private float constRunLife;

    internal bool isJumping;
    internal float jumpStartY;
    private Ray groundCheckJump;
    private RaycastHit hitDown;
    private float hitDownY;

    [SerializeField] private float delayBetweenDamage = 0f;
    [SerializeField] private float delayBeforeDamage = 0f;
    private float constdelayBeforeDamage = 0f;
    private float timestamp = 0f;
    private bool isInVoid;
    private Vector3 respawnVoidPoint = Vector3.zero;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        groundCheck = new Ray(new Vector3(transform.position.x, transform.position.y + 30, posBackground), Vector3.down);
        cam = Camera.main;
        constRunLife = runLife;

        groundCheckJump = new Ray(transform.position, Vector3.down);
        material = GetComponent<Renderer>().material;
        baseColor = material.color;

        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).gameObject.activeSelf == true)
                currentWeapon = i;
        }

        constdelayBeforeDamage = delayBeforeDamage;
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
            if (material.color == baseColor)
                material.color = new Color(255, 255, 255);

            else
                material.color = baseColor;

            yield return new WaitForSeconds(invincibilityDeltaTime);
        }

        isInvincible = false;
    }

    private void Move()
    {
        float horizontalInput = Input.GetAxis("HorizontalInput");
        float playerRot = transform.rotation.eulerAngles.y;

        if (playerRot != Mathf.Clamp(playerRot, -1f, 1f) && horizontalInput > 0) // rotate right
            transform.Rotate(0f, 180f, 0f);
        else if (playerRot == Mathf.Clamp(playerRot, -1f, 1f) && horizontalInput > horizontalInputSensitivity) // move right
            rb.AddForce(speed * new Vector3(1, -hitDownY, 0), ForceMode.Acceleration);


        if (playerRot != Mathf.Clamp(playerRot, 179f, 181f) && horizontalInput < -0) // rotate left
            transform.Rotate(0f, 180f, 0f);
        else if (playerRot == Mathf.Clamp(playerRot, 179f, 181f) && horizontalInput < -horizontalInputSensitivity) // move left
            rb.AddForce(speed * new Vector3(-1, -hitDownY, 0), ForceMode.Acceleration);
    }

    private void Teleport()
    {
        startTimer += Time.deltaTime;

        if (transform.position.z == posForeground)
            groundCheck = new Ray(new Vector3(transform.position.x, transform.position.y + teleportationHeight, posBackground), Vector3.down);

        if (transform.position.z == posBackground)
            groundCheck = new Ray(new Vector3(transform.position.x, transform.position.y + teleportationHeight, posForeground), Vector3.down);

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

    private void Jump()
    {
        groundCheckJump = new Ray(new Vector3(transform.position.x, transform.position.y, transform.position.z), Vector3.down);

        if (Physics.Raycast(groundCheckJump, out hitDown, 1.1f))
        {
            isGrounded = true;
            isJumping = false;

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
            isJumping = true;
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            jumpStartY = transform.position.y;
            rb.AddForce(Vector3.up * jump, ForceMode.VelocityChange);
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

        if (other.gameObject.CompareTag("Trap"))
        {
            if (isInvincible)
                return;

            runLife -= other.GetComponent<Trap>().damage;
            damageSound.Play();
            cam.GetComponent<ScreenShake>().StartShake();

            StartCoroutine(BecomeInvincible());
        }

        if (other.gameObject.CompareTag("Void"))
        {
            isInVoid = true;
            --runLife;
            respawnVoidPoint = other.transform.GetChild(0).position;
            transform.position = respawnVoidPoint;
        }

        if (other.gameObject.CompareTag("Heart"))
        {
            runLife++;
            Destroy(other.gameObject);
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

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Poison") || other.gameObject.CompareTag("Tide"))
        {
            delayBeforeDamage -= Time.deltaTime;

            if (delayBeforeDamage <= 0f)
            {
                if (Time.time > timestamp)
                {
                    timestamp = Time.time + delayBetweenDamage;
                    --runLife;
                }
            }
        }

        if (other.gameObject.CompareTag("Lava") || other.gameObject.CompareTag("Laser"))
        {
            if (Time.time > timestamp)
            {
                timestamp = Time.time + delayBetweenDamage;
                --runLife;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 10) // 10 = Platform
            Physics.IgnoreCollision(GetComponent<Collider>(), other.GetComponent<Collider>(), false);

        if (other.gameObject.CompareTag("Poison") || other.gameObject.CompareTag("Tide"))
            delayBeforeDamage = constdelayBeforeDamage;
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