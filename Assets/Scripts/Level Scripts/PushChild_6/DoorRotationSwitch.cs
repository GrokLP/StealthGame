using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorRotationSwitch : MonoBehaviour
{
    bool openDoor;
    bool triggered;

    public bool OpenDoor
    {
        get { return openDoor; }
    }

    private void OnTriggerStay(Collider other)
    {
        openDoor = true;

        if(!triggered)
        {
            AudioManager.Instance.PlaySound("TriggerActivated");
            triggered = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        openDoor = false;
        AudioManager.Instance.PlaySound("TriggerDeactivated");
        triggered = false;
    }
}
