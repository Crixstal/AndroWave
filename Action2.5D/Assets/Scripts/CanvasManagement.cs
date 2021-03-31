using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasManagement : MonoBehaviour
{
    [SerializeField]    private GameObject HUD = null;
    [SerializeField]    private GameObject pause = null;
    [SerializeField]    private GameObject GameOver = null;
    [SerializeField]    private Player player = null;
    
    private GameManager gm = null;
    private bool playerIsAlive;

    void Awake()
    {
        HUD.SetActive(true);
        pause.SetActive(false);
        GameOver.SetActive(false);
    }

    void FixedUpdate()
    {
        playerIsAlive = player.IsAlive();

        if (!playerIsAlive)
        {
            HUD.SetActive(false);
            GameOver.SetActive(true);
            player.enabled = false;
        }

        if (gm.GetComponent<GameManager>().isPaused)
        {
            pause.SetActive(true);
            player.enabled = false;
        }

        else
        {
            pause.SetActive(true);
            player.enabled = true;
        }
    }
}
