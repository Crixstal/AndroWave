using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    [SerializeField]
    private GameObject player = null;
    [SerializeField]
    private float smoothTime = 0.1f, rotate = 5f, rotateSpeed = 0.1f;

    [HideInInspector]
    public bool Yaxis = false;

    public Vector3 offset = new Vector3(0, 4, -15);
    private Vector3 velocity = Vector3.zero;
    private float foregroundZ;

    private void Start()
    {
        foregroundZ = player.GetComponent<Player>().posForeground;
    }

    void Update()
    {
        Quaternion startQuaternion = new Quaternion(transform.rotation.x, 0, 0, transform.rotation.w);
        Quaternion leftQuaternion = Quaternion.AngleAxis(-rotate, Vector3.up) * startQuaternion;
        Quaternion rightQuaternion = Quaternion.AngleAxis(rotate, Vector3.up) * startQuaternion;
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

        transform.position = Vector3.SmoothDamp(transform.position, new Vector3(targetPosition.x, targetPosition.y, foregroundZ + offset.z), ref velocity, smoothTime);

        float horizontalInput = Input.GetAxis("HorizontalMovement");
        float verticalInput = Input.GetAxis("Teleport");

        if (horizontalInput < 0 && verticalInput == Mathf.Clamp(verticalInput, -0.4f, 0.4f))
            transform.rotation = Quaternion.Lerp(transform.rotation, leftQuaternion, rotateSpeed);

        else if (horizontalInput > 0 && verticalInput == Mathf.Clamp(verticalInput, -0.4f, 0.4f))
            transform.rotation = Quaternion.Lerp(transform.rotation, rightQuaternion, rotateSpeed);

        else
            transform.rotation = Quaternion.Lerp(transform.rotation, startQuaternion, rotateSpeed);
    }
}
