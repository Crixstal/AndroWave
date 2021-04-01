using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoints : MonoBehaviour
{
    private Renderer flag;
    private Material mat;

    void Awake()
    {
        flag = GetComponent<Renderer>();
        mat = GetComponent<Renderer>().material;
        mat.color = Color.yellow;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 8) // 8 = Player
        {
            other.GetComponent<Player>().checkpointPos = transform.position;
            mat.color = Color.cyan;
            flag.GetComponent<Renderer>().material = mat;
        }
    }
}