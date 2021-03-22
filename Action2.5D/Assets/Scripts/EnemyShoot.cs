using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShoot : MonoBehaviour
{
    [SerializeField]
    private GameObject player = null;

    [SerializeField]
    private Rigidbody projectile = null;

    [SerializeField]
    private float detectionZone = 50, bulletSpeed = 30, shootDelay = 1.0f;

    private float timer = 0.0f;
    private float gap = 1f;
    private Quaternion leftQuaternion = Quaternion.AngleAxis(-90, Vector3.up);
    private Quaternion rightQuaternion = Quaternion.AngleAxis(90, Vector3.up);
    private Quaternion frontQuaternion = Quaternion.AngleAxis(180, Vector3.up);
    private Quaternion backQuaternion = Quaternion.AngleAxis(0, Vector3.up);

    void FixedUpdate()
    {
        if (Vector3.Distance(transform.position, player.transform.position) < detectionZone)
        {
            timer += Time.deltaTime;

            if (player.transform.position.z < transform.position.z - gap)
                transform.rotation = Quaternion.Lerp(transform.rotation, frontQuaternion, 0.1f);

            else if (player.transform.position.z > transform.position.z + gap)
                transform.rotation = Quaternion.Lerp(transform.rotation, backQuaternion, 0.1f);

            else
            {
                if (player.transform.position.x < transform.position.x)
                    transform.rotation = Quaternion.Lerp(transform.rotation, leftQuaternion, 0.1f);

                if (player.transform.position.x > transform.position.x)
                    transform.rotation = Quaternion.Lerp(transform.rotation, rightQuaternion, 0.1f);
            }

            if (timer > shootDelay)
            {
                timer = timer - shootDelay;
                Rigidbody bullet = (Rigidbody)Instantiate(projectile, transform.position, transform.rotation);

                bullet.AddForce(transform.forward * bulletSpeed, ForceMode.Impulse);

                Destroy(bullet.gameObject, 1);
            }
        }
    }
}
