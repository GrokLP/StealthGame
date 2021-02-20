using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatformShake : MonoBehaviour
{
    float shakePower;

    public void DoCameraShake()
    {
        shakePower += 0.1f;
        CameraShake.Instance.StartShake(0.2f, shakePower);
        AudioManager.Instance.PlaySound("FallingPlatform");
    }
}
