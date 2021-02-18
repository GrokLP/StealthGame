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
        AudioManager.Instance.PlaySound("ButtonClick");
        GameManager.Instance.TogglePause();
    }

    void HandleRestartClick()
    {
        AudioManager.Instance.PlaySound("ButtonClick");
        GameManager.Instance.RestartLevel();
    }

    void HandleNextLevelClick()
    {
        AudioManager.Instance.PlaySound("ButtonClick");
        GameManager.Instance.LoadNextLevel();
    }

    void HandlePreviousLevelClick()
    {
        AudioManager.Instance.PlaySound("ButtonClick");
        GameManager.Instance.LoadPreviousLevel();
    }

    
    void HandleMainMenuClick()
    {
        AudioManager.Instance.PlaySound("ButtonClick");
        GameManager.Instance.RestartGame();
    }

    void HandleQuitClick()
    {
        AudioManager.Instance.PlaySound("ButtonClick");
        GameManager.Instance.QuitGame();
    }
}
