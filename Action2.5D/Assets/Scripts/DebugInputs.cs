using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DebugInputs : MonoBehaviour
{
    [SerializeField] private Player player = null;
    private float initialDelay;
    private float startTimer;

    void Start()
    {
        initialDelay = player.teleportationDelay;
    }

    void FixedUpdate()
    {
        MovePlayer();
        Teleport();
        Shoot();

        if (Input.GetKeyDown(KeyCode.R)) // reload scene
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        if (Input.GetKeyDown(KeyCode.T)) // decrease teleportation delay
        {
            if (player.teleportationDelay != 0.1f)
                player.teleportationDelay = 0.1f;

            else if (player.teleportationDelay == 0.1f)
                player.teleportationDelay = initialDelay;
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
        float playerRot = player.transform.rotation.eulerAngles.y;

        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            if (playerRot != Mathf.Clamp(playerRot, -1f, 1f)) // rotate right
                player.transform.Rotate(0f, 180f, 0f);

            else
                player.GetComponent<Rigidbody>().AddForce(Vector3.right * 70, ForceMode.Acceleration);
        }

        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            if (playerRot != Mathf.Clamp(playerRot, 179f, 181f)) // rotate left
                player.transform.Rotate(0f, 180f, 0f);

            else
                player.GetComponent<Rigidbody>().AddForce(Vector3.left * 70, ForceMode.Acceleration);
        }

        if (Input.GetKey(KeyCode.Space) && player.isGrounded)
        {
            player.GetComponent<Rigidbody>().AddForce(Vector3.up * 40, ForceMode.VelocityChange);
        }
    }

    void Teleport()
    {
        startTimer += Time.deltaTime;

        if (player.transform.position.z == player.posForeground)
            player.groundCheck = new Ray(new Vector3(player.transform.position.x, player.transform.position.y + 30, player.posBackground), Vector3.down);

        if (player.transform.position.z == player.posBackground)
            player.groundCheck = new Ray(new Vector3(player.transform.position.x, player.transform.position.y + 30, player.posForeground), Vector3.down);

        if (Input.GetKeyDown(KeyCode.W) && Physics.Raycast(player.groundCheck, out player.hit, Mathf.Infinity, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore))
        {
            if (player.transform.position.z == player.posForeground && startTimer >= player.teleportationDelay)
            {
                player.transform.position = new Vector3(player.hit.point.x, player.hit.point.y + player.transform.localScale.y, player.posBackground);
                startTimer = 0f;
            }

            if (player.transform.position.z == player.posBackground && startTimer >= player.teleportationDelay)
            {
                player.transform.position = new Vector3(player.hit.point.x, player.hit.point.y + player.transform.localScale.y, player.posForeground);
                startTimer = 0f;
            }
        }
    }

    void Shoot()
    {
        if (Input.GetKey(KeyCode.F))
        {
            player.transform.GetChild(player.currentWeapon).GetComponent<WeaponPlayer>().shootInput = 1f;

            if (Input.GetKey(KeyCode.DownArrow))
                player.transform.GetChild(player.currentWeapon).GetComponent<WeaponPlayer>().verticalInput = -1f;

            if (Input.GetKey(KeyCode.UpArrow))
                player.transform.GetChild(player.currentWeapon).GetComponent<WeaponPlayer>().verticalInput = 1f;
        }
    }
}
