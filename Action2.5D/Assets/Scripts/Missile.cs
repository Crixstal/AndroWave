using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : Explosion
{
    public void OnCollisionEnter(Collision collision)
    {
        Explosive(gameObject);
    }
}
