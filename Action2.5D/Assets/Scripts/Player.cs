using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]    private float speed = 20f;
    [SerializeField]    private float jump = 10f;
    [SerializeField]    private float teleportationHeight = 6f;
    [SerializeField]    private float teleportationDelay = 4f;

    public float posForeground = 10.54f;
    public float posBackground = 32.54f;
    public bool isGrounded;

    private Rigidbody rb;
    private bool verticalAxisIsInUse;
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
    }

    private void Jump()
    {
        if (Input.GetButton("Jump") && isGrounded)
        {
            rb.velocity = Vector3.zero;
            rb.AddForce(Vector3.up * jump, ForceMode.Impulse);
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