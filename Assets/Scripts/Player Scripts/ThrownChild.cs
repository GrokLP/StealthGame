using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrownChild : MonoBehaviour
{
    //resume jumping animation
    //show random dialogue response

    [SerializeField] public Animator childSoundRadius;

    //BoxCollider objectCollider;
    Quaternion startingRotation;
    Vector3 startingPosition;

    private void Start()
    {
        ThrowObject.ChildLanded += PlayAnimation;
        //objectCollider = gameObject.GetComponent<BoxCollider>();

        startingRotation = transform.rotation;
        startingPosition = transform.position;
    }

    void AnimationComplete()
    {
        childSoundRadius.SetBool("HasLanded", false);

        //objectCollider.enabled = true;
    }

    void PlayAnimation()
    {
            transform.rotation = startingRotation;
            childSoundRadius.SetBool("HasLanded", true);

            //objectCollider.enabled = false;
    }

    private void OnDestroy()
    {
        ThrowObject.ChildLanded -= PlayAnimation;
    }
}
