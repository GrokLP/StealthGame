using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelFinishThrowChild : MonoBehaviour
{
    public static event System.Action OnGameWin;
    public static event System.Action OnWrongColor;
    public static event System.Action OnNoChild;

    [SerializeField] ExitColor exitColor;
    enum ExitColor
    {
        RED,
        BLUE,
        GREEN,
        WHITE
    }

    string exitColorString;

    ThrowObject throwObjectScript;

    private void Start()
    {
        exitColorString = exitColor.ToString();
        throwObjectScript = GameObject.FindGameObjectWithTag("Player").GetComponent<ThrowObject>();


    }

    private void OnTriggerEnter(Collider other)
    {
        string currentPlayerColor = GameManager.Instance.CurrentPlayerColor.ToString();

        if (exitColorString == currentPlayerColor && other.CompareTag("Player") && throwObjectScript.hasChild)
        {
            if (OnGameWin != null)
                OnGameWin();

            AudioManager.Instance.PlaySound("LevelClear");
        }
        else if (exitColorString != currentPlayerColor && other.CompareTag("Player") && throwObjectScript.hasChild)
        {
            if (OnWrongColor != null)
                OnWrongColor();

            AudioManager.Instance.PlaySound("WrongColor");
        }
        else if (exitColorString != currentPlayerColor && other.CompareTag("Player") && !throwObjectScript.hasChild)
        {
            if (OnWrongColor != null)
                OnWrongColor();

            AudioManager.Instance.PlaySound("WrongColor");
        }

        else if (exitColorString == currentPlayerColor && other.CompareTag("Player") && !throwObjectScript.hasChild)
        {
            if (OnWrongColor != null)
                OnNoChild();

            AudioManager.Instance.PlaySound("WrongColor");
        }
    }
}
