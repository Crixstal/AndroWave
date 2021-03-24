using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropGrenade : EnemyShoot
{
    [SerializeField]
    private float moveSpeed = 10f;

    private Rigidbody body = null;

    private void Start()
    {
        body = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Vector3.Distance(transform.position, player.transform.position) < detectionZone)
        {
            timer += Time.deltaTime;

            if (player.transform.position.z < transform.position.z - gap)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, frontQuaternion, 0.1f);
                body.AddForce(transform.forward * moveSpeed);
            }

            else if (player.transform.position.z > transform.position.z + gap)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, backQuaternion, 0.1f);
                body.AddForce(transform.forward * moveSpeed);
            }

            else
                body.velocity = Vector3.zero;

            if (timer > shootDelay)
            {
                timer = timer - shootDelay;
                Rigidbody bullet = (Rigidbody)Instantiate(projectile, transform.position, projectileQuaternion);

                bullet.AddForce(transform.forward * bulletSpeed, ForceMode.Impulse);

                Destroy(bullet.gameObject, 1);
            }
        }
    }
}
