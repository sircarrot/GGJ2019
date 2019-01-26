using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CarrotPack;

public class DialogueManager : MonoBehaviour, IManager
{
    [SerializeField] private TextAsset DialogueCSV;
    private Dictionary<string, string> dialogueDictionary = new Dictionary<string, string>();

    private bool dialogueProgress = false;
    private bool displayingText = false;

    private void PopulateDictionary()
    {
        if (DialogueCSV == null)
        {
            Debug.LogError("No CSV File found!");
            return;
        }


        string[] records = DialogueCSV.text.Split('\n');
        int recordsSize = records.Length;

        foreach(string record in records)
        {
            string[] data = record.Split(',');

            string id = data[0].Trim(' ', ',');
            string dialogue = "";
            for(int i = 1; i < data.Length; ++i)
            {
                dialogue += data[i];
            }
            dialogue = dialogue.Trim('"', ' ', '\r');

            dialogueDictionary.Add(id, dialogue);
        }
    }

    public void InitializeManager()
    {
        PopulateDictionary();
    }    

    public string GetDialogue(string id)
    {
        if(dialogueDictionary.ContainsKey(id))
        {
            return dialogueDictionary[id];
        }
        else
        {
            return "";
        }

    }
}
