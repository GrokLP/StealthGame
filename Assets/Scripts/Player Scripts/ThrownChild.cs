using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ThrownChild : MonoBehaviour
{
    //resume jumping animation
    //show random dialogue response

    [SerializeField] public Animator childSoundRadius;
    [SerializeField] ChildEventComments childEventComments;

    //BoxCollider objectCollider;
    Quaternion startingRotation;
    Vector3 startingPosition;

    bool pickedUp;

    private void Start()
    {
        ThrowObject.ChildLanded += PlayAnimation;

        startingRotation = transform.rotation;
        startingPosition = transform.position;

        JumpAnimation();
    }



    void AnimationComplete()
    {
        childSoundRadius.SetBool("HasLanded", false);
        JumpAnimation();
    }

    void PlayAnimation()
    {
        transform.rotation = startingRotation;
        childEventComments.EndComment(); //clean up reference
        childSoundRadius.SetBool("HasLanded", true);
    }

    private void OnDestroy()
    {
        ThrowObject.ChildLanded -= PlayAnimation;
        DOTween.KillAll(true);
    }

    public void JumpAnimation()
    {
        if(!pickedUp)
        {
            transform.DOLocalJump(transform.position, 3, 200, 300, false);
        }
    }

    public void KillTween()
    {
        DOTween.KillAll(true);
    }
}
