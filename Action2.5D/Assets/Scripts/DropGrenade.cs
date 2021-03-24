using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropGrenade : EnemyShoot
{
    [SerializeField]
    private float moveSpeed = 10f;

    private Rigidbody body = null;
    private Vector3 dropPosition;

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
                body.AddForce(transform.right * -moveSpeed);
            }

            else if (player.transform.position.z > transform.position.z + gap)
            {
                body.AddForce(transform.right * moveSpeed);
            }

            else
                body.velocity = Vector3.zero;

            if (player.transform.position.x < transform.position.x)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, leftQuaternion, 1f);
                dropPosition = new Vector3(1 * transform.localScale.x, 0, 0);
            }

            if (player.transform.position.x > transform.position.x)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, rightQuaternion, 1f);
                dropPosition = new Vector3(-1 * transform.localScale.x, 0, 0);
            }

            if (timer > shootDelay)
            {
                timer = timer - shootDelay;
                Rigidbody bullet = (Rigidbody)Instantiate(projectile, transform.position + dropPosition, projectileQuaternion);

                //bullet.AddForce(transform.forward * bulletSpeed, ForceMode.Impulse);

                Destroy(bullet.gameObject, 1);
            }
        }
    }
}
