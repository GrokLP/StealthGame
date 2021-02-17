using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelOneFourDialogue : MonoBehaviour
{
    [SerializeField] NPCDialogueTrigger npcDialogueTrigger;
    bool triggered;

    private void OnTriggerEnter(Collider other)
    {
        if(GameManager.Instance.LevelAttempts <= 1 && !triggered)
        {
            npcDialogueTrigger.thisNPC = true;
            npcDialogueTrigger.TriggerDialogue();
            npcDialogueTrigger.thisNPC = false;
            npcDialogueTrigger.dialogueStarted = true;
            triggered = true;
        }
     }
}
