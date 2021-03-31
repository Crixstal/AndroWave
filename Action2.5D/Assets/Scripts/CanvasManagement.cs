using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasManagement : MonoBehaviour
{
    [SerializeField]    private GameObject HUD = null;
    [SerializeField]    private GameObject pause = null;
    [SerializeField]    private GameObject GameOver = null;
    [SerializeField]    private Player player = null;
    
    private bool playerIsAlive;
    private bool gameIsPaused;

    void Awake()
    {
        HUD.SetActive(true);
        pause.SetActive(false);
        GameOver.SetActive(false);
    }

    void Update()
    {
        playerIsAlive = player.IsAlive();
        gameIsPaused = gameObject.GetComponent<GameManager>().isPaused;

        if (!playerIsAlive)
        {
            HUD.SetActive(false);
            GameOver.SetActive(true);
            player.enabled = false;
        }

        if (gameIsPaused)
        {
            pause.SetActive(true);
            player.enabled = false;
        }

        else
        {
            pause.SetActive(false);
            player.enabled = true;
        }
    }
}
