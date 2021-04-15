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
        if (other.gameObject.layer == 8 && gameObject.name == "Ascenseur") // 8 = Player
        {
            lift.SetTrigger("Ascenseur_Up");
            GetComponent<BoxCollider>().enabled = false;
        }

        if (other.gameObject.layer == 8 && gameObject.name == "Petit Ascenseur 1") // 8 = Player
        {
            lift.SetTrigger("PAscenseur1_Down");
            GetComponent<BoxCollider>().enabled = false;
        }

        if (other.gameObject.layer == 8 && gameObject.name == "Petit Ascenseur  2") // 8 = Player
        {
            lift.SetTrigger("PAscenseur2_Down");
            GetComponent<BoxCollider>().enabled = false;
        }

        if (other.gameObject.layer == 8 && gameObject.name == "Arène") // 8 = Player
        {
            lift.SetTrigger("Arene Trigger");
            GetComponent<BoxCollider>().enabled = false;
        }
    }
}