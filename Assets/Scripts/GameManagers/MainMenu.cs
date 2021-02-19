using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject mainFirstSelected;

    [SerializeField] Animation _mainMenuAnimator;
    [SerializeField] AnimationClip _fadeToBlackAnimation; //rename these if i change animation
    [SerializeField] AnimationClip _fadeFromBlackAnimation;

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
    public void OnFadeOutComplete() //fadetoblack
    {
        OnMainMenuFadeComplete.Invoke(true);
    }

    public void OnFadeInComplete() //fadefromblack
    {
        OnMainMenuFadeComplete.Invoke(false);
        //UIManager.Instance.SetDummyCameraActive(true);
    }

    void HandleGameStateChanged(GameManager.GameState currentState, GameManager.GameState previousState)
    {
        if(previousState == GameManager.GameState.PREGAME && currentState == GameManager.GameState.RUNNING)
        {
            //FadeOut();
            mainMenu.SetActive(false);
        }

        if(previousState != GameManager.GameState.PREGAME && currentState == GameManager.GameState.PREGAME)
        {
            //FadeIn();
            GameManager.Instance.UnloadLevel(GameManager.Instance.CurrentLevelIndex);
            mainMenu.SetActive(true);
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(mainFirstSelected);
        }
    }

    public void FadeOut()
    {
        //UIManager.Instance.SetDummyCameraActive(false);
        
        _mainMenuAnimator.Stop();
        _mainMenuAnimator.clip = _fadeToBlackAnimation;
        _mainMenuAnimator.Play();
    }

    public void FadeIn()
    {
        _mainMenuAnimator.Stop();
        _mainMenuAnimator.clip = _fadeFromBlackAnimation;
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

