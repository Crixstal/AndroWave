using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MainMenuManagement : MonoBehaviour
{
    [SerializeField] private GameObject main = null;
    [SerializeField] private GameObject mainFirstButton = null;

    [SerializeField] private GameObject level = null;
    [SerializeField] private GameObject levelFirstButton = null;

    [SerializeField] private GameObject language = null;
    [SerializeField] private GameObject languageFirstButton = null;

    [SerializeField] private GameObject boar = null;
    [SerializeField] private GameObject player = null;

    [SerializeField] private EventSystem eventSystem = null;

    void Awake()
    {
        main.SetActive(true);
        level.SetActive(false);
        language.SetActive(false);

        boar.SetActive(true);
        player.SetActive(false);

        eventSystem.SetSelectedGameObject(mainFirstButton);
    }

    public void ChangeScene(int scene)
    {
        SceneManager.LoadScene(scene);
        Time.timeScale = 1f;
    }

    public void SelectLevel()
    {
        main.SetActive(false);
        level.SetActive(true);
        language.SetActive(false);

        boar.SetActive(false);
        player.SetActive(true);

        eventSystem.SetSelectedGameObject(levelFirstButton);
    }

    public void Language()
    {
        main.SetActive(false);
        level.SetActive(false);
        language.SetActive(true);

        boar.SetActive(true);
        player.SetActive(false);

        eventSystem.SetSelectedGameObject(languageFirstButton);
    }

    public void Return()
    {
        main.SetActive(true);
        level.SetActive(false);
        language.SetActive(false);

        boar.SetActive(true);
        player.SetActive(false);

        eventSystem.SetSelectedGameObject(mainFirstButton);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
