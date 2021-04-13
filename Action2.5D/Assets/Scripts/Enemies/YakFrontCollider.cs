using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YakFrontCollider : MonoBehaviour
{
    public bool frontIsTriggered = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 8) // 8 = Player
            frontIsTriggered = true;
    }

    void OnTriggerExit(Collider other)
    {
        frontIsTriggered = false;
    }
}
