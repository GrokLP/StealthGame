using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueManager : Singleton<DialogueManager>
{
    public static event System.Action OnEnterDialogue;
    public static event System.Action OnExitDialogue;

    [SerializeField] BetterJump betterJump;
    [SerializeField] ThrowObject throwObject;
    [SerializeField] PushObject pushObject;
    [SerializeField] ChangeColor changeColor;

    Queue<string> sentences;

    public bool inDialogue;
    int dialogueCount;

    void Start()
    {
        sentences = new Queue<string>();
    }

    public void StartDialogue(Dialogue dialogue, TextMeshProUGUI nameText, TextMeshProUGUI dialogueText)
    {
        dialogueCount++;//this won't work with multiple NPCs
        
        if (OnEnterDialogue != null)
            OnEnterDialogue();

        betterJump.IsActive = false;
        throwObject.IsActive = false;
        pushObject.IsActive = false;

        inDialogue = true;
        
        nameText.text = dialogue.name;
        
        sentences.Clear();

        switch (dialogueCount)
        {
            case (1):

                foreach (string sentence in dialogue.firstSentences)
                {
                    sentences.Enqueue(sentence);
                }

                DisplayNextSentence(nameText, dialogueText);

                break;

            case (2):

                foreach (string sentence in dialogue.secondSentences)
                {
                    sentences.Enqueue(sentence);
                }

                DisplayNextSentence(nameText, dialogueText);

                break;

            default:

                foreach (string sentence in dialogue.thirdSentences)
                {
                    sentences.Enqueue(sentence);
                }

                DisplayNextSentence(nameText, dialogueText);

                break;
        }
    }

    public void DisplayNextSentence(TextMeshProUGUI nameText, TextMeshProUGUI dialogueText)
    {
        if(sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence, dialogueText));
    }

    IEnumerator TypeSentence(string sentence, TextMeshProUGUI dialogueText)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(0.03f);
        }
    }

    public void EndDialogue()
    {
        if (OnExitDialogue != null)
            OnExitDialogue();

        switch(changeColor.CurrentPlayerColor)
        {
            case ChangeColor.PlayerColor.BLUE:
                betterJump.IsActive = true;
                break;
            case ChangeColor.PlayerColor.RED:
                throwObject.IsActive = true;
                break;
            case ChangeColor.PlayerColor.GREEN:
                pushObject.IsActive = true;
                break;
        }

        inDialogue = false;
    }
}
