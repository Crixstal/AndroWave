using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DebugInputs : MonoBehaviour
{
    [SerializeField] private Player player = null;
    private float initialDelay;

    void Start()
    {
        initialDelay = player.GetComponent<Player>().teleportationDelay;
    }

    void Update()
    {
        MovePlayer();

        if (Input.GetKeyDown(KeyCode.R)) // reload scene
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        if (Input.GetKeyDown(KeyCode.T)) // decrease teleportation delay
        {
            if (player.GetComponent<Player>().teleportationDelay != 0.1f)
                player.GetComponent<Player>().teleportationDelay = 0.1f;

            else if (player.GetComponent<Player>().teleportationDelay == 0.1f)
                player.GetComponent<Player>().teleportationDelay = initialDelay;
        }

        if (Input.GetKeyDown(KeyCode.K)) // kill all enemies
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            for (var i = 0; i < enemies.Length; i++)
                Destroy(enemies[i]);
        }

        if (Input.GetKeyDown(KeyCode.C)) // kill visible enemies
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
    void MovePlayer()
    {
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            //player.GetComponent<Rigidbody>().AddForce(5f, 0f, 0f);
            player.GetComponent<Rigidbody>().AddForce(Vector3.right * 60, ForceMode.Acceleration);
        }

        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            //player.GetComponent<Rigidbody>().AddForce(-5f, 0f, 0f);
            player.GetComponent<Rigidbody>().AddForce(Vector3.left * 60, ForceMode.Acceleration);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            //player.GetComponent<Rigidbody>().AddForce(0f, 10f, 0f, ForceMode.Impulse);
            player.GetComponent<Rigidbody>().AddForce(Vector3.up * 40, ForceMode.VelocityChange);
        }
    }
}
