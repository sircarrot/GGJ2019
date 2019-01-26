using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CarrotPack;

public class InputManager : MonoBehaviour, IManager {

    public GameManager gameManager;

    public void InitializeManager()
    {
        if (gameManager == null) { gameManager = Toolbox.Instance.GetManager<GameManager>(); }
    }
    
	// Update is called once per frame
	void Update ()
    {
		if(Input.GetKey(KeyCode.Space))
        {
            gameManager.ReceiveInput(KeyCode.Space);
        }

        if(Input.GetKey(KeyCode.A))
        {
            gameManager.ReceiveInput(KeyCode.A);
        }

        if (Input.GetKey(KeyCode.D))
        {
            gameManager.ReceiveInput(KeyCode.D);
        }

        if (Input.GetKey(KeyCode.F)) // Interact
        {
            gameManager.ReceiveInput(KeyCode.F);
        }
    }
}
