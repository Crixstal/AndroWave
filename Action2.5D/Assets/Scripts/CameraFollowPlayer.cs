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

    private Vector3 velocity = Vector3.zero;

    // Update is called once per frame
    void Update()
    {
        Quaternion leftQuaternion = Quaternion.AngleAxis(-rotate, Vector3.up);
        Quaternion rightQuaternion = Quaternion.AngleAxis(rotate, Vector3.up);

        Vector3 targetPosition = player.transform.TransformPoint(offset) + new Vector3(1f, 0f, 1f);
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);

        if (Input.GetKey(KeyCode.LeftArrow))
            transform.rotation = Quaternion.Lerp(transform.rotation, leftQuaternion, rotateSpeed);

        else if (Input.GetKey(KeyCode.RightArrow))
            transform.rotation = Quaternion.Lerp(transform.rotation, rightQuaternion, rotateSpeed);

        else
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.identity, rotateSpeed);
    }
}
