using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrownChild : MonoBehaviour
{
    //resume jumping animation
    //show random dialogue response

    [SerializeField] public Animator childSoundRadius;
    [SerializeField] ChildEventComments childEventComments;

    //BoxCollider objectCollider;
    Quaternion startingRotation;
    Vector3 startingPosition;

    private void Start()
    {
        ThrowObject.ChildLanded += PlayAnimation;

        startingRotation = transform.rotation;
        startingPosition = transform.position;
    }

    void AnimationComplete()
    {
        childSoundRadius.SetBool("HasLanded", false);
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
    }
}
