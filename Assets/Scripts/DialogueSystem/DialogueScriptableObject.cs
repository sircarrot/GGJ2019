using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/DialogueScriptableObject")]
public class DialogueScriptableObject : ScriptableObject {

    public List<Dialogue> dialogueList = new List<Dialogue>();

    public Dictionary<string, string> dialogueDictionary = new Dictionary<string, string>();

    public void InitializeDictionary()
    {
        foreach(Dialogue dialogue in dialogueList)
        {
            if(dialogue.id == "" || dialogue.id == null)
            {
                Debug.LogWarning("No dialogue id!");
                continue;
            }
            dialogueDictionary.Add(dialogue.id, dialogue.dialogue);
        }
    }
}

[System.Serializable]
public class Dialogue
{
    public string id;
    public string dialogue;
}