using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorSwitch : MonoBehaviour
{
    [SerializeField] Animator animateElevator;
    [SerializeField] ThrownChild thrownChild;
    bool aTriggered;
    bool dTriggered;
    void OnTriggerEnter(Collider other) 
    {
        if(!aTriggered)
        {
            AudioManager.Instance.PlaySound("TriggerActivated");
            aTriggered = true;
        }

        dTriggered = false;
        animateElevator.SetBool("ElevatorSwitch", true);

        if(other.CompareTag("ChildCube"))
        {
            thrownChild.inSwitch = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(!dTriggered)
        {
            AudioManager.Instance.PlaySound("TriggerDeactivated");
            dTriggered = true;
        }

        aTriggered = false;
        StartCoroutine(DelayTrigger());

        if (other.CompareTag("ChildCube"))
        {
            thrownChild.inSwitch = false;
        }
    }

    IEnumerator DelayTrigger()
    {
        yield return new WaitForSeconds(5);

        animateElevator.SetBool("ElevatorSwitch", false);
    }
}
