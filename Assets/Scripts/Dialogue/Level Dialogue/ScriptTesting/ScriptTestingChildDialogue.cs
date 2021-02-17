using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptTestingChildDialogue : MonoBehaviour
{
    [SerializeField] ChildDialogueScript childDialogueScript;
    bool triggered;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && GameManager.Instance.LevelAttempts <=1 && !triggered)
        {
            childDialogueScript.TriggerDialogue();
            childDialogueScript.dialogueStarted = true;
            triggered = true;
        }
    }
}
