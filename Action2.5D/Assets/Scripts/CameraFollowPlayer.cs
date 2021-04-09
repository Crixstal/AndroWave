using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    [SerializeField]
    private GameObject player = null;
    [SerializeField]
    private float smoothTime = 0.1f, cameraShift = 2f;

    [SerializeField] private float horizontalInputSensitivity = 0.5f;

    [HideInInspector]
    public bool Yaxis = true, Xaxis = true;
    [HideInInspector]
    public float Xpos;
    public Vector3 offset = new Vector3(0, 4, -15);

    private Rigidbody body;
    private Vector3 velocity = Vector3.zero;
    private float foregroundZ;
    private float xCameraShift = 0;
    private float epsilon = 0.5f;
    private float jumpForce;
    private float YPos;

    private void Start()
    {
        foregroundZ = player.GetComponent<Player>().posForeground;
        body = player.GetComponent<Rigidbody>();
        jumpForce = player.GetComponent<Player>().jump;
        YPos = transform.position.y;
    }

    void LateUpdate()
    {
        Vector3 targetPosition;
        bool isJumping = player.GetComponent<Player>().isJumping;

        if ((!isJumping && Yaxis && Xaxis) || (Yaxis && Xaxis &&
            (player.transform.position.y < player.GetComponent<Player>().jumpStartY)))
        {
            targetPosition = player.transform.position + offset;
            transform.position = new Vector3(targetPosition.x, targetPosition.y, foregroundZ + offset.z);
            //YPos = player.transform.position.y;
        }

        else if (!isJumping && Yaxis && !Xaxis)
        {
            targetPosition = player.transform.position + offset;
            transform.position = new Vector3(Xpos, targetPosition.y, foregroundZ + offset.z);
            //YPos = player.transform.position.y;
        }

        else if (Xaxis && (!Yaxis || isJumping))
        {
            targetPosition = player.transform.position + offset;
            transform.position = new Vector3(targetPosition.x, transform.position.y, foregroundZ + offset.z);
        }

        else
        {
            transform.position = new Vector3(Xpos, transform.position.y, foregroundZ + offset.z);
        }

        float horizontalInput = Input.GetAxis("HorizontalInput");

        if (horizontalInput > horizontalInputSensitivity || horizontalInput < horizontalInputSensitivity - epsilon)
        {
            xCameraShift = cameraShift * body.velocity.x;
        }

        targetPosition = transform.position + new Vector3(xCameraShift, 0, 0);
        /*
        if (YPos < player.transform.position.y)
        {
            Vector3 yPos = new Vector3(transform.position.x, YPos, transform.position.z);
            transform.position = Vector3.SmoothDamp(yPos, targetPosition, ref velocity, smoothTime);
            YPos++;
            Debug.Log("test");
        }

        else*/
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }
}