using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpWeapon : MonoBehaviour
{
    [SerializeField]
    private GameObject weapon = null, weaponOnTheGround = null;
    [SerializeField]
    private int weaponID = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Weapon"))
        {
            if (!weapon.activeSelf)
            {
                weapon.SetActive(true);
                gameObject.transform.GetChild(gameObject.GetComponent<Player>().currentWeapon);
                gameObject.GetComponent<Player>().currentWeapon = weaponID;
            }
            
            Destroy(weaponOnTheGround);
        }
    }
}
