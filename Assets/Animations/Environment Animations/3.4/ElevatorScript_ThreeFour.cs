using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorScript_ThreeFour : MonoBehaviour
{
    [SerializeField] Animator elevatorAnimator;

    float animationTime = 11;
    float nextTrigger;

    private void OnTriggerEnter(Collider other)
    {
        if(Time.time > nextTrigger)
        {
            nextTrigger = Time.time + animationTime;
            elevatorAnimator.SetTrigger("ElevatorTrigger");
        }
    }
}
