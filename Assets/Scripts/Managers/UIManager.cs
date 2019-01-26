using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CarrotPack;

public class UIManager : MonoBehaviour, IManager {

    [SerializeField] private GameObject UIPrefab;

    private Transform CanvasTransform;
    private Transform playerTransform;
    private List<Transform> npcList = new List<Transform>();

    public void InitializeManager()
    {
        CanvasTransform = Instantiate(UIPrefab, gameObject.transform).transform;
    }

    public void SetPlayerTransform(Transform playerTransform)
    {
        this.playerTransform = playerTransform;
    }
	
	// Update is called once per frame
	void Update () {

        Debug.Log(GetClosestNPC());


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
