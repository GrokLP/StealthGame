using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch1 : MonoBehaviour
{
    [SerializeField] Animator switch1;

    private void OnTriggerExit(Collider other)
    {
        switch1.SetTrigger("Switch");
    }

}
