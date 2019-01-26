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
        //fadeImage.enabled = true;
        //this.Chain().TweenLinear((a) => 
        //{
        //    fadeImage.color = Color.Lerp(Color.clear, Color.black, a);
        //}, GameManager.fadingTime, 0f, 1f).Run(() => 
        //{
        //    SceneManager.LoadScene(target);
        //}).TweenLinear((a) => 
        //{
        //    fadeImage.color = Color.Lerp(Color.black, Color.clear, a);
        //}, GameManager.fadingTime, 0f, 1f).Run(() => 
        //{
        //    fadeImage.enabled = false;
        //}).Start();
    }

    public void ReceiveInput(KeyCode keyCode)
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
                characterController.Jump();
                break;

                //Interact
            case KeyCode.F:
                break;
        }

    }

    public enum GameState
    {
        Platformer,
        Dialogue
    }

}

