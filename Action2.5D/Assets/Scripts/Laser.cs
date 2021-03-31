using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    private float laserLength = 1;

    private LineRenderer lr;

    // Start is called before the first frame update
    void Start()
    {
        lr = GetComponent<LineRenderer>(); 
    }

    // Update is called once per frame
    void Update()
    {
        lr.SetPosition(0, Vector3.zero);
        RaycastHit hit;

        if (Physics.Raycast(transform.position, -transform.right, out hit))
        {
            if (hit.collider)
                lr.SetPosition(1, transform.InverseTransformPoint(hit.point));
        }

        else
        {
            lr.SetPosition(1, transform.right * laserLength);
        }
    }
}
