using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    //-------------------------------------------

    public GameObject MainMenu, PauseMenu, GameOverMenu;

    //-------------------------------------------

    private void Awake()
    {
        MainMenu.SetActive(true);
        PauseMenu.SetActive(false);
        GameOverMenu.SetActive(false);
    }

    //-------------------------------------------

    public void StartGame()
    {
        MainMenu.SetActive(false);
        GameManager.instance.StartGame();
    }

    //-------------------------------------------

    public void PauseGame()
    {
        MainMenu.SetActive(false);
        PauseMenu.SetActive(true);
        GameManager.instance.PauseGame();
    }

    //-------------------------------------------

    public void ResumeGame()
    {
        PauseMenu.SetActive(false);
        GameManager.instance.ResumeGame();
    }

    //-------------------------------------------

    public void RestartGame()
    {
        GameOverMenu.SetActive(false);
        GameManager.instance.StartGame();
    }

    //-------------------------------------------

    public void ShowGameOver()
    {
        GameOverMenu.SetActive(true);
    }
    
    //-------------------------------------------
}
