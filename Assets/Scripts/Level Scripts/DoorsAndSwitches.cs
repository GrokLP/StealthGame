using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorsAndSwitches : MonoBehaviour
{

    [SerializeField] Animator animateDoors;

    void OnTriggerEnter(Collider player) //need to rename player variable and make sure i add rididbody to stationary guard prefab
    {
        animateDoors.SetBool("OpenDoors", true);
    }
}
