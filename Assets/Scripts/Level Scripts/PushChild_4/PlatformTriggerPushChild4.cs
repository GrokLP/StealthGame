using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformTriggerPushChild4 : MonoBehaviour
{
    bool raisePlatform;
    bool triggered;

    public bool RaisePlatform
    {
        get { return raisePlatform; }
    }

    private void OnTriggerStay(Collider other)
    {
        raisePlatform = true;

        if (!triggered)
        {
            AudioManager.Instance.PlaySound("TriggerActivated");
            triggered = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        raisePlatform = false;

        AudioManager.Instance.PlaySound("TriggerDeactivated");
        triggered = false;
    }
}
