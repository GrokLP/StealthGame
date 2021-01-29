using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingPlatformSwitch : MonoBehaviour
{
    bool trigger;

    public bool Trigger
    {
        get { return trigger; }
    }

    private void OnTriggerEnter(Collider other)
    {
        trigger = true;
    }

    private void OnTriggerExit(Collider other)
    {
        trigger = false;
    }
}
