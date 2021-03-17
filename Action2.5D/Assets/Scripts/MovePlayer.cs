using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayer : MonoBehaviour
{
    private Rigidbody body;
    public float speed = 20f;
    public float jump = 10f;

    public bool isGrounded;

    private float maxAirSpeed = 5f;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody>();
    }

    private void OnCollisionStay(Collision collision)
    {
        isGrounded = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        isGrounded = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.Space) && isGrounded && body.velocity.y < maxAirSpeed)
        {
            var jumpSpeed = transform.TransformDirection(0, 1, 0) * jump;
            body.AddForce(jumpSpeed, ForceMode.Impulse);
            isGrounded = false;
        }

        if (Input.GetKey(KeyCode.DownArrow) && !isGrounded && body.velocity.y > -maxAirSpeed)
        {
            var jumpSpeed = Physics.gravity * 2;
            body.AddForce(jumpSpeed, ForceMode.Impulse);
            isGrounded = false;
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            if (!isGrounded && -body.velocity.x > maxAirSpeed)
                return;

            var velocity = transform.TransformDirection(-1, 0, 0) * speed;
            body.AddForce(velocity, ForceMode.Acceleration);
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            if (!isGrounded && body.velocity.x > maxAirSpeed)
                return;

            var velocity = transform.TransformDirection(1, 0, 0) * speed;
            body.AddForce(velocity, ForceMode.Acceleration);
        }
    }
}
