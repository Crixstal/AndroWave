using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KamikazeRun : MonoBehaviour
{
    [SerializeField]
    private int damage = 5;
    [SerializeField]
    private float moveSpeed = 40, maxSpeed = 100;

    private Rigidbody body;

    // Start is called before the first frame update
    void Start()
    {
        body = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (body.velocity.x <= maxSpeed)
            body.AddForce(-Vector3.right * moveSpeed); //move to the left
    }

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
