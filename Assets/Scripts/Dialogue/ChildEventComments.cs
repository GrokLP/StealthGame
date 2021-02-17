using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class ChildEventComments : MonoBehaviour
{
    public TextMeshProUGUI commentText;
    public ChildComments childComments;

    public void TriggerComment(string source)
    {
        string childType = source;
        DialogueManager.Instance.StartChildComment(childComments, commentText, childType);
    }

    public void EndComment()
    {
        DialogueManager.Instance.EndChildComment(commentText);
    }
}
