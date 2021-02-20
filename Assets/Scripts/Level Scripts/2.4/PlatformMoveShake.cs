using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMoveShake : MonoBehaviour
{

    [SerializeField] PlatformTrigger platformTrigger;
    private void Start()
    {
        PlatformTrigger.OnPlatformTrigger += DoCameraShake;
    }

    void DoCameraShake()
    {
        CameraShake.Instance.StartShake(7f, 0.1f);
        AudioManager.Instance.PlaySound("DoorOpen");
        StartCoroutine(StopSound());
    }

    IEnumerator StopSound()
    {
        yield return new WaitForSeconds(7f);

        AudioManager.Instance.StopSound("DoorOpen");
    }
}
