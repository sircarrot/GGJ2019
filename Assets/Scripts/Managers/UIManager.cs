﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CarrotPack;

public class UIManager : MonoBehaviour, IManager {

    [SerializeField] private GameObject UIPrefab;

    private Transform cameraTransform;
    private Transform CanvasTransform;
    private Transform playerTransform;
    private List<Transform> npcList = new List<Transform>();
    private static float cameraSpeed = 0.1f; //0-1, 1 mean instant

    public void InitializeManager()
    {
        CanvasTransform = Instantiate(UIPrefab, gameObject.transform).transform;
        this.CentralizerCamera();
    }

    public void SetPlayerTransform(Transform playerTransform)
    {
        this.playerTransform = playerTransform;
    }
	
	// Update is called once per frame
	void Update () 
    {
        Debug.Log(GetClosestNPC());
	}

    void FixedUpdate()
    {
        this.UpdateCamera();
    }

    private void UpdateCamera()
    {
        this.cameraTransform.position = this.playerTransform.position * UIManager.cameraSpeed + this.cameraTransform.position * (1 - UIManager.cameraSpeed);
        this.cameraTransform.position = this.cameraTransform.position.WithZ(-10f);
    }
    public void CentralizerCamera()
    {
        this.cameraTransform = GameObject.Find("Main Camera").transform;
        this.cameraTransform.position = this.playerTransform.position.WithZ(-10f);
    }

    public void UpdateNPCList(List<Transform> npcList)
    {
        this.npcList = new List<Transform>(npcList);
        Debug.Log("NPC Size list: " + this.npcList.Count);

    }

    public Vector3 GetClosestNPC()
    {
        Vector3 distanceVector = Vector3.zero;
        bool initialized = false;
        foreach(Transform npc in npcList)
        {
            if (!initialized)
            {
                distanceVector = npc.position - playerTransform.position;
                initialized = true;
            }
            else
            {
                Vector3 tempDistanceVector = npc.position - playerTransform.position;
                if(tempDistanceVector.magnitude < distanceVector.magnitude)
                {
                    distanceVector = tempDistanceVector;
                }
            }
        }

        return distanceVector;
    }

}
