using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCLoader : MonoBehaviour
{
    [SerializeField] NPCDialogueTrigger[] NPCVersions;

    private void Start()
    {
        if(NPCVersions.Length == 1 | NPCVersions.Length > 1 && GameManager.Instance.LevelAttempts == 1)
        {
            NPCVersions[0].gameObject.SetActive(true);
        }
        else if(GameManager.Instance.LevelAttempts > 1)
        {
            if(GameManager.Instance.LevelAttempts-1 < NPCVersions.Length) 
            {
                NPCVersions[GameManager.Instance.LevelAttempts - 1].gameObject.SetActive(true); //could i change this so it is just enabling a script?
            }
            else
            {
                NPCVersions[NPCVersions.Length -1].gameObject.SetActive(true);
            }
        }
    }
}


