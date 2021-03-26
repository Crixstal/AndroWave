using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    [SerializeField]
    private GameObject player = null;
    [SerializeField]
    private float smoothTime = 0.1f, cameraShift = 2f;

    [HideInInspector]
    public bool Yaxis = false;

    public Vector3 offset = new Vector3(0, 4, -15);
    private Vector3 velocity = Vector3.zero;
    private float foregroundZ;
    private float yPos;

    private void Start()
    {
        foregroundZ = player.GetComponent<Player>().posForeground;
        yPos = transform.position.y;
    }

    void Update()
    {
        Vector3 targetPosition;

        if (!Yaxis)
        {
            targetPosition = player.transform.position + offset;
            transform.position = new Vector3(targetPosition.x, transform.position.y, transform.position.z);
        }

        else
        {
            targetPosition = player.transform.position + offset;
            transform.position = new Vector3(transform.position.x, targetPosition.y, transform.position.z);
        }

        transform.position = new Vector3(targetPosition.x, yPos + offset.y, foregroundZ + offset.z);

        Vector3 leftShift = transform.position + new Vector3(cameraShift, 0, 0);
        Vector3 rightShift = transform.position + new Vector3(-cameraShift, 0, 0);
        Vector3 baseCamera = transform.position;
        float horizontalInput = Input.GetAxis("HorizontalMovement");
        float verticalInput = Input.GetAxis("Teleport");

        if (horizontalInput < 0 && verticalInput == Mathf.Clamp(verticalInput, -0.4f, 0.4f))
            targetPosition = rightShift;

        else if (horizontalInput > 0 && verticalInput == Mathf.Clamp(verticalInput, -0.4f, 0.4f))
            targetPosition = leftShift;

        else
            targetPosition = baseCamera;

        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }
}
