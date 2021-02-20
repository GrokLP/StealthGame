using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelTwoOneDialogue : MonoBehaviour
{
    [SerializeField] GameObject dialogueUI;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;

    public Dialogue dialogue;

    bool thisNPC;
    bool triggered;
    int dialogueCount;

    private void Start()
    {
        DialogueManager.OnEnterDialogue += EnableDialogueUI;
        DialogueManager.OnExitDialogue += DisableDialogueUI;
    }

    private void Update()
    {
        if (thisNPC && DialogueManager.Instance.InDialogue && Input.GetButtonDown("Jump"))
        {
            DialogueManager.Instance.DisplayNextSentence(nameText, dialogueText);
        }
    }

        public void TriggerDialogue()
        {
            DialogueManager.Instance.StartDialogue(dialogue, nameText, dialogueText, dialogueCount);
            if (dialogueCount < dialogue.responseList.Length - 1)
            {
                dialogueCount++; //moved this under method call because using arrays that start at 0
            }
        }

        private void OnTriggerEnter(Collider collider)
        {
            if (collider.CompareTag("Player") && GameManager.Instance.LevelAttempts <= 1 && !triggered)
            {
                thisNPC = true;
                TriggerDialogue();
                triggered = true;
            }

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
