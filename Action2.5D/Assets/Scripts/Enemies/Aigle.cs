using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aigle : Enemy
{
    [SerializeField] private float height = 0f;
    [SerializeField] private float speed = 0f;

    protected override void setRenderer()
    {
        material = meshRenderer.materials[1];
        baseColor = material.GetColor("_BaseColor");
    }

    public override void Shoot()
    {
        if (Time.time > shotTimer)
        {
            animator.SetBool("Grenade", true);
            shotTimer = Time.time + delayPerShot;

            audioSource.clip = eagleShootSound;
            audioSource.Play();

            currentGrenade = Instantiate(grenade, bulletSpawn, Quaternion.identity);
        }    
    }

    public override void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == 8) // 8 = Player
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(playerPos.x, playerPos.y + height, playerPos.z), speed * Time.deltaTime);
            
            if (enemyPos.z == playerPos.z)
                Shoot();
            else
                animator.SetBool("Grenade", false);
        }
    }
}