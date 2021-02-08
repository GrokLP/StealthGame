using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorSwitch : MonoBehaviour
{
    [SerializeField] Animator animateElevator;

    void OnTriggerEnter(Collider collider) 
    {
        animateElevator.SetBool("ElevatorSwitch", true);
    }

    private void OnTriggerExit(Collider other)
    {
        StartCoroutine(DelayTrigger());
    }

    IEnumerator DelayTrigger()
    {
        yield return new WaitForSeconds(5);

        animateElevator.SetBool("ElevatorSwitch", false);
    }
}
