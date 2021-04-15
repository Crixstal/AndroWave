using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animation : MonoBehaviour
{
    private Animator lift;

    void Start()
    {
        lift = gameObject.GetComponent<Animator>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 8 && gameObject.name == "Ascenceur") // 8 = Player
        {
            lift.SetTrigger("Ascenceur_Up");
            GetComponent<BoxCollider>().enabled = false;
        }
    }
}