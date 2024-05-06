using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueOption
{
    public string text;
    public int scoreImpact;  // Impact on the player's score
    public DialogueLine[] nextLines;  // Next set of dialogue lines after this choice
}