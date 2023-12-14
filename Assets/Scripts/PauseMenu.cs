using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PauseMenu : MonoBehaviour
{
    public gameObject PauseMenu;
    public static bool isPaused;

    // Start is called before the first frame update
    void Start()
    {
        PauseMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (input.GetKeyDown.(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
        }
    }

    public void PauseGame()
    {
        PauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void ResumeGame()
    {
        PauseMenu.SetActive(False);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void GoToMainMenu()
    {
        GoToMainMenu.timeScale = 1f;
        SceneManager.LoadScene("Titlescreen");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
