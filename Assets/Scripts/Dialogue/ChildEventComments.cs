using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChildEventComments : MonoBehaviour
{
    public TextMeshProUGUI commentText;
    public ChildComments childComments;

    public void TriggerComment()
    {
        DialogueManager.Instance.StartChildComment(childComments, commentText);
    }

    public void EndComment()
    {
        DialogueManager.Instance.EndChildComment(commentText);
    }


}
