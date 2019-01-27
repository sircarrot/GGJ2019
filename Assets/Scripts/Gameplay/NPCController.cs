using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CarrotPack;

public class NPCController : MonoBehaviour {

    public Font npcFont;
    public string npcName;
    public int currentDialogueSequence = 0;
    public int loopSequenceStart = 4;

    private Animator animator;

    private void Start()
    {
        this.animator = this.GetComponent<Animator>();
        this.StopAnimator();
    }

    public void StartAnimator()
    {
        if(this.animator != null)
        {
            this.animator.speed = 1f;
        }
    }
    public void StopAnimator()
    {
        if(this.animator != null)
        {
            this.animator.speed = 0f;
        }
    }

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
