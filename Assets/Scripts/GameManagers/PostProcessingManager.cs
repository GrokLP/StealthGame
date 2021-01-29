using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostProcessingManager : Singleton<PostProcessingManager>
{
    [SerializeField] GameObject master;
    [SerializeField] GameObject red;
    [SerializeField] GameObject blue;
    [SerializeField] GameObject green;
    [SerializeField] GameObject white;

    ChangeColor.PlayerColor currentPlayerColor;

    private void Start()
    {
        GameManager.Instance.OnGameStateChanged.AddListener(HandleGameStateChanged);
    }

    public void UpdatePostProcessingColor(ChangeColor.PlayerColor currentColor, ChangeColor.PlayerColor previousColor)
    {
        switch(currentColor)
        {
            case ChangeColor.PlayerColor.RED:
                red.SetActive(true);
                break;

            case ChangeColor.PlayerColor.BLUE:
                blue.SetActive(true);
                break;

            case ChangeColor.PlayerColor.GREEN:
                green.SetActive(true);
                break;

            case ChangeColor.PlayerColor.WHITE:
                white.SetActive(true);
                break;

            default:
                break;
        }

        switch (previousColor)
        {
            case ChangeColor.PlayerColor.RED:
                if (currentColor != ChangeColor.PlayerColor.RED)
                    red.SetActive(false);
                break;

            case ChangeColor.PlayerColor.BLUE:
                if (currentColor != ChangeColor.PlayerColor.BLUE)
                    blue.SetActive(false);
                break;

            case ChangeColor.PlayerColor.GREEN:
                if (currentColor != ChangeColor.PlayerColor.GREEN)
                    green.SetActive(false);
                break;

            case ChangeColor.PlayerColor.WHITE:
                if (currentColor != ChangeColor.PlayerColor.WHITE)
                    white.SetActive(false);
                break;

            default:
                break;
        }
    }

    void HandleGameStateChanged(GameManager.GameState currentState, GameManager.GameState previousState)
    {
        if(currentState == GameManager.GameState.RUNNING && previousState != GameManager.GameState.PAUSED)
        {
            red.SetActive(false);
            blue.SetActive(false);
            green.SetActive(false);
            white.SetActive(false);
            master.SetActive(false);
        }

        //else if (currentState == GameManager.GameState.RUNNING && previousState == GameManager.GameState.PAUSED)
        //{

        //}
    }
}


