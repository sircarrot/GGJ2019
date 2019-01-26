using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CarrotPack;

public class NPCController : MonoBehaviour {

    public string npcName;
    public int currentDialogueSequence = 0;
    public int loopSequenceStart = 4;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Toolbox.Instance.GetManager<GameManager>().SetNPC(this);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Toolbox.Instance.GetManager<GameManager>().RemoveNPC();
    }
}
