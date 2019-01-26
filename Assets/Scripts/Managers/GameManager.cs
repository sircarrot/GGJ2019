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
    private NPCController npcInRange = null;

    public void InitializeManager()
    {
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
        if (currentGameState != GameState.Platformer) return;
        
        switch(keyCode)
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
                    Debug.Log(npcInRange.npcName);
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

    public enum GameState
    {
        Platformer,
        Dialogue
    }

}

