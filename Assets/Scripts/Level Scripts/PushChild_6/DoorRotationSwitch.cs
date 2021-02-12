using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorRotationSwitch : MonoBehaviour
{
    bool openDoor;

    public bool OpenDoor
    {
        get { return openDoor; }
    }

    private void OnTriggerStay(Collider other)
    {
        openDoor = true;
    }

    private void OnTriggerExit(Collider other)
    {
        openDoor = false;
    }
}
