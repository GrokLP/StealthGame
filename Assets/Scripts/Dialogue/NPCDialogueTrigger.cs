using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NPCDialogueTrigger : MonoBehaviour
{
    [SerializeField] Animator dialogueIconAnimator;
    [SerializeField] GameObject dialogueUI;
    [SerializeField] GameObject dialogueIcon;
    [SerializeField] GameObject nextTextTriangle;
    [SerializeField] PlayerController playerController;

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;

    public Dialogue dialogue;
    //public Dialogue dialogueTwo; 

    public bool thisNPC;
    public bool dialogueStarted;
    int dialogueCount;

    private void Start()
    {
        DialogueManager.OnEnterDialogue += EnableDialogueUI;
        DialogueManager.OnExitDialogue += DisableDialogueUI;
        DialogueManager.DisplayNextTextTriangle += DisplayNextTextTriangle;
    }

    private void Update()
    {
        if(thisNPC && !DialogueManager.Instance.InDialogue && Input.GetButtonDown("Jump"))
        {
            TriggerDialogue();
            dialogueStarted = true;
            playerController.interactButtonIcon.SetActive(false);
        }
        else if(thisNPC | dialogueStarted && Input.GetButtonDown("Jump") && !DialogueManager.Instance.IsTyping)
        {
            nextTextTriangle.SetActive(false);
            DialogueManager.Instance.DisplayNextSentence(nameText, dialogueText);
        }
        else if(thisNPC | dialogueStarted && Input.GetButtonDown("Jump") && DialogueManager.Instance.IsTyping)
        {
            DialogueManager.Instance.Interrupt = true;
            nextTextTriangle.SetActive(false);
        }
    }

    public void TriggerDialogue()
    {
        dialogueCount++;

        //could do check here for if player has died/restarted (though maybe it's also important to check if they actually talked to NPC the first time)
        DialogueManager.Instance.StartDialogue(dialogue, nameText, dialogueText, dialogueCount);
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            thisNPC = true;
            
            if(dialogueCount < 3) //3 dialogue responses as of right now
                dialogueIconAnimator.SetBool("PlayerInRange", true);
            
            if(!dialogueStarted)
                playerController.interactButtonIcon.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            thisNPC = false;
            dialogueIconAnimator.SetBool("PlayerInRange", false);
            playerController.interactButtonIcon.SetActive(false);
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
            //playerController.interactButtonIcon.SetActive(true);
        }
    }

    private void OnDestroy()
    {
        DialogueManager.OnEnterDialogue -= EnableDialogueUI;
        DialogueManager.OnExitDialogue -= DisableDialogueUI;
        DialogueManager.DisplayNextTextTriangle -= DisplayNextTextTriangle;
    }

    private void DisplayNextTextTriangle()
    {
        nextTextTriangle.SetActive(true);
    }    
}
