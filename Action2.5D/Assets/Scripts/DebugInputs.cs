using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DebugInputs : MonoBehaviour
{
    [SerializeField]
    private GameObject Player = null;

    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        //timer = Player.timer;
    }

    void MovePlayer()
    {
        if (Input.GetKey(KeyCode.RightArrow))
        {
            Player.GetComponent<Rigidbody>().AddForce(5f, 0f, 0f);
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            Player.GetComponent<Rigidbody>().AddForce(-5f, 0f, 0f);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Player.GetComponent<Rigidbody>().AddForce(0f, 10f, 0f, ForceMode.Impulse);
        }
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();

        if (Input.GetKeyDown(KeyCode.R))
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        if (Input.GetKeyDown(KeyCode.T))
        {
            //if (Player.timer != 0)
            //Player.timer = 0;
            //else
            //Player.timer = timer;
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            for (var i = 0; i < enemies.Length; i++)
                Destroy(enemies[i]);
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            for (var i = 0; i < enemies.Length; i++)
            {
                Vector2 screenPosition = Camera.main.WorldToScreenPoint(enemies[i].transform.position);
                if ((screenPosition.y < Screen.height && screenPosition.y > 0) && (screenPosition.x < Screen.width && screenPosition.x > 0))
                {
                    Destroy(enemies[i]);
                }
            }
        }
    }
}
