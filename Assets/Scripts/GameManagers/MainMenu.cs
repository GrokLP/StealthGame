using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] Animation _mainMenuAnimator;
    [SerializeField] AnimationClip _fadeOutAnimation; //rename these if i change animation
    [SerializeField] AnimationClip _fadeInAnimation;

    [SerializeField] Button StartNewGameButton;
    [SerializeField] Button LoadGameButton;
    [SerializeField] Button SettingsButton;
    [SerializeField] Button QuitButton;

    public Events.EventFadeComplete OnMainMenuFadeComplete;

    private void Start()
    {
        StartNewGameButton.onClick.AddListener(HandleStartNewGameButton);
        LoadGameButton.onClick.AddListener(HandleLoadGameButton);
        SettingsButton.onClick.AddListener(HandleSettingsButton);
        QuitButton.onClick.AddListener(HandleQuitButton);
        
        GameManager.Instance.OnGameStateChanged.AddListener(HandleGameStateChanged);
    }
    public void OnFadeOutComplete()
    {
        OnMainMenuFadeComplete.Invoke(true);
    }

    public void OnFadeInComplete()
    {
        OnMainMenuFadeComplete.Invoke(false);
        UIManager.Instance.SetDummyCameraActive(true);
    }

    void HandleGameStateChanged(GameManager.GameState currentState, GameManager.GameState previousState)
    {
        if(previousState == GameManager.GameState.PREGAME && currentState == GameManager.GameState.RUNNING)
        {
            FadeOut();
        }

        if(previousState != GameManager.GameState.PREGAME && currentState == GameManager.GameState.PREGAME)
        {
            FadeIn();
        }
    }

    public void FadeOut()
    {
        UIManager.Instance.SetDummyCameraActive(false);
        
        _mainMenuAnimator.Stop();
        _mainMenuAnimator.clip = _fadeOutAnimation;
        _mainMenuAnimator.Play();
    }

    public void FadeIn()
    {
        _mainMenuAnimator.Stop();
        _mainMenuAnimator.clip = _fadeInAnimation;
        _mainMenuAnimator.Play();
    }

    void HandleStartNewGameButton()
    {
        AudioManager.Instance.PlaySound("ButtonClick");
        GameManager.Instance.StartGame();
    }

    void HandleLoadGameButton()
    {
        AudioManager.Instance.PlaySound("ButtonClick");
        //add functionality
    }

    void HandleSettingsButton()
    {
        AudioManager.Instance.PlaySound("ButtonClick");
        //add functionality
    }

    void HandleQuitButton()
    {
        AudioManager.Instance.PlaySound("ButtonClick");
        GameManager.Instance.QuitGame();
    }
}

