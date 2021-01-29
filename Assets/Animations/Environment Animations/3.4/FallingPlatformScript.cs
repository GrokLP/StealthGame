using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatformScript : MonoBehaviour
{
    [SerializeField] Animator FallingPlatforms;

    [SerializeField] PlatformNumber platformNumber;
    enum PlatformNumber
    {
        COLORSWITCH,
        ONE,
        TWO,
        THREE,
        FOUR
    }

    private void OnCollisionEnter(Collision collision)
    {
        switch(platformNumber)
        {
            case PlatformNumber.COLORSWITCH:
                FallingPlatforms.SetTrigger("ColorSwitch");
                break;
            case PlatformNumber.ONE:
                FallingPlatforms.SetTrigger("PlatformOne");
                break;
            case PlatformNumber.TWO:
                FallingPlatforms.SetTrigger("PlatformTwo");
                break;
            case PlatformNumber.THREE:
                FallingPlatforms.SetTrigger("PlatformThree");
                break;
            case PlatformNumber.FOUR:
                FallingPlatforms.SetTrigger("PlatformFour");
                break;
        }
    }
}
