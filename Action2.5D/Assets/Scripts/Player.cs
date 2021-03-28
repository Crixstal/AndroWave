using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]    private float horizontalInputSensitivity = 0.5f;
    [SerializeField]    private float verticalInputSensitivity = 0.8f;
    [SerializeField]    private float speed = 0f;
    [SerializeField]    private float drag = 6f;
    [SerializeField]    private float jump = 0f;
    [SerializeField]    private float gravityUp = 0f;
    [SerializeField]    private float gravityDown = 0f;
    public float teleportationDelay = 0f;
    public float posForeground = 0f;
    public float posBackground = 0f;

    [HideInInspector]   public bool isGrounded;
    [HideInInspector]   public bool canShoot;

    private Rigidbody rb;
    private float startTimer;
    readonly private int Platform = 10; // see Input Manager
    private Ray groundCheck;
    private RaycastHit hit;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        groundCheck = new Ray(new Vector3(transform.position.x, transform.position.y + 30, posBackground), Vector3.down);
    }

    void FixedUpdate()
    {
        rb.drag = drag;

        Move();
        Teleport();
        Jump();
        Gravity();
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
        if (Input.GetButton("Teleport") && isGrounded && Physics.Raycast(groundCheck, out hit))
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

    private void OnCollisionStay(Collision collision)
    {
        isGrounded = true;
        canShoot = false;

        if (collision.gameObject.layer == Platform)
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
        if (other.gameObject.layer == Platform)
            Physics.IgnoreCollision(GetComponent<Collider>(), other.GetComponent<Collider>(), true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == Platform)
            Physics.IgnoreCollision(GetComponent<Collider>(), other.GetComponent<Collider>(), false);
    }
}