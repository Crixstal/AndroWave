using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KamikazeRun : MonoBehaviour
{
    [SerializeField]
    protected GameObject player = null;
    [SerializeField]
    private int damage = 5;
    [SerializeField]
    private float moveSpeed = 40, maxSpeed = 100, detectionZone = 30f;

    private Rigidbody body;

    // Start is called before the first frame update
    void Start()
    {
        body = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, player.transform.position) < detectionZone)
        {
            if (body.velocity.x <= maxSpeed)
                body.AddForce(-Vector3.right * moveSpeed); //move to the left
        }
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
