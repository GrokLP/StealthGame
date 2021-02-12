using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallSwitch : MonoBehaviour
{
    [SerializeField] Animator wallAnimator;

    private void OnTriggerEnter(Collider other)
    {
        wallAnimator.SetTrigger("Trigger");
    }
}
