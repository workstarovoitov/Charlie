using UnityEngine;
using UnityEngine.Events;
using System;

public class ControllerTalker : UsableObject, IUsable
{
    [SerializeField] private DialogueItem[] dialogue;
    private int dialogeNum = 0;
    public int DialogueNum
    {
        get => dialogeNum;
        set => dialogeNum = value;
    }
    public DialogueItem[] Dialogue
    {
        get => dialogue;
        set => dialogue = value;
    }
    public static event Action<ControllerTalker> OnDialogueStart;

    public UnityEvent[] OnFinishDialogue;

    public new void Start() { }

    public new void Use()
    {
        OnDialogueStart?.Invoke(this);
    }

    public void FinishDialogue()
    {
        OnFinishDialogue[dialogeNum].Invoke();
    }

    public void NextDialogue()
    {
        dialogeNum++;
    }
    
    public void SetDialogue(int value)
    {
        if (value < dialogue.Length)
        {
            dialogeNum = value;
            return;
        }
        Debug.LogWarning("No such replic");
    }
}