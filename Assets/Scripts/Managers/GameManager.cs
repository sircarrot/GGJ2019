using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CarrotPack;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour, IManager
{
    [SerializeField] private CharacterController characterController;
    private GameState currentGameState = GameState.Platformer;
    private UIManager uiManager;

    [Header("Talk to NPC")]
    private DialogueManager dialogueManager;
    private NPCController npcInRange = null;
    private int lineNumber = 1;

    public void InitializeManager()
    {
        dialogueManager = Toolbox.Instance.GetManager<DialogueManager>();
        uiManager = Toolbox.Instance.GetManager<UIManager>();

        uiManager.SetPlayerTransform(characterController.transform);
        uiManager.UpdateNPCList(FindAllNPC());
        SceneManager.sceneLoaded += (Scene, loadSceneMode) => 
        {
            uiManager.UpdateNPCList(FindAllNPC());
        };

        DontDestroyOnLoad(characterController.gameObject);
    }

    public List<Transform> FindAllNPC()
    {
        GameObject[] objectArray = GameObject.FindGameObjectsWithTag("NPC");
        List<Transform> transformList = new List<Transform>();

        foreach(GameObject objectTag in objectArray)
        {
            transformList.Add(objectTag.transform);
        }

        Debug.Log("NPC Size list: " + transformList.Count);
        return transformList;
    }

    public void ChangeScene(string targetScene, string targetAnchor)
    {
        //fade in
        uiManager.FadeScreenTransition(() =>
        {
            SceneManager.LoadScene(targetScene);
            this.RunOnNextUpdate(() => 
            {
                characterController.transform.position = GameObject.Find(targetAnchor).transform.position;
            });
        });
    }

    public void ReceiveInput(KeyCode keyCode, bool isKeyDown = false)
    {
        if (currentGameState == GameState.Dialogue)
        {
            //string dialogue = TryToGetDialogue();
            //if(dialogue != "")
            //{
            //    uiManager.NextDialogue(dialogue);
            //}
            //else
            //{
            //    EndTalkToNPC();
            //}
            TalkToNPC();
            return;
        }

        switch (keyCode)
        {
            case KeyCode.A:
                characterController.MoveCharacter(MoveDirection.Left);
                break;

            case KeyCode.D:
                characterController.MoveCharacter(MoveDirection.Right);
                break;

            case KeyCode.Space:
                if(isKeyDown)
                {
                    characterController.Jump();
                }
                break;

                //Interact
            case KeyCode.F:
                if(npcInRange != null)
                {
                    TalkToNPC();
                }
                break;
        }

    }

    public void SetNPC(NPCController npc)
    {
        npcInRange = npc;
    }

    public void RemoveNPC()
    {
        npcInRange = null;
    }

    public void TalkToNPC()
    {
        Debug.Log(npcInRange);

        string dialogue = TryToGetDialogue();

        if (dialogue == "")
        {
            EndTalkToNPC();
            return;
        }

        if (currentGameState == GameState.Dialogue)
        {
            uiManager.NextDialogue(dialogue);

        }
        else
        {
            uiManager.OpenDialogue(dialogue);
            currentGameState = GameState.Dialogue;
        }

        lineNumber++;
    }

    public void EndTalkToNPC()
    {
        uiManager.CloseDialogue();
        lineNumber = 1;
        npcInRange.currentDialogueSequence++;
        currentGameState = GameState.Platformer;
    }

    public string TryToGetDialogue(bool repeat = false)
    {
        int currentDialogueSequence = npcInRange.currentDialogueSequence;
        if (repeat)
        {
            currentDialogueSequence = npcInRange.loopSequenceStart;
            npcInRange.currentDialogueSequence = npcInRange.loopSequenceStart;
        }

        string dialogueID = npcInRange.npcName + "_" + currentDialogueSequence.ToString() + "_" + lineNumber;

        string dialogue = dialogueManager.GetDialogue(dialogueID);
        if (repeat) { return dialogue; }
        if (dialogue == "")
        {
            // End of a sequence
            if(lineNumber != 1)
            {
                return "";
            }

            // Go back to first loop
            dialogue = TryToGetDialogue(true);

            if(dialogue == "")
            {
                Debug.Log("Dialogue id doesn't exist: " + dialogueID);
            }

            return dialogue;
        }

        return dialogue;
    }

    public enum GameState
    {
        Platformer,
        Dialogue
    }

}

