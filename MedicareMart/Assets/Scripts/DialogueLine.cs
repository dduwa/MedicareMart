using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueLine
{
    public string text;
    public DialogueOption[] options;  // Choices available at this point in the dialogue
}