using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NPCDialogueTrigger : MonoBehaviour
{
    [SerializeField] Animator dialogueIconAnimator;
    [SerializeField] Animator jumpAnimator;
    [SerializeField] GameObject dialogueUI;
    [SerializeField] GameObject dialogueIcon;
    [SerializeField] GameObject nextTextTriangle;
    [SerializeField] PlayerController playerController;

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;

    public Dialogue dialogue;

    public bool thisNPC;
    public bool dialogueStarted;
    int dialogueCount;

    bool stopAnimation;

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
            AudioManager.Instance.PlaySound("ConversationNext");
            TriggerDialogue();
            dialogueStarted = true;
            playerController.interactButtonIcon.SetActive(false);
        }
        else if(thisNPC | dialogueStarted && Input.GetButtonDown("Jump") && !DialogueManager.Instance.IsTyping)
        {
            AudioManager.Instance.PlaySound("ConversationNext");
            nextTextTriangle.SetActive(false);
            DialogueManager.Instance.DisplayNextSentence(nameText, dialogueText);
        }
        else if(thisNPC | dialogueStarted && Input.GetButtonDown("Jump") && DialogueManager.Instance.IsTyping)
        {
            //AudioManager.Instance.PlaySound("ConversationNext");
            DialogueManager.Instance.Interrupt = true;
            nextTextTriangle.SetActive(false);
        }
    }

    public void TriggerDialogue()
    {
        
        DialogueManager.Instance.StartDialogue(dialogue, nameText, dialogueText, dialogueCount);

        if(dialogueCount < dialogue.responseList.Length - 1)
        {
            dialogueCount++; //moved this under method call because using arrays that start at 0
        }
        else
        {
            stopAnimation = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            thisNPC = true;

            jumpAnimator.SetBool("InDialogue", true);

            if (!stopAnimation)
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
            
            if(!stopAnimation)
                jumpAnimator.SetBool("InDialogue", false);
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
            dialogueIcon.SetActive(true);
            dialogueStarted = false;
        }
    }

    private void DisplayNextTextTriangle()
    {
        nextTextTriangle.SetActive(true);
    }

    private void OnDestroy()
    {
        DialogueManager.OnEnterDialogue -= EnableDialogueUI;
        DialogueManager.OnExitDialogue -= DisableDialogueUI;
        DialogueManager.DisplayNextTextTriangle -= DisplayNextTextTriangle;

        //if(AudioManager.Instance.StopSound() != null)
        AudioManager.Instance.StopSound("Typing"); //prevents bug where typing sound persists on reload in 1.4 if player reloads while inside NPC talk range
    }
}
