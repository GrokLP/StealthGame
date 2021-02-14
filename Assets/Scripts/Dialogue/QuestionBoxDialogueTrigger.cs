using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestionBoxDialogueTrigger : MonoBehaviour
{
    [SerializeField] GameObject dialogueUI;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;

    public Dialogue dialogue;

    bool thisNPC;

    private void Start()
    {
        DialogueManager.OnEnterDialogue += EnableDialogueUI;
        DialogueManager.OnExitDialogue += DisableDialogueUI;
    }

    private void Update()
    {
        //if (thisNPC && !DialogueManager.Instance.inDialogue && Input.GetButtonDown("Jump"))
        //{
        //    TriggerDialogue();
        //}
        if (thisNPC && DialogueManager.Instance.inDialogue && Input.GetButtonDown("Jump"))
        {
            DialogueManager.Instance.DisplayNextSentence(nameText, dialogueText);
        }
    }

    public void TriggerDialogue()
    {
        DialogueManager.Instance.StartDialogue(dialogue, nameText, dialogueText);
    }

    private void OnCollisionEnter(Collision other)
    {
        TriggerDialogue();
        thisNPC = true;
    }

    void EnableDialogueUI()
    {
        if (thisNPC)
        {
            dialogueUI.SetActive(true);
        }
    }

    void DisableDialogueUI()
    {
        if (thisNPC)
        {
            dialogueUI.SetActive(false);
            thisNPC = false;
        }
    }
    private void OnDestroy()
    {
        DialogueManager.OnEnterDialogue -= EnableDialogueUI;
        DialogueManager.OnExitDialogue -= DisableDialogueUI;
    }
}
