using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayer : MonoBehaviour
{
    [SerializeField]
    private float speed = 20f;
    [SerializeField]
    private float jump = 10f;
    [SerializeField]
    private float posForeground = 10.54f;
    [SerializeField]
    private float posBackground = 32.54f;
    [SerializeField]
    private float teleportationHeight = 6f;
    [SerializeField]
    private float teleportationDelay = 4f;
    [SerializeField]
    private GameObject bullet = null;
    [SerializeField]
    private float angleShoot2D = 50f;
    [SerializeField]
    private float angleShoot3D = 5f;
    [SerializeField]
    private float horizontalInputSensitivity = 0.5f;
    [SerializeField]
    private float verticalInputSensitivity = 0.5f;

    private Rigidbody rb;
    private bool isGrounded;
    private bool verticalAxisIsInUse;
    private bool changeShootPlanIsInUse;
    private float startTimer = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        Move();
        Teleport();
        Jump();
        Visor2D();
        Visor3D();
        Shoot();
    }

    private void Move()
    {
        float horizontalInput = Input.GetAxis("HorizontalMovement");
        float verticalInput = Input.GetAxis("Teleport");

        if (verticalInput == Mathf.Clamp(verticalInput, -0.5f, 0.5f))
        {
            Vector3 pos = transform.position;
            Vector3 newPos = pos + new Vector3(horizontalInput * speed * Time.deltaTime, 0, 0);
            transform.position = Vector3.Lerp(pos, newPos, 0.5f);
        }
    }

    private void Teleport()
    {
        float verticalInput = Input.GetAxisRaw("Teleport");
        float rotation = transform.rotation.eulerAngles.y;

        startTimer += Time.deltaTime;

        if (startTimer >= teleportationDelay)
        {
            if (verticalInput == 1f)
            {
                // ----- TELEPORT BACKGROUND -----
                if (verticalAxisIsInUse == false && startTimer >= teleportationDelay)
                {
                    if (transform.position.z == posForeground)
                        transform.position = new Vector3(transform.position.x, teleportationHeight, posBackground);
                    else
                        return;

                    if (rotation == Mathf.Clamp(rotation, 269f, 271f)) // return in 2D shooting
                        transform.Rotate(0f, 90f, 0f);

                    verticalAxisIsInUse = true;

                    startTimer = 0f;
                }
            }

            else if (verticalInput == -1f)
            {
                // ----- TELEPORT FOREGROUND -----
                if (verticalAxisIsInUse == false && startTimer >= teleportationDelay)
                {
                    if (transform.position.z == posBackground)
                        transform.position = new Vector3(transform.position.x, teleportationHeight, posForeground);
                    else
                        return;

                    if (rotation == Mathf.Clamp(rotation, 89f, 91f)) // return in 2D shooting
                        transform.Rotate(0f, -90f, 0f);

                    verticalAxisIsInUse = true;

                    startTimer = 0f;
                }
            }

            else
                verticalAxisIsInUse = false;

        }

        bullet.transform.position = new Vector3(transform.position.x + transform.localScale.x, transform.position.y, transform.position.z); // bullet same plane as player
    }

    private void Jump()
    {
        if (Input.GetButton("Jump") && isGrounded)
        {
            rb.velocity = Vector3.zero;
            rb.AddForce(Vector3.up * jump, ForceMode.Impulse);
        }
    }

    private void Visor2D()
    {
        float horizontalInput = Input.GetAxis("HorizontalVisor");
        float verticalInput = Input.GetAxis("VerticalVisor");
        float rotation = transform.rotation.eulerAngles.y;
        float sensitivityRange = Mathf.Clamp(verticalInput, -verticalInputSensitivity, verticalInputSensitivity);

        if (rotation == Mathf.Clamp(rotation, 89f, 91f) || rotation == Mathf.Clamp(rotation, 269f, 271f)) // don't rotate when in 3D shooting
            return;

        // ----- SHOOT UP -----
        if (verticalInput > verticalInputSensitivity)
            bullet.transform.position = new Vector3(transform.position.x, transform.position.y + transform.localScale.y, transform.position.z);

        // ----- SHOOT DOWN -----
        if (verticalInput < -verticalInputSensitivity)
            bullet.transform.position = new Vector3(transform.position.x, transform.position.y - transform.localScale.y, transform.position.z);

        // ----- SHOOT RIGHT -----
        if (rotation != Mathf.Clamp(rotation, -1f, 1f) && verticalInput == sensitivityRange && horizontalInput > horizontalInputSensitivity) // if not already turned
            transform.Rotate(0f, 180f, 0f);
        if (rotation == Mathf.Clamp(rotation, -1f, 1f))
            bullet.transform.position = new Vector3(transform.position.x + transform.localScale.x, transform.position.y, transform.position.z);

        // ----- SHOOT LEFT -----
        if (rotation != Mathf.Clamp(rotation, 179f, 181f) && verticalInput == sensitivityRange && horizontalInput < -horizontalInputSensitivity) // if not already turned
            transform.Rotate(0f, 180f, 0f);
        if (rotation == Mathf.Clamp(rotation, 179f, 181f))
            bullet.transform.position = new Vector3(transform.position.x - transform.localScale.x, transform.position.y, transform.position.z);
    }

    private void Visor3D()
    {
        float shootInput = Input.GetAxisRaw("Shoot");
        float horizontalInput = Input.GetAxis("HorizontalVisor");
        float verticalInput = Input.GetAxis("VerticalVisor");
        float rotation = transform.rotation.eulerAngles.y;

        if (shootInput == -1f)
        {
            if (changeShootPlanIsInUse == false)
            {
                // ----- SHOOT BACKGROUND -----
                if (transform.position.z == posForeground)
                {
                    if (rotation == Mathf.Clamp(rotation, -1f, 1f))
                        transform.Rotate(0f, -90f, 0f);

                    else if (rotation == Mathf.Clamp(rotation, 179f, 181f))
                        transform.Rotate(0f, 90f, 0f);

                    else if (rotation == Mathf.Clamp(rotation, 269f, 271f))
                        transform.Rotate(0f, 90f, 0f);

                    bullet.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + transform.localScale.z);
                    
                    changeShootPlanIsInUse = true;
                }

                // ----- SHOOT FOREGROUND -----
                if (transform.position.z == posBackground)
                {
                    if (rotation == Mathf.Clamp(rotation, -1f, 1f))
                        transform.Rotate(0f, 90f, 0f);

                    else if (rotation == Mathf.Clamp(rotation, 179f, 181f))
                        transform.Rotate(0f, -90f, 0f);

                    else if (rotation == Mathf.Clamp(rotation, 89f, 91f))
                        transform.Rotate(0f, -90f, 0f);

                    bullet.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - transform.localScale.z);
                
                    changeShootPlanIsInUse = true;
                }
            }
        }

        else
            changeShootPlanIsInUse = false;
    }

    private void Shoot()
    {
        float shootInput = Input.GetAxisRaw("Shoot");
        float horizontalInput = Input.GetAxis("HorizontalVisor");
        float verticalInput = Input.GetAxis("VerticalVisor");
        float rotation = transform.rotation.eulerAngles.y;
        float sensitivityRange = Mathf.Clamp(verticalInput, -verticalInputSensitivity, verticalInputSensitivity);

        if (shootInput == 1f)
        {
            Instantiate(bullet);

            if (rotation != Mathf.Clamp(rotation, 89f, 91f) || rotation != Mathf.Clamp(rotation, 269f, 271f))
            {
                if (verticalInput > verticalInputSensitivity) // shoot up
                    bullet.GetComponent<BulletPlayer>().direction = Vector3.Normalize(new Vector3(0f, transform.position.y, 0f));

                else if (verticalInput < -verticalInputSensitivity && !isGrounded) // shoot down
                    bullet.GetComponent<BulletPlayer>().direction = Vector3.Normalize(new Vector3(0f, -transform.position.y, 0f));
            }
            
            if (rotation == Mathf.Clamp(rotation, -1f, 1f) && verticalInput == sensitivityRange) // shoot right
                bullet.GetComponent<BulletPlayer>().direction = Vector3.Normalize(new Vector3(transform.position.x, verticalInput * angleShoot2D, 0f));

            else if (rotation == Mathf.Clamp(rotation, 179f, 181f) && verticalInput == sensitivityRange) // shoot left
                bullet.GetComponent<BulletPlayer>().direction = Vector3.Normalize(new Vector3(-transform.position.x, verticalInput * angleShoot2D, 0f));

            if (rotation == Mathf.Clamp(rotation, 89f, 91f)) // shoot foreground
                bullet.GetComponent<BulletPlayer>().direction = Vector3.Normalize(new Vector3(horizontalInput * angleShoot3D, verticalInput * angleShoot3D, -transform.position.z));

            else if (rotation == Mathf.Clamp(rotation, 269f, 271f)) // shoot background
                bullet.GetComponent<BulletPlayer>().direction = Vector3.Normalize(new Vector3(horizontalInput * angleShoot3D, verticalInput * angleShoot3D, transform.position.z));
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        int Platform = 10; // see Input Manager

        isGrounded = true;

        if (collision.gameObject.layer == Platform)
            isGrounded = false;
    }

    private void OnCollisionExit(Collision collision)
    {
        isGrounded = false;
    }
}