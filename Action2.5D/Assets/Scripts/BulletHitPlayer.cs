using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHitPlayer : MonoBehaviour
{
    [SerializeField]
    private int damage = 1;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            int life = PlayerPrefs.GetInt("life");
            life -= damage;
            PlayerPrefs.SetInt("life", life);
        }
    }
}
