using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayer : MonoBehaviour
{
    private Rigidbody rb;
    public float speed = 20f;
    public float jump = 5f;
    public float posForeground = 10.54f;
    public float posBackground = 32.54f;
    public float teleportationHeight = 6f;
    public GameObject bullet = null;

    private bool isGrounded;
    private bool jumpAxisIsInUse = false;
    private bool shootAxisIsInUse = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        Move();
        Rotate();
        Teleport();
        Jump();
        Shoot();
    }

    private void Move()
    {
        float horizontalInput = Input.GetAxis("HorizontalMovement");
        float verticalInput = Input.GetAxis("Rotate");

        if (verticalInput == Mathf.Clamp(verticalInput, -0.5f, 0.5f))
        {
            Vector3 pos = transform.position;
            Vector3 newPos = pos + new Vector3(horizontalInput * speed * Time.deltaTime, 0, 0);
            transform.position = Vector3.Lerp(pos, newPos, 0.5f);
        }
    }

    private void Rotate()
    {
        float verticalInput = Input.GetAxisRaw("Rotate");

        if (verticalInput == 1f)
        {
            if (jumpAxisIsInUse == false)
            {
                transform.Rotate(0f, -90f, 0f);
                jumpAxisIsInUse = true;
            }
        }

        else if (verticalInput == -1f)
        {
            if (jumpAxisIsInUse == false)
            {
                transform.Rotate(0f, 90f, 0f);
                jumpAxisIsInUse = true;
            }
        }

        else
            jumpAxisIsInUse = false;
    }

    private void Teleport()
    {
        if (Input.GetButtonDown("Teleport"))
        {
            if (transform.position.z == posForeground)
                transform.position = new Vector3(transform.position.x, teleportationHeight, posBackground);

            else
                transform.position = new Vector3(transform.position.x, teleportationHeight, posForeground);
        }
    }

    private void Jump()
    {
        if (Input.GetButton("Jump") && isGrounded)
            rb.AddForce(new Vector3(0.0f, 1.0f, 0.0f) * jump, ForceMode.Impulse);
    }

    private void Shoot()
    {
        float shootInput = Input.GetAxisRaw("Shoot");
        float horizontalInput = Input.GetAxis("HorizontalVisor");
        float verticalInput = Input.GetAxis("VerticalVisor");
        float rotation = transform.rotation.eulerAngles.y;

        if (shootInput == 1f)
        {
            if (shootAxisIsInUse == false)
            {
                if (rotation == Mathf.Clamp(rotation, 179f, 181f)) // shoot left
                    bullet.transform.position = new Vector3(transform.position.x - transform.localScale.x, transform.position.y, transform.position.z);

                else if (rotation == Mathf.Clamp(rotation, 269f, 271f)) // shoot background
                    bullet.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + transform.localScale.z);

                else if (rotation == Mathf.Clamp(rotation, 89f, 91f)) // shoot foreground
                    return;

                else // shoot right
                    bullet.transform.position = new Vector3(transform.position.x + transform.localScale.x, transform.position.y, transform.position.z);

                Instantiate(bullet);
                shootAxisIsInUse = true;
            }

            if (rotation == Mathf.Clamp(rotation, 179f, 181f)) // shoot left
                bullet.GetComponent<BulletPlayer>().direction = Vector3.Normalize(new Vector3(-transform.position.x, verticalInput * 10f, 0f));
            
            else if (rotation == Mathf.Clamp(rotation, 269f, 271f)) // shoot background
                bullet.GetComponent<BulletPlayer>().direction = Vector3.Normalize(new Vector3(horizontalInput * 10f, 0f, transform.position.z));

            else // shoot right
                bullet.GetComponent<BulletPlayer>().direction = Vector3.Normalize(new Vector3(transform.position.x, verticalInput * 10f, 0f));
        }

        else
            shootAxisIsInUse = false;
    }

    private void OnCollisionStay(Collision collision)
    {
        isGrounded = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        isGrounded = false;
    }
}