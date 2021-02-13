using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformTriggerPushChild4 : MonoBehaviour
{
    bool raisePlatform;

    public bool RaisePlatform
    {
        get { return raisePlatform; }
    }

    private void OnTriggerStay(Collider other)
    {
        raisePlatform = true;
    }

    private void OnTriggerExit(Collider other)
    {
        raisePlatform = false;
    }
}
