using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotationFourTwo : MonoBehaviour
{
    [SerializeField] Animator cameraAnimator;

    private void Start()
    {
        LaserTriggerPlatform.OnNextPlatform += OnNextPlatform;
    }

    void OnNextPlatform(bool platformDirection)
    {
        if(platformDirection)
        {
            cameraAnimator.SetTrigger("Next");
        }
        else if(!platformDirection)
        {
            cameraAnimator.SetTrigger("Previous");
        }
    }

    private void OnDestroy()
    {
        LaserTriggerPlatform.OnNextPlatform -= OnNextPlatform;
    }

}
