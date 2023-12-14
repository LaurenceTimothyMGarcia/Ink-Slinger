using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject aboutMenu;

    public GameObject credits;

    public void Start()
    {
        mainMenu.SetActive(true);
        aboutMenu.SetActive(false);
        credits.SetActive(false);
    }

    public void AboutMenu()
    {
        mainMenu.SetActive(false);
        aboutMenu.SetActive(true);
        credits.SetActive(false);
    }

    public void MainMenu()
    {
        mainMenu.SetActive(true);
        aboutMenu.SetActive(false);
        credits.SetActive(false);

    }
    public void LoadCredits()
    {
        credits.SetActive(true);
        mainMenu.SetActive(false);
        aboutMenu.SetActive(false);
    }
}
