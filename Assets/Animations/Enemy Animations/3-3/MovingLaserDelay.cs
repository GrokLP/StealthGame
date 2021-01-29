using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingLaserDelay : MonoBehaviour
{
    [SerializeField] Animator laserAnimator;

    public enum DelayTime
    {
        FIRST,
        SECOND,
        THIRD
    }

    [SerializeField] DelayTime delayTime;

    void Start()
    {
        StartCoroutine(PlayLaserAnimation());
    }

    IEnumerator PlayLaserAnimation()
    {
        switch(delayTime)
        {
            case DelayTime.FIRST:

                yield return new WaitForSeconds(0);
                laserAnimator.SetTrigger("Go");
                break;

            case DelayTime.SECOND:

                yield return new WaitForSeconds(2.5f);
                laserAnimator.SetTrigger("Go");
                break;

            case DelayTime.THIRD:

                yield return new WaitForSeconds(5f);
                laserAnimator.SetTrigger("Go");
                break;

        }
    }
}
