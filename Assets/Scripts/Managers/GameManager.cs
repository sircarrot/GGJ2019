using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CarrotPack;

public class GameManager : MonoBehaviour, IManager
{
    [SerializeField] private CharacterController characterController;
    private GameState currentGameState = GameState.Platformer;
    private UIManager uiManager;

    public void InitializeManager()
    {
        uiManager = Toolbox.Instance.GetManager<UIManager>();
        uiManager.SetPlayerTransform(characterController.transform);
    }

    public void ChangeScene()
    {

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

