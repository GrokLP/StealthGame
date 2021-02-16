﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueManager : Singleton<DialogueManager>
{
    public static event System.Action OnEnterDialogue;
    public static event System.Action OnExitDialogue;
    public static event System.Action DisplayNextTextTriangle;

    [SerializeField] BetterJump betterJump;
    [SerializeField] ThrowObject throwObject;
    [SerializeField] PushObject pushObject;
    [SerializeField] ChangeColor changeColor;

    Queue<string> sentences;

    bool inDialogue;
    public bool InDialogue
    {
        get { return inDialogue; }
    }

    bool isTyping;
    public bool IsTyping
    {
        get { return isTyping; }
    }
    
    bool interrupt;
    public bool Interrupt
    {
        set { interrupt = value; }
    }

    void Start()
    {
        sentences = new Queue<string>();
    }

    public void StartDialogue(Dialogue dialogue, TextMeshProUGUI nameText, TextMeshProUGUI dialogueText, int dialogueCount)
    {        
        if (OnEnterDialogue != null)
            OnEnterDialogue();

        betterJump.IsActive = false;
        throwObject.IsActive = false;
        pushObject.IsActive = false;

        inDialogue = true;
        
        nameText.text = dialogue.name;
        
        sentences.Clear();

        foreach (string sentence in dialogue.responseList[dialogueCount].sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence(nameText, dialogueText);
    }

    public void DisplayNextSentence(TextMeshProUGUI nameText, TextMeshProUGUI dialogueText)
    {
        if(sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        dialogueText.text = sentence;
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence, dialogueText));
    }

    IEnumerator TypeSentence(string sentence, TextMeshProUGUI dialogueText)
    {
        dialogueText.ForceMeshUpdate(); //is this performant??
        int totalVisibleCharacters = dialogueText.textInfo.characterCount;
        int counter = 0;

        bool loopFinished = false;

        interrupt = false;

        while(!loopFinished)
        {
            isTyping = true;
            int visibleCount = counter % (totalVisibleCharacters + 1);

            dialogueText.maxVisibleCharacters = visibleCount;

            if (visibleCount >= totalVisibleCharacters)
            {
                loopFinished = true;
                isTyping = false;
                if (DisplayNextTextTriangle != null)
                    DisplayNextTextTriangle();
            }
            else if (interrupt)
            {
                dialogueText.maxVisibleCharacters = totalVisibleCharacters;
                isTyping = false;
                if (DisplayNextTextTriangle != null)
                    DisplayNextTextTriangle();
            }

            counter += 1;
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

    public void StartChildComment(ChildComments comments, TextMeshProUGUI commentText)
    {
        int commentIndex = Random.Range(0, comments.commentPool.Length);
        commentText.text = comments.commentPool[commentIndex];

        StartCoroutine(TypeComment(commentText));

        //animate text?
        //randomize how often comments appear
    }

    IEnumerator TypeComment(TextMeshProUGUI commentText)
    {
        commentText.ForceMeshUpdate(); //is this performant??
        int totalVisibleCharacters = commentText.textInfo.characterCount;
        int counter = 0;

        bool loopFinished = false;

        while (!loopFinished)
        {
            int visibleCount = counter % (totalVisibleCharacters + 1);

            commentText.maxVisibleCharacters = visibleCount;

            if (visibleCount >= totalVisibleCharacters)
            {
                loopFinished = true;
            }

            counter += 1;
            yield return new WaitForSeconds(0.03f);
        }
    }

    public void EndChildComment(TextMeshProUGUI commentText)
    {
        commentText.text = "";
    }
}
