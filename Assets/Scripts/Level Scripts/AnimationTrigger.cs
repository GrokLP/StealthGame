using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTrigger : MonoBehaviour
{
    [SerializeField] Animator levelAnimator;

    private void OnTriggerEnter(Collider other)
    {
        levelAnimator.SetTrigger("WallSwitch");
    }

}
