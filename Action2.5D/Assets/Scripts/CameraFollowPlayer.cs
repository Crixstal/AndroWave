using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    public GameObject player;
    public float smoothTime = 0.1f;
    public float rotate = 5f;
    public float rotateSpeed = 0.1f;
    public Vector3 offset = new Vector3(0, 4, -15);

    [HideInInspector]
    public bool Yaxis = false;
    private Vector3 velocity = Vector3.zero;

    private void Start()
    {
        Vector3 targetPosition = player.transform.position + offset;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }

    // Update is called once per frame
    void Update()
    {
        Quaternion leftQuaternion = Quaternion.AngleAxis(-rotate, Vector3.up);
        Quaternion rightQuaternion = Quaternion.AngleAxis(rotate, Vector3.up);
        Vector3 targetPosition;

        if (!Yaxis)
        {
            float y = transform.position.y;
            targetPosition = player.transform.position + offset;
            transform.position = new Vector3(targetPosition.x, transform.position.y, transform.position.z);
        }

        else
        {
            //float x = transform.position.x;
            targetPosition = player.transform.position + offset;
            transform.position = new Vector3(transform.position.x, targetPosition.y, transform.position.z);
        }

        //transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);

        float horizontalInput = Input.GetAxis("HorizontalMovement");
        float verticalInput = Input.GetAxis("Rotate");

        if (horizontalInput < 0 && verticalInput == Mathf.Clamp(verticalInput, -0.4f, 0.4f))
            transform.rotation = Quaternion.Lerp(transform.rotation, leftQuaternion, rotateSpeed);

        else if (horizontalInput > 0 && verticalInput == Mathf.Clamp(verticalInput, -0.4f, 0.4f))
            transform.rotation = Quaternion.Lerp(transform.rotation, rightQuaternion, rotateSpeed);

        else
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.identity, rotateSpeed);
    }
}
