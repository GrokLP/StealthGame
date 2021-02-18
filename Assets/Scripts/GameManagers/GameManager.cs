using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public enum GameState
    {
        PREGAME,
        RUNNING,
        PAUSED,
        GAMEWIN,
        GAMELOSE
    }
    
    public GameObject[] SystemPrefabs; //add system prefabs in inspector
    public Events.EventGameState OnGameStateChanged;
    public Events.EventGameStateLose OnGameStateChangedLose;

    public static event System.Action OnLoadComplete;

    List<GameObject> _instancedSystemPrefabs;
    List<AsyncOperation> _loadOperations;
    
    private int _currentLevelIndex;
    public int CurrentLevelIndex
    {
        get { return _currentLevelIndex; }
    }

    private string gameOverSource;

    private bool displayMessage;
    public bool DisplayMessage
    {
        get { return displayMessage; }
        set { displayMessage = value; }
    }

    private ChangeColor.PlayerColor currentPlayerColor;
    public ChangeColor.PlayerColor CurrentPlayerColor
    {
        get { return currentPlayerColor;}
    }

    private GameState _currentGameState = GameState.PREGAME;
    public GameState CurrentGameState
    {
        get { return _currentGameState; }
        private set { _currentGameState = value; }
    }

    int levelAttempts = 1;

    public int LevelAttempts
    {
        get { return levelAttempts; }
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);

        _instancedSystemPrefabs = new List<GameObject>();
        _loadOperations = new List<AsyncOperation>();

        InstantiateSystemPrefabs();

        UIManager.Instance.OnMainMenuFadeComplete.AddListener(HandleMainMenuFadeComplete);

        PlayerController.OnGameLose += HandleOnGameLose;
        PlayerDetection.OnGameLose += HandleOnGameLose;
        FieldOfView.OnGameLose += HandleOnGameLose;
        Drones.OnGameLose += HandleOnGameLose;
        SecurityCameras.OnGameLose += HandleOnGameLose;
        GuardDog.OnGameLose += HandleOnGameLose;
        LaserGuard.OnGameLose += HandleOnGameLose;
        FallTrigger.OnGameLose += HandleOnGameLose;

        LevelFinish.OnGameWin += HandleOnGameWin;
        LevelFinish.OnWrongColor += OnWrongColor;
        LevelFinishThrowChild.OnGameWin += HandleOnGameWin;
        LevelFinishThrowChild.OnWrongColor += OnWrongColor;
        LevelFinishThrowChild.OnNoChild += OnNoChild;

        PushChildExitOne.OnNoChild += OnNoChild;
        PushChildExitTwo.OnNoChild += OnNoChild;
        PushChildExitOne.OnWrongColor += OnWrongColor;
        PushChildExitTwo.OnWrongColor += OnWrongColor;
        PushChildExitOne.OnAlreadyOccupied += OnAlreadyOccupied;
        PushChildExitTwo.OnAlreadyOccupied += OnAlreadyOccupied;
        LevelFinishPushChild.OnGameWin += HandleOnGameWin;
    }
        
    private void Update()
    {
        if (_currentGameState == GameState.PREGAME)
        {
            return;
        }

        if (Input.GetButtonDown("Pause"))
            
        {
            if (_currentGameState == GameState.GAMEWIN | _currentGameState == GameState.GAMELOSE)
                return;

            TogglePause();
        }

        if (Input.GetButtonDown("LevelEnd") && _currentGameState == GameState.GAMELOSE)
        {
            RestartLevel();
        }

        if (Input.GetButtonDown("LevelEnd") && _currentGameState == GameState.GAMEWIN)
        {
            LoadNextLevel();
        }
    }

    // Loads all systems and saves them to a list for cleanup
    void InstantiateSystemPrefabs()
    {
        GameObject prefabInstance;
        for (int i = 0; i < SystemPrefabs.Length; i++)
        {
            prefabInstance = Instantiate(SystemPrefabs[i]);
            _instancedSystemPrefabs.Add(prefabInstance);
        }
    }

    // Asynchronously load in level, send message that it is complete, 
    // add to list of operations, and save current level name in variable
    public void LoadLevel(int levelIndex)
    {
        AsyncOperation ao = SceneManager.LoadSceneAsync(levelIndex, LoadSceneMode.Additive);
        if (ao == null)
        {
            Debug.LogError("[GameManager] Unable to load level " + levelIndex);
            return;
        }

        ao.completed += OnLoadOperationComplete;
        _loadOperations.Add(ao);
        
        if(levelIndex == _currentLevelIndex) //track level loads for in level events
        {
            levelAttempts++;
        }
        else
        {
            levelAttempts = 1;
        }

        _currentLevelIndex = levelIndex;
    }

    // Asynchronously unload level and send message that it is complete
    public void UnloadLevel(int levelIndex)
    {
        AsyncOperation ao = SceneManager.UnloadSceneAsync(levelIndex);
        if (ao == null)
        {
            Debug.LogError("[GameManager] Unable to unload level " + levelIndex);
            return;
        }
        ao.completed += OnUnloadOperationComplete;
    }
   
    // upon receiving message that load is complete, remove from list
    // of operations and update game state to RUNNING
    void OnLoadOperationComplete(AsyncOperation ao)
    {
        if(_loadOperations.Contains(ao))
        {
            _loadOperations.Remove(ao);

            if (_loadOperations.Count == 0)
            {
                UpdateState(GameState.RUNNING);
            }          
        }

        if(OnLoadComplete != null)
            OnLoadComplete();
    }

    void OnUnloadOperationComplete(AsyncOperation ao)
    {
        //unload complete
    }

    // Updates gamestate and sets/resets timescale accordingly, then
    // invokes gamestate change event while passing on the current
    // and previous state
    void UpdateState(GameState state)
    {
        GameState previousGameState = _currentGameState;
        _currentGameState = state;

        switch(_currentGameState)
        {
            case GameState.PREGAME:
                Time.timeScale = 1;
                displayMessage = false;
                break;

            case GameState.RUNNING:
                Time.timeScale = 1;
                ChangeColor.Instance.OnPlayerColorChange.AddListener(OnColorChange); //add listening on game run because there is no player to get color from in main menu
                displayMessage = false;
                break;

            case GameState.PAUSED:
                Time.timeScale = 0;
                break;

            case GameState.GAMEWIN:
                //Time.timeScale = 0;
                break;

            case GameState.GAMELOSE:
                //Time.timeScale = 0;
                break;

            default:
                break;
        }

        OnGameStateChanged.Invoke(_currentGameState, previousGameState);

        if (_currentGameState == GameState.GAMELOSE)
            OnGameStateChangedLose.Invoke(gameOverSource);
    }
    
    // Listens for for menu fade-in to be complete (through UIManager)
    // and unloads level (this would occur after a restart)
    void HandleMainMenuFadeComplete(bool fadeOut)
    {
        if(!fadeOut)
        {
            UnloadLevel(_currentLevelIndex);
        }
    }

    public void StartGame()
    {
        if (SceneManager.sceneCount <= 1)
            LoadLevel(1);
    }

    //ternary operator used as argument for update state method
    public void TogglePause()
    {
        UpdateState(_currentGameState == GameState.RUNNING ? GameState.PAUSED : GameState.RUNNING); //ternary operator used as argument for update state method
    }

    public void RestartGame()
    {
        UpdateState(GameState.PREGAME);
    }

    public void QuitGame()
    {
        //autosave and cleanup etc
        
        Application.Quit();
    }

    public void RestartLevel()
    {
        UpdateState(GameState.RUNNING);//checking for platform pause -- seems to work? Will leave note until sure!
        UnloadLevel(_currentLevelIndex);
        LoadLevel(_currentLevelIndex);
    }

    public void LoadNextLevel()
    {
        UnloadLevel(_currentLevelIndex);
        if(_currentLevelIndex < SceneManager.sceneCountInBuildSettings - 1)
        {
            UpdateState(GameState.RUNNING);//platforms
            LoadLevel(_currentLevelIndex + 1);
        }
        else
        {
            UpdateState(GameState.RUNNING);//platforms
            LoadLevel(1);
        }
           
    }

    public void LoadPreviousLevel()
    {
        UnloadLevel(_currentLevelIndex);
        if (_currentLevelIndex > 1)
        {
            UpdateState(GameState.RUNNING);//platforms
            LoadLevel(_currentLevelIndex - 1);
        }

        else
        {
            UpdateState(GameState.RUNNING);//platforms
            LoadLevel(1);
        }

    }

    // base OnDestroy from singleton class ensures no duplicates
    // and for loop deletes all system prefabs in list and then clears
    // list
    protected override void OnDestroy()
    {
        base.OnDestroy();

        for (int i = 0; i < _instancedSystemPrefabs.Count; i++)
        {
            Destroy(_instancedSystemPrefabs[i]);
        }
        _instancedSystemPrefabs.Clear();
    }

    void HandleOnGameLose(string source)
    {
        gameOverSource = source;//need to pass this to UI manager
        AudioManager.Instance.StopSound("Typing"); //prevents dialogue audio from continuing if level restarts -- maybe a better solution
        UpdateState(GameState.GAMELOSE);
    }

    void HandleOnGameWin()
    {
        UpdateState(GameState.GAMEWIN);
    }

    void OnColorChange(ChangeColor.PlayerColor currentColor, ChangeColor.PlayerColor previousColor)
    {
        currentPlayerColor = currentColor;
        PostProcessingManager.Instance.UpdatePostProcessingColor(currentColor, previousColor);
    }

    void OnWrongColor()
    {
        UIManager.Instance.DisplayWrongColorMessage();
    }

    void OnNoChild()
    {
        UIManager.Instance.DisplayNoChildMessage();
    }

    void OnAlreadyOccupied()
    {
        UIManager.Instance.DisplayAlreadyOccupiedMessage();
    }
}
