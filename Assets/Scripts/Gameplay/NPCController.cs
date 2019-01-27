using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CarrotPack;

public class NPCController : MonoBehaviour {

    public Font npcFont;
    public string npcName;
    public int currentDialogueSequence = 0;
    public int loopSequenceStart = 4;

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Call On Trigger");
        Toolbox.Instance.GetManager<GameManager>().SetNPC(this);
    }

    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("Exit On Trigger");
        Toolbox.Instance.GetManager<GameManager>().RemoveNPC();
    }
}
