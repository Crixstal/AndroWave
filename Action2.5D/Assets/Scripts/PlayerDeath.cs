using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDeath : MonoBehaviour
{
    [SerializeField]
    private int life = 20;

    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.SetInt("life", life);
    }

    // Update is called once per frame
    void OnCollisionEnter()
    {
        life = PlayerPrefs.GetInt("life");
        Debug.Log(life);

        if (life <= 0)
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
