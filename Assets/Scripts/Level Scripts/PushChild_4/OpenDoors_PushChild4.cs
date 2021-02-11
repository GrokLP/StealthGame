using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoors_PushChild4 : MonoBehaviour
{
    [SerializeField] Animator animateDoors;

    private void OnTriggerEnter(Collider other)
    {
        animateDoors.SetBool("OpenDoors", true);
    }
}
