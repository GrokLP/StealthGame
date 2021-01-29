using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] Button ResumeButton;
    [SerializeField] Button RestartButton;
    [SerializeField] Button NextLevelButton;
    [SerializeField] Button PreviousLevelButton;
    [SerializeField] Button MainMenuButton;
    [SerializeField] Button QuitButton;

    private void Start()
    {
        ResumeButton.onClick.AddListener(HandleResumeClick);
        RestartButton.onClick.AddListener(HandleRestartClick);
        NextLevelButton.onClick.AddListener(HandleNextLevelClick);
        PreviousLevelButton.onClick.AddListener(HandlePreviousLevelClick);
        MainMenuButton.onClick.AddListener(HandleMainMenuClick);
        QuitButton.onClick.AddListener(HandleQuitClick);
    }

    void HandleResumeClick()
    {
        GameManager.Instance.TogglePause();
    }

    void HandleRestartClick()
    {
        GameManager.Instance.RestartLevel();
    }

    void HandleNextLevelClick()
    {
        GameManager.Instance.LoadNextLevel();
    }

    void HandlePreviousLevelClick()
    {
        GameManager.Instance.LoadPreviousLevel();
    }

    
    void HandleMainMenuClick()
    {
        GameManager.Instance.RestartGame();
    }

    void HandleQuitClick()
    {
        GameManager.Instance.QuitGame();
    }
}
