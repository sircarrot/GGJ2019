using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CarrotPack;

public class UIManager : MonoBehaviour, IManager {

    [SerializeField] private GameObject UIPrefab;

    private UIComponents uiComponents;

    [Header("Fade Effect")]
    [SerializeField] private static float fadingTime = 0.5f;

    private Camera cam;
    private Transform cameraTransform;


    private Transform CanvasTransform;
    private Transform playerTransform;
    private List<Transform> npcList = new List<Transform>();
    private static float cameraSpeed = 0.1f; //0-1, 1 mean instant

    public void InitializeManager()
    {
        CanvasTransform = Instantiate(UIPrefab, gameObject.transform).transform;
        uiComponents = CanvasTransform.GetComponent<UIComponents>();
        CentralizerCamera();
         
        DontDestroyOnLoad(this.cameraTransform.gameObject);
        DontDestroyOnLoad(this.CanvasTransform.gameObject);
    }

    public void SetPlayerTransform(Transform playerTransform)
    {
        this.playerTransform = playerTransform;
    }
	
	// Update is called once per frame
	void Update () 
    {
        UpdateArrowPosition();
    }

    void FixedUpdate()
    {
        UpdateCamera();
    }

    private void InitializeCamera()
    {
        if (cam == null) { cam = GameObject.Find("Main Camera").GetComponent<Camera>(); }
        cameraTransform = cam.transform;
    }

    private void UpdateCamera()
    {
        cameraTransform.position = playerTransform.position * UIManager.cameraSpeed + cameraTransform.position * (1 - UIManager.cameraSpeed);
        cameraTransform.position = cameraTransform.position.WithZ(-10f);
    }

    public void CentralizerCamera()
    {
        InitializeCamera();
        cameraTransform.position = playerTransform.position.WithZ(-10f);
    }

    public void UpdateArrowPosition()
    {
        Transform npc = GetClosestNPC();
        if (npc == null)
        {
            uiComponents.npcArrow.gameObject.SetActive(false);
            return;
        }
        Vector3 screenPoint = cam.WorldToViewportPoint(npc.position);
        if (screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1)
        {
            uiComponents.npcArrow.gameObject.SetActive(false);
            return;
        }

        uiComponents.npcArrow.gameObject.SetActive(true);
        Vector3 direction = (npc.position - playerTransform.position).normalized;
        //uiComponents.npcArrow.transform.position = cam.WorldToScreenPoint(playerTransform.position);
        //uiComponents.npcArrow.transform.position += direction * 500;

        float multiplier = Mathf.Min(Mathf.Abs(1920/direction.x), Mathf.Abs(920/direction.y)) / 2;
        multiplier -= 100f;
        uiComponents.npcArrow.transform.position = cam.WorldToScreenPoint(cam.transform.position);
        uiComponents.npcArrow.transform.position += direction * multiplier;

    }

    public void UpdateNPCList(List<Transform> npcList)
    {
        this.npcList = new List<Transform>(npcList);
        Debug.Log("NPC Size list: " + this.npcList.Count);
    }

    public Transform GetClosestNPC()
    {
        Vector3 distanceVector = Vector3.zero;
        Transform closestNPC = null;
        bool initialized = false;
        foreach(Transform npc in npcList)
        {
            if (!initialized)
            {
                distanceVector = npc.position - playerTransform.position;
                initialized = true;
                closestNPC = npc;
            }
            else
            {
                Vector3 tempDistanceVector = npc.position - playerTransform.position;
                if(tempDistanceVector.magnitude < distanceVector.magnitude)
                {
                    distanceVector = tempDistanceVector;
                    closestNPC = npc;
                }
            }
        }

        return closestNPC;
    }

    public void FadeScreenTransition(System.Action action = null)
    {
        //fade in
        uiComponents.fadeImage.enabled = true;
        this.Chain().TweenLinear((a) =>
        {
            uiComponents.fadeImage.color = Color.Lerp(Color.clear, Color.black, a);
        }, fadingTime, 0f, 1f).Run(() =>
        {
            action.Invoke();
            //SceneManager.LoadScene(target);
        }).TweenLinear((a) =>
        {
            uiComponents.fadeImage.color = Color.Lerp(Color.black, Color.clear, a);
        }, fadingTime, 0f, 1f).Run(() =>
        {
            uiComponents.fadeImage.enabled = false;
        }).Start();
    }

}
