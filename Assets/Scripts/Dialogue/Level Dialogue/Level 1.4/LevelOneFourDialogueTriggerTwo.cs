﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelOneFourDialogueTriggerTwo : MonoBehaviour
{
    [SerializeField] NPCDialogueTrigger npcDialogueTrigger;
    bool triggered;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && GameManager.Instance.LevelAttempts >= 2 && !triggered)
        {
            npcDialogueTrigger.thisNPC = true;
            npcDialogueTrigger.TriggerDialogue();
            npcDialogueTrigger.thisNPC = false;
            npcDialogueTrigger.dialogueStarted = true;
            triggered = true;
        }
    }
}
