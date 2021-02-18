using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChildDialogueScript : MonoBehaviour
{
    //triggered by event
    //can continue converation with a button

    [SerializeField] GameObject dialogueUI;
    [SerializeField] GameObject nextTextTriangle;

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;
    [SerializeField] PlayerController playerController;

    public ChildDialogue childDialogue;

    public bool dialogueStarted;

    bool thisChild;

    private void Start()
    {
        DialogueManager.OnEnterDialogue += EnableDialogueUI;
        DialogueManager.OnExitDialogue += DisableDialogueUI;
        DialogueManager.DisplayNextTextTriangle += DisplayNextTextTriangle;
    }

    private void Update()
    {
        if (dialogueStarted && Input.GetButtonDown("Jump") && !DialogueManager.Instance.IsTyping)
        {
            AudioManager.Instance.StopSound("ConversationNext");
            nextTextTriangle.SetActive(false);
            DialogueManager.Instance.DisplayNextSentence(nameText, dialogueText);
        }
        else if (dialogueStarted && Input.GetButtonDown("Jump") && DialogueManager.Instance.IsTyping)
        {
            AudioManager.Instance.StopSound("ConversationNext");
            DialogueManager.Instance.Interrupt = true;
            nextTextTriangle.SetActive(false);
        }
    }
    public void TriggerDialogue()
    {
        dialogueStarted = true;
        thisChild = true;
        DialogueManager.Instance.StartChildDialogue(childDialogue, nameText, dialogueText);
    }

    public void EnableDialogueUI()
    {
        if(thisChild)
        {
            dialogueUI.SetActive(true);
        }

    }

    public void DisableDialogueUI()
    {
        if(thisChild)
        {
            dialogueUI.SetActive(false);
            dialogueStarted = false;
            thisChild = false;
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
    }
}
