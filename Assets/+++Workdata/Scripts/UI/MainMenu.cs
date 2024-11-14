using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private CanvasGroup mainMenuScreen;
    [SerializeField] private CanvasGroup levelSelectionScreen;
    [SerializeField] private CanvasGroup optionScreen;

    [SerializeField] private Button[] levelButtons;

    private GameManager gameManager;

    private void Awake()
    {
        Time.timeScale = 1f;
        gameManager = FindObjectOfType<GameManager>();
    }

    private void Start()
    {
        for (int i = 0; i < levelButtons.Length; i++)
        {
            levelButtons[i].interactable = false;
        }

        for (int i = 0; i < gameManager.unlockedLevel.Count; i++)
        {
            levelButtons[i].interactable = true;
        }
    }

    public void LoadLevel1()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void LoadLevel2()
    {
        SceneManager.LoadScene("Level2");
    }

    public void ShowMainMenu()
    {
        mainMenuScreen.ShowCanvasGroup();
        levelSelectionScreen.HideCanvasGroup();
        optionScreen.HideCanvasGroup();
    }

    public void ShowLevelSelection()
    {
        mainMenuScreen.HideCanvasGroup();
        levelSelectionScreen.ShowCanvasGroup();
    }

    public void ShowOptions()
    {
        mainMenuScreen.HideCanvasGroup();
        optionScreen.ShowCanvasGroup();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
