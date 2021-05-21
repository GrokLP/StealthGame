using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonTrigger : MonoBehaviour
{   
    //make prefab and add buttons where applicable
    
    [SerializeField] Animator triggerAnimator;
    [SerializeField] float buttonHeight;

    Vector3 startingHeight;
    Vector3 pressedHeight;

    bool pressed;

    private void Start()
    {
        startingHeight = transform.position;
        pressedHeight = new Vector3(startingHeight.x, startingHeight.y - buttonHeight + 0.05f, startingHeight.z);
    }

    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.CompareTag("Player") | (other.gameObject.CompareTag("ChildCube")))
        {
            if (!pressed)
            {
                StartCoroutine(ButtonPressed());
                triggerAnimator.SetTrigger("Trigger"); //this would be better after button moves
                AudioManager.Instance.PlaySound("ButtonSwitch");
                pressed = true;
            }
        }
 
    }

    IEnumerator ButtonPressed()
    {
        yield return new WaitForSeconds(0.3f);

        CameraShake.Instance.StartShake(0.2f, 0.1f);

        while (startingHeight != pressedHeight)
        {
            transform.position = Vector3.MoveTowards(transform.position, pressedHeight, 2f * Time.deltaTime);
            yield return null;
        }

        Debug.Log("test"); //this is never reached
    }
}
