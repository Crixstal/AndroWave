using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YakBackCollider : MonoBehaviour
{
    internal bool backIsTriggered = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 8) // 8 = Player
            backIsTriggered = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 8) // 8 = Player
            backIsTriggered = false;
    }
}
