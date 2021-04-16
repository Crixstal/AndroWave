using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    [SerializeField] internal float damage = 0f;
    [SerializeField] internal float blastDelay = 0f;
    [SerializeField] internal float destructionDelay = 0f;
    [SerializeField] private Material triggerMaterial = null;
    [SerializeField] private Material fireMaterial = null;
    [SerializeField] private AudioClip timerSound = null;
    [SerializeField] private AudioClip explosionSound = null;
    [SerializeField] private AudioSource audioSource = null;


    private IEnumerator Explosion()
    {
        audioSource.clip = timerSound;
        audioSource.Play();

        yield return new WaitForSeconds(blastDelay);

        GetComponent<MeshRenderer>().material = fireMaterial;
        gameObject.GetComponent<BoxCollider>().enabled = true;

        audioSource.clip = explosionSound;
        audioSource.Play();

        yield return new WaitForSeconds(destructionDelay);

        Destroy(gameObject);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 8) // 8 = Player
        {
            GetComponent<MeshRenderer>().material = triggerMaterial;
            StartCoroutine(Explosion());
        }
    }
}
