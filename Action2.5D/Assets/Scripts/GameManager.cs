using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    internal bool isPaused = false;

    void Update()
    {
        Pause();
        Quit();
    }

    private void Pause()
    {
        if (Input.GetButtonDown("Pause") && !isPaused)
        {
            Time.timeScale = 0f;
            isPaused = true;
        }

        else if (Input.GetButtonDown("Pause") && isPaused)
        {
            Time.timeScale = 1f;
            isPaused = false;
        }
    }

    private void Quit()
    {
        if (Input.GetKey(KeyCode.Escape))
            Application.Quit();
    }
}
