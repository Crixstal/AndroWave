using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField]
    protected float AOE = 1f;
    [SerializeField]
    protected int damage = 5;

    private GameObject player = null;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public void Explosive(GameObject obj)
    {
        if (Vector3.Distance(obj.transform.position, player.transform.position) < AOE)
        {
            int life = PlayerPrefs.GetInt("life");
            life -= damage;
            PlayerPrefs.SetInt("life", life);
        }
        Destroy(obj.gameObject);
    }
}
