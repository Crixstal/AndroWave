using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpWeapon : MonoBehaviour
{
    [SerializeField]
    private GameObject Weapon = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Pick up Weapon");

            if (!Weapon.activeSelf)
            {
                Weapon.SetActive(true);
            }
            Destroy(gameObject);
        }
    }
}
