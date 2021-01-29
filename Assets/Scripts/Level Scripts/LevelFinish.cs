using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelFinish : MonoBehaviour
{
    public static event System.Action OnGameWin;
    public static event System.Action OnWrongColor;

    [SerializeField] ExitColor exitColor;
    enum ExitColor
    {
        RED,
        BLUE,
        GREEN,
        WHITE
    }

    string exitColorString;

    private void Start()
    {
        exitColorString = exitColor.ToString();
    }

    private void OnTriggerEnter(Collider other)
    {
        string currentPlayerColor = GameManager.Instance.CurrentPlayerColor.ToString();

        if (exitColorString == currentPlayerColor && other.CompareTag("Player"))
        {
            if (OnGameWin != null)
                OnGameWin();
        }
        else if (exitColorString != currentPlayerColor && other.CompareTag("Player"))
        {
            if (OnWrongColor != null)
                OnWrongColor();
        }
    }
}
