using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelFinishPushChild : MonoBehaviour
{
    //check if both exits are successfully triggered
    //OnGameWin event

    public static event System.Action OnGameWin;

    [SerializeField] PushChildExitOne pushChildExitOne;
    [SerializeField] PushChildExitTwo pushChildExitTwo;

    private void Start()
    {
        //PushChildExitOne.OnExitTriggered += CheckExits;
        //PushChildExitTwo.OnExitTriggered += CheckExits;
    }

    void CheckExits() //was causing false win state on reload?
    {
        if(pushChildExitOne.ExitTriggered && pushChildExitTwo.ExitTriggered)
        {
            if (OnGameWin != null)
                OnGameWin();
        }
        else
        {
            return;
        }
    }

    private void Update()
    {
        if (pushChildExitOne.ExitTriggered && pushChildExitTwo.ExitTriggered)
        {
            if (OnGameWin != null)
                OnGameWin();
        }
    }
}
