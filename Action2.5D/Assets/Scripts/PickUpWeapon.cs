using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpWeapon : MonoBehaviour
{
    [SerializeField]
    private GameObject weapon = null, weaponOnTheGround = null;
    [SerializeField]
    private int weaponID = 0;

/*
    void ChangeWeapon()
    {
        int i = 0;
        foreach (Transform weapon in transform)
        {
            if (i == selectedWeapon)
                weapon.gameObject.SetActive(true);
            else
                weapon.gameObject.SetActive(false);
            i++;
        }
    }
*/
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
