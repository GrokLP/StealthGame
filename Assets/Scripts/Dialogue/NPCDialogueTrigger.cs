using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NPCDialogueTrigger : MonoBehaviour
{
    [SerializeField] Animator dialogueIconAnimator;
    [SerializeField] GameObject dialogueUI;
    [SerializeField] GameObject dialogueIcon;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;

    public Dialogue dialogue;

    bool thisNPC;
    bool dialogueStarted;

    private void Start()
    {
        DialogueManager.OnEnterDialogue += EnableDialogueUI;
        DialogueManager.OnExitDialogue += DisableDialogueUI;
    }

    private void Update()
    {
        if(thisNPC && !DialogueManager.Instance.inDialogue && Input.GetButtonDown("Jump"))
        {
            TriggerDialogue();
            dialogueStarted = true;
        }
        else if(thisNPC | dialogueStarted && Input.GetButtonDown("Jump"))
        {
            DialogueManager.Instance.DisplayNextSentence(nameText, dialogueText);
        }
    }

    public void TriggerDialogue()
    {
        DialogueManager.Instance.StartDialogue(dialogue, nameText, dialogueText);
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            thisNPC = true;
            dialogueIconAnimator.SetBool("PlayerInRange", true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            thisNPC = false;
            dialogueIconAnimator.SetBool("PlayerInRange", false);
        }
    }

    void EnableDialogueUI()
    {
        if(thisNPC)
        {
            dialogueUI.SetActive(true);
            dialogueIcon.SetActive(false);

        }
    }

    void DisableDialogueUI()
    {
        if(thisNPC | dialogueStarted)
        {
            dialogueUI.SetActive(false);
            dialogueIcon.SetActive(true); //maybe put this in ontrigger exit so it's not confusing?
            dialogueStarted = false;
        }
    }

    private void OnDestroy()
    {
        DialogueManager.OnEnterDialogue -= EnableDialogueUI;
        DialogueManager.OnExitDialogue -= DisableDialogueUI;
    }
}
