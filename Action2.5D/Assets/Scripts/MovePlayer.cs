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
    private float teleportationDelay = 6f;
    [SerializeField]
    private GameObject bullet = null;

    private Rigidbody rb;
    private bool isGrounded;
    private bool verticalAxisIsInUse = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        Move();
        Teleport();
        Jump();
        Shoot();
    }

    private void Move()
    {
        float horizontalInput = Input.GetAxis("HorizontalMovement");
        float verticalInput = Input.GetAxis("Teleport");

        Vector2 inputs = new Vector2(horizontalInput, verticalInput);
        Debug.Log(inputs);

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

        if (verticalInput == 1f)
        {
            if (verticalAxisIsInUse == false)
            {
                if (transform.position.z == posForeground)
                    transform.position = new Vector3(transform.position.x, teleportationHeight, posBackground);

                else 
                    return;

                verticalAxisIsInUse = true;
            }
        }

        else if (verticalInput == -1f)
        {
            if (verticalAxisIsInUse == false)
            {
                if (transform.position.z == posBackground)
                    transform.position = new Vector3(transform.position.x, teleportationHeight, posForeground);

                else
                    return;

                verticalAxisIsInUse = true;
            }
        }

        else
            verticalAxisIsInUse = false;
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

                if (rotation == Mathf.Clamp(rotation, 179f, 181f)) // shoot left
                    bullet.transform.position = new Vector3(transform.position.x - transform.localScale.x, transform.position.y, transform.position.z);

                else if (rotation == Mathf.Clamp(rotation, 269f, 271f)) // shoot background
                    bullet.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + transform.localScale.z);

                else if (rotation == Mathf.Clamp(rotation, 89f, 91f)) // shoot foreground
                    return;

                else // shoot right
                    bullet.transform.position = new Vector3(transform.position.x + transform.localScale.x, transform.position.y, transform.position.z);

                Instantiate(bullet);


            if (rotation == Mathf.Clamp(rotation, 179f, 181f)) // shoot left
                bullet.GetComponent<BulletPlayer>().direction = Vector3.Normalize(new Vector3(-transform.position.x, verticalInput * 10f, 0f));
            
            else if (rotation == Mathf.Clamp(rotation, 269f, 271f)) // shoot background
                bullet.GetComponent<BulletPlayer>().direction = Vector3.Normalize(new Vector3(horizontalInput * 10f, 0f, transform.position.z));

            else // shoot right
                bullet.GetComponent<BulletPlayer>().direction = Vector3.Normalize(new Vector3(transform.position.x, verticalInput * 10f, 0f));
        }

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