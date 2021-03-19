using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeCamera : MonoBehaviour
{
    [SerializeField]
    private CameraFollowPlayer Camera = null;
    private Camera cam;

    private void Start()
    {
        cam = Camera.GetComponentInParent<Camera>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Front"))
        {
            transform.position += new Vector3(0f, 0f, 22f);
        }

        if (other.CompareTag("Back"))
        {
            transform.position -= new Vector3(0f, 0f, 22f);
        }

        if (other.CompareTag("Up") && !Camera.Yaxis)
        {
            Camera.Yaxis = true;
            return;
        }

        if (other.CompareTag("Side") && Camera.Yaxis)
        {
            Camera.Yaxis = false;
            cam.transform.position = new Vector3(cam.transform.position.x, other.transform.position.y + Camera.offset.y + 1, cam.transform.position.z);
            return;
        }
    }

}
