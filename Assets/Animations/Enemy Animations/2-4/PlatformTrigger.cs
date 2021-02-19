using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformTrigger : MonoBehaviour
{
    public static event System.Action OnPlatformTrigger;

    [SerializeField] Animator animationTrigger;
    [SerializeField] Animator animationTriggerTwo;
    MovingPlatformScript platformScript;

    private void Start()
    {
        platformScript = FindObjectOfType<MovingPlatformScript>();
    }

    private void OnTriggerEnter(Collider other)
    {
        AudioManager.Instance.PlaySound("TriggerActivated");

        if(platformScript.Trigger == false)
        {
            OnPlatformTrigger();
            animationTrigger.SetTrigger("CameraTrigger");
            animationTriggerTwo.SetTrigger("CameraTrigger");
        }
    }
}
