using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] Animator wrongColorAnim;
    
    [SerializeField] MainMenu _mainMenu;
    [SerializeField] PauseMenu _pauseMenu;
    [SerializeField] GameObject _gameLoseUI;
    [SerializeField] GameObject _gameWinUI;
    [SerializeField] GameObject _restartMessage;
    [SerializeField] GameObject _nextLevelMessage;
    [SerializeField] GameObject _finishLevelMessage;

    [SerializeField] Camera _dummyCamera;

    public Events.EventFadeComplete OnMainMenuFadeComplete;

    UIPosition setUIPosition;

    private void Start()
    {
        _mainMenu.OnMainMenuFadeComplete.AddListener(HandleMainMenuFadeComplete);
        GameManager.Instance.OnGameStateChanged.AddListener(HandleGameStateChanged);
        GameManager.Instance.OnGameStateChangedLose.AddListener(HandleGameStateChangedLose);
    }

    private void Update()
    {
        if(GameManager.Instance.CurrentGameState != GameManager.GameState.PREGAME)
        {
            return;
        }
        
        if(Input.GetKeyDown(KeyCode.Space) && SceneManager.sceneCount <= 1)
        {
            GameManager.Instance.StartGame();
        }
    }

    public void SetDummyCameraActive(bool active)
    {
        _dummyCamera.gameObject.SetActive(active);
    }

    void HandleGameStateChanged(GameManager.GameState currentState, GameManager.GameState previousState)
    {
        _pauseMenu.gameObject.SetActive(currentState == GameManager.GameState.PAUSED); //could also be an if/else statement instead of inside function call
        _gameWinUI.gameObject.SetActive(currentState == GameManager.GameState.GAMEWIN);
        _gameLoseUI.gameObject.SetActive(currentState == GameManager.GameState.GAMELOSE);
        
        _restartMessage.gameObject.SetActive(currentState == GameManager.GameState.GAMELOSE); //need an if statement that determines gamewin/lose state to change text
        _nextLevelMessage.gameObject.SetActive(currentState == GameManager.GameState.GAMEWIN);

        //******Set position of in world pause menu UI******
        /*if (currentState == GameManager.GameState.PAUSED)
        {
            var setPausePosition = FindObjectOfType<PlayerController>().transform.position;
            _pauseMenu.transform.position = setPausePosition + new Vector3(0, 5, 0);
        }*/

        if (currentState == GameManager.GameState.RUNNING)
        {
            GetUIPositions();
        }
    }

    void HandleGameStateChangedLose(string gameOverSource)
    {
        if(GameManager.Instance.DisplayMessage == false)
        {
            switch (gameOverSource)
            {
                case "Spotlight":
                    _gameLoseUI.GetComponentInChildren<TextMeshProUGUI>().text = "Spotted!";
                    GameManager.Instance.DisplayMessage = true;
                    break;

                case "Laser":
                    _gameLoseUI.GetComponentInChildren<TextMeshProUGUI>().text = "Disintegrated!";
                    GameManager.Instance.DisplayMessage = true;
                    break;

                case "GuardDog":
                    _gameLoseUI.GetComponentInChildren<TextMeshProUGUI>().text = "The dogs got you!";
                    GameManager.Instance.DisplayMessage = true;
                    break;

                case "Camera":
                    _gameLoseUI.GetComponentInChildren<TextMeshProUGUI>().text = "Smile!";
                    GameManager.Instance.DisplayMessage = true;
                    break;

                case "TooClose":
                    _gameLoseUI.GetComponentInChildren<TextMeshProUGUI>().text = "Too Close!";
                    GameManager.Instance.DisplayMessage = true;
                    break;

                case "Fell":
                    _gameLoseUI.GetComponentInChildren<TextMeshProUGUI>().text = "You Fell!";
                    GameManager.Instance.DisplayMessage = true;
                    break;

                default:
                    break;
            }
        }
    }

    void HandleMainMenuFadeComplete(bool fadeOut)
    {
        OnMainMenuFadeComplete.Invoke(fadeOut);
    }

    void GetUIPositions()
    {
        setUIPosition = FindObjectOfType<UIPosition>();

        _gameLoseUI.transform.position = setUIPosition.gameEndUIMessage.position;
        _gameLoseUI.transform.localScale = new Vector3(setUIPosition.gameEndUIScale, setUIPosition.gameEndUIScale, setUIPosition.gameEndUIScale);

        _gameWinUI.transform.position = setUIPosition.gameEndUIMessage.position;
        _gameWinUI.transform.localScale = new Vector3(setUIPosition.gameEndUIScale, setUIPosition.gameEndUIScale, setUIPosition.gameEndUIScale);

        _restartMessage.transform.position = setUIPosition.pressSpaceMessage.position;
        _restartMessage.transform.localScale = new Vector3(setUIPosition.pressSpaceUIScale, setUIPosition.pressSpaceUIScale, setUIPosition.pressSpaceUIScale);

        _nextLevelMessage.transform.position = setUIPosition.pressSpaceMessage.position;
        _nextLevelMessage.transform.localScale = new Vector3(setUIPosition.pressSpaceUIScale, setUIPosition.pressSpaceUIScale, setUIPosition.pressSpaceUIScale);

        _finishLevelMessage.transform.position = setUIPosition.levelFinishMessage.position;
        _finishLevelMessage.transform.localScale = new Vector3(setUIPosition.levelFinishScale, setUIPosition.levelFinishScale, setUIPosition.levelFinishScale);
    }

    public void DisplayWrongColorMessage()
    {
        wrongColorAnim.SetTrigger("WrongColor");
    }
}
