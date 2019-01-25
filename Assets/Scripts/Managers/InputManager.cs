using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CarrotPack;

public class InputManager : MonoBehaviour, IManager {

    public GameManager gameManager;

    public void InitializeManager()
    {
    }
    

	// Update is called once per frame
	void Update ()
    {
		if(Input.GetKey(KeyCode.Space))
        {

        }

        if(Input.GetKey(KeyCode.A))
        {

        }

        if (Input.GetKey(KeyCode.D))
        {

        }

        if (Input.GetKey(KeyCode.F)) // Interact
        {

        }
    }
}
