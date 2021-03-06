using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gorille : Enemy
{
    [SerializeField] private int bulletPerSalve = 0;
    [SerializeField] private float timeBetweenBullets = 0f;


    private IEnumerator Salve()
    {
        for (int i = 0; i < bulletPerSalve; ++i)
        {
            yield return new WaitForSeconds(timeBetweenBullets);

            audioSource.clip = apeShootSound;
            audioSource.Play();

            currentBullet = Instantiate(bullet, bulletSpawn, Quaternion.Euler(0f, transform.rotation.eulerAngles.y, 0f));
            currentBullet.GetComponent<BulletEnemy>().direction = Vector3.Normalize(relativePos);
        }
    }

    public override void Shoot()
    {
        if (Time.time > shotTimer)
        {
            shotTimer = Time.time + delayPerShot;

            StartCoroutine(Salve());
        }
    }
}
