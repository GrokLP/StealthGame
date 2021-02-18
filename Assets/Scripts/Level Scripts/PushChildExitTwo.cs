using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushChildExitTwo : MonoBehaviour
{
    public static event System.Action OnWrongColor;
    public static event System.Action OnNoChild;
    public static event System.Action OnAlreadyOccupied;
    public static event System.Action OnExitTriggered;

    [SerializeField] ExitColor exitColor;
    enum ExitColor
    {
        RED,
        BLUE,
        GREEN,
        WHITE
    }
    string exitColorString;
    
    bool childOccupied;
    public bool ChildOccupied
    {
        get { return childOccupied; }
    }

    bool exitTriggered;
    public bool ExitTriggered
    {
        get { return exitTriggered; }
    }

    [SerializeField] PushChildExitOne pushChildExitOne;

    private void Start()
    {
        exitColorString = exitColor.ToString();
        childOccupied = false;
        exitTriggered = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        //check if childcube - set bool to true
        //check if already occupied by child
        //check if player is correct color
        //check if child is in other exit


        string currentPlayerColor = GameManager.Instance.CurrentPlayerColor.ToString();

        if (other.CompareTag("PushChildCube") && exitColorString == "GREEN")
        {
            childOccupied = true;
            exitTriggered = true;

            if (OnExitTriggered != null)
                OnExitTriggered();
        }

        else if(other.CompareTag("PushChildCube") && exitColorString != "GREEN")
        {
            if (OnWrongColor != null)
                OnWrongColor();

            AudioManager.Instance.PlaySound("WrongColor");
        }

        else if(other.CompareTag("Player") && childOccupied)
        {
            if (OnAlreadyOccupied != null)
                OnAlreadyOccupied();

            AudioManager.Instance.PlaySound("WrongColor");
        }

        else if (exitColorString != currentPlayerColor && other.CompareTag("Player") && pushChildExitOne.ChildOccupied)
        {
            if (OnWrongColor != null)
                OnWrongColor();

            AudioManager.Instance.PlaySound("WrongColor");
        }

        else if (exitColorString != currentPlayerColor && other.CompareTag("Player") && !pushChildExitOne.ChildOccupied)
        {
            if (OnWrongColor != null)
                OnWrongColor();

            AudioManager.Instance.PlaySound("WrongColor");
        }

        else if (exitColorString == currentPlayerColor && other.CompareTag("Player") && !pushChildExitOne.ChildOccupied)
        {
            exitTriggered = true; //set to true in case child cube is on move and enters after player

            if (OnNoChild != null)
                OnNoChild();

            AudioManager.Instance.PlaySound("WrongColor");
        }

        else if(exitColorString == currentPlayerColor && other.CompareTag("Player") && pushChildExitOne.ChildOccupied)
        {
            exitTriggered = true;

            if (OnExitTriggered != null)
                OnExitTriggered();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        string currentPlayerColor = GameManager.Instance.CurrentPlayerColor.ToString();

        if (other.CompareTag("PushChildCube"))
        {
            childOccupied = false;
            exitTriggered = false;
        }

        else if (exitColorString == currentPlayerColor && other.CompareTag("Player"))
        {
            exitTriggered = false;
        }
    }
}
