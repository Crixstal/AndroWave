using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CanvasManagement : MonoBehaviour
{
    [SerializeField] private GameObject HUD = null;

    [SerializeField] private GameObject pause = null;
    [SerializeField] private GameObject pauseFirstButton = null;

    [SerializeField] private GameObject gameOver = null;
    [SerializeField] private GameObject gameOverFirstButton = null;

    [SerializeField] private GameObject win = null;
    [SerializeField] private GameObject winFirstButton = null;

    [SerializeField] private Player player = null;

    [SerializeField] private EventSystem eventSystem = null;
    
    private bool gameIsPaused;

    void Awake()
    {
        HUD.SetActive(true);
        pause.SetActive(false);
        gameOver.SetActive(false);
        win.SetActive(false);
    }

    void Update()
    {
        gameIsPaused = gameObject.GetComponent<GameManager>().isPaused;

        if (player.generalLife <= 0 && !gameOver.activeSelf)
        {
            eventSystem.SetSelectedGameObject(gameOverFirstButton);
            HUD.SetActive(false);
            gameOver.SetActive(true);
        }

        if (gameIsPaused && !pause.activeSelf)
        {
            eventSystem.SetSelectedGameObject(pauseFirstButton);
            pause.SetActive(true);
        }
        else if (!gameIsPaused)
            pause.SetActive(false);

        if (player.win && !win.activeSelf)
        {
            eventSystem.SetSelectedGameObject(winFirstButton);
            HUD.SetActive(false);
            win.SetActive(true);
            Time.timeScale = 0f;
        }
    }
}
