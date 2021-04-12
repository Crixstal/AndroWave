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
        if (other.CompareTag("Unlock Y") && !Camera.Yaxis)
        {
            Camera.Yaxis = true;
            return;
        }

        if (other.CompareTag("Lock Y") && Camera.Yaxis)
        {
            Camera.Yaxis = false;
            return;
        }

        if (other.CompareTag("Unlock X") && !Camera.Xaxis)
        {
            Camera.Xaxis = true;
            return;
        }

        if (other.CompareTag("Lock X") && Camera.Xaxis)
        {
            Camera.Xaxis = false;
            Camera.Xpos = cam.transform.position.x;
            return;
        }
    }
}
