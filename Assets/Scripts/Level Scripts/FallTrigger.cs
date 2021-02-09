using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallTrigger : MonoBehaviour
{
    public static event System.Action<string> OnGameLose;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player")) 
        {
            if (OnGameLose != null)
                OnGameLose("Fell");
        }

        else if(other.CompareTag("PushChildCube") | other.CompareTag("ChildCube"))
        {
            if (OnGameLose != null)
                OnGameLose("ChildFell");
        }
    }
}
