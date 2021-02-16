using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NPCResponses
{
    [TextArea(3, 10)]
    public string[] sentences;
}

[System.Serializable]
public class Dialogue
{
    public string name;

    public NPCResponses[] responseList;
}
