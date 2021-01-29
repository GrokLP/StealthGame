using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrownObjectSoundRadius : MonoBehaviour
{
    [SerializeField] Animator soundRadius;

    SphereCollider objectCollider;
    Quaternion startingRotation;
    Vector3 startingPosition;
    bool thisObject; //ensures only thrown object triggers animation if there are multiple throwable objects in scene

    private void Start()
    {
        ThrowObject.ObjectLanded += PlayAnimation;
        objectCollider = gameObject.GetComponent<SphereCollider>();

        startingRotation = transform.rotation;
        startingPosition = transform.position;
    }

    void AnimationComplete()
    {
        soundRadius.SetBool("HasLanded", false);
        thisObject = false;

        objectCollider.enabled = true;
    }

    void PlayAnimation()
    {
        if(thisObject)
        {
            transform.rotation = startingRotation;
            soundRadius.SetBool("HasLanded", true);

            objectCollider.enabled = false;
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            thisObject = true;
        }
    }

    private void OnDestroy()
    {
        ThrowObject.ObjectLanded -= PlayAnimation;
    }
}
