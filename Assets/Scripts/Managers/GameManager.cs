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
    private List<NPCController> npcInRangeList = new List<NPCController>();
    private InteractType interactType = InteractType.None;
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
            switch(keyCode)
            {
                case KeyCode.F:
                case KeyCode.Space:
                    TalkToNPC();
                    break;
            }
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
                switch(interactType)
                {
                    case InteractType.SingleNPC:
                    //case InteractType.MultipleNPC:
                        if (npcInRange != null)
                            TalkToNPC();
                        break;
                }
                break;
        }

    }

    public void SetNPC(NPCController npc)
    {
        npcInRange = npc;
        interactType = InteractType.SingleNPC;
    }

    public void RemoveNPC()
    {
        npcInRange = null;
        interactType = InteractType.None;
    }

    public void TalkToNPC()
    {
        string dialogue = TryToGetDialogue();

        Debug.Log(dialogue);
        Debug.Log(currentGameState);

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
            uiManager.OpenDialogue(dialogue, npcInRange.transform);
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
            if (currentDialogueSequence < npcInRange.loopSequenceStart) { return ""; }
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

public enum InteractType
{
    None,
    SingleNPC,
    MultipleNPC
}
