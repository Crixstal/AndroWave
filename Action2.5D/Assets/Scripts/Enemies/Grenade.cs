using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : Explosion
{
    [SerializeField]
    private float timer = 10f;
    private float count = 0f;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (count < timer)
            count += Time.deltaTime;

        else if (count >= timer)
            Explosive(gameObject);
    }
}
