using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [HideInInspector]
    public bool isPaused = false;

    void Update()
    {
        Pause();
        Quit();
    }

    private void Pause()
    {
        if (Input.GetButtonDown("Pause") && !isPaused)
        {
            Time.timeScale = 0;
            isPaused = true;
        }

        else if (Input.GetButtonDown("Pause") && isPaused)
        {
            Time.timeScale = 1;
            isPaused = false;
        }
    }

    private void Quit()
    {
        if (Input.GetKey(KeyCode.Escape))
            Application.Quit();
    }
}
