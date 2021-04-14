﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    [SerializeField] internal float damage = 0f;
    [SerializeField] internal float blastDelay = 0f;
    [SerializeField] internal float destructionDelay = 0f;

    private IEnumerator Explosion()
    {
        yield return new WaitForSeconds(blastDelay);

        gameObject.GetComponent<BoxCollider>().enabled = true;

        yield return new WaitForSeconds(destructionDelay);

        Destroy(gameObject);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 8) // 8 = Player
            StartCoroutine(Explosion());
    }
}