using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShoot : MonoBehaviour
{
    [SerializeField]
    protected GameObject player = null;

    [SerializeField]
    protected Rigidbody projectile = null;

    [SerializeField]
    protected float detectionZone = 30f, bulletSpeed = 30f, shootDelay = 1.0f, gap = 1.0f;

    protected float timer = 0.0f;

    protected Quaternion leftQuaternion = Quaternion.AngleAxis(-90, Vector3.up);
    protected Quaternion rightQuaternion = Quaternion.AngleAxis(90, Vector3.up);
    protected Quaternion frontQuaternion = Quaternion.AngleAxis(180, Vector3.up);
    protected Quaternion backQuaternion = Quaternion.AngleAxis(0, Vector3.up);
    protected Quaternion projectileQuaternion = Quaternion.identity;

    void FixedUpdate()
    {
        if (Vector3.Distance(transform.position, player.transform.position) < detectionZone)
        {
            timer += Time.deltaTime;

            if (player.transform.position.z < transform.position.z - gap)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, frontQuaternion, 0.1f);
                projectileQuaternion = Quaternion.AngleAxis(90, Vector3.right);
            }

            else if (player.transform.position.z > transform.position.z + gap)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, backQuaternion, 0.1f);
                projectileQuaternion = Quaternion.AngleAxis(-90, Vector3.right);
            }

            else
            {
                if (player.transform.position.x < transform.position.x)
                {
                    transform.rotation = Quaternion.Lerp(transform.rotation, leftQuaternion, 0.1f);
                    projectileQuaternion = Quaternion.AngleAxis(90, Vector3.forward);
                }

                if (player.transform.position.x > transform.position.x)
                {
                    transform.rotation = Quaternion.Lerp(transform.rotation, rightQuaternion, 0.1f);
                    projectileQuaternion = Quaternion.AngleAxis(-90, Vector3.forward);
                }
            }

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
