using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayer : MonoBehaviour
{
    private Rigidbody rb;
    public float speed = 10f;
    public float jump = 3f;

    public bool isGrounded;
    public bool isPaused = false;

    //private float maxAirSpeed = 5f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        Pause();
    }
    void FixedUpdate()
    {
        Move();
        Jump();
    }

    private void Move()
    {
        float horizontalInput = Input.GetAxis("HorizontalMovement");
        //float verticalInput = Input.GetAxis("Vertical");

        //transform.LookAt(transform.position + new Vector3(horizontalInput, 0, 0));

        Vector3 pos = transform.position;
        Vector3 newPos = pos + new Vector3(horizontalInput * speed * Time.deltaTime, 0, 0);
        transform.position = Vector3.Lerp(pos, newPos, 0.5f);
    }

    private void Jump()
    {
        if (Input.GetButton("Jump") && isGrounded)
            rb.AddForce(new Vector3(0.0f, 1.0f, 0.0f) * jump, ForceMode.Impulse);
    }

    private void Pause()
    {
        if (Input.GetButtonDown("Pause") && !isPaused)
        {
            Time.timeScale = 0;
            isPaused = true;
        }

        else if (Input.GetButtonDown("Pause") && isPaused)
        {
            Time.timeScale = 1;
            isPaused = false;
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