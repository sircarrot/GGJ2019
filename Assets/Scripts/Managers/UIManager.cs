﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CarrotPack;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour, IManager {

    [SerializeField] private GameObject UIPrefab;

    private UIComponents uiComponents;
    private Transform CanvasTransform;

    [Header("Fade Effect")]
    [SerializeField] private static float fadingTime = 0.5f;
    private Camera cam;
    private Transform cameraTransform;
    private Bounds cameraBox;
    private static float cameraSpeed = 0.1f; //0-1, 1 mean instant
    private static Vector2 cameraOffset = new Vector2(0f, 2f);

    [Header("Arrow Animation")]
    private Transform playerTransform;
    private List<Transform> npcList = new List<Transform>();

    [Header("Text Animation")]
    [SerializeField] private float textSpeed = 10f;
    [SerializeField] private float textLetterPause = 1f;
    [SerializeField] private float bubbleFrameRate = 2f;
    private bool currentlyOnDialogue = false;
    private bool displayingText = false;
    private IEnumerator DisplayTextCoroutine;
    private IEnumerator BubbleCoroutine;
    private string currentDialogueText;
    private NPCController currentTalkingNPC;

    public void InitializeManager()
    {
        CanvasTransform = Instantiate(UIPrefab, gameObject.transform).transform;
        uiComponents = CanvasTransform.GetComponent<UIComponents>();
        CentralizerCamera();
        UpdateCameraBox();
         
        SceneManager.sceneLoaded += (Scene, loadSceneMode) => 
        {
            CentralizerCamera();
            UpdateCameraBox();
        };
        DontDestroyOnLoad(cameraTransform.gameObject);
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

    #region Camera
    private void InitializeCamera()
    {
        if (cam == null) { cam = GameObject.Find("Main Camera").GetComponent<Camera>(); }
        cameraTransform = cam.transform;
    }

    private void UpdateCamera()
    {
        cameraTransform.position = (playerTransform.position + (Vector3)UIManager.cameraOffset) * UIManager.cameraSpeed + cameraTransform.position * (1 - UIManager.cameraSpeed);
        cameraTransform.position = cameraTransform.position.WithZ(-10f);
        if(cameraTransform.position.x < (cameraBox.min.x + cam.orthographicSize * cam.aspect))
        {
            cameraTransform.position = cameraTransform.position.WithX(cameraBox.min.x + cam.orthographicSize * cam.aspect);
        }
        if(cameraTransform.position.y < (cameraBox.min.y + cam.orthographicSize))
        {
            cameraTransform.position = cameraTransform.position.WithY(cameraBox.min.y + cam.orthographicSize);
        }
        if(cameraTransform.position.x > (cameraBox.max.x - cam.orthographicSize * cam.aspect))
        {
            cameraTransform.position = cameraTransform.position.WithX(cameraBox.max.x - cam.orthographicSize * cam.aspect);
        }
        if(cameraTransform.position.y > (cameraBox.max.y - cam.orthographicSize))
        {
            cameraTransform.position = cameraTransform.position.WithY(cameraBox.max.y - cam.orthographicSize);
        }
    }

    public void CentralizerCamera()
    {
        InitializeCamera();
        cameraTransform.position = playerTransform.position.WithZ(-10f);
    }

    private void UpdateCameraBox()
    {
        cameraBox = GameObject.Find("CameraBox").GetComponent<Collider2D>().bounds;
    }
    #endregion

    #region Arrow NPC
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
    #endregion Arrow NPC

    #region Dialogue
    public void OpenDialogue(string dialogueText, NPCController npc)
    {
        if(currentlyOnDialogue)
        {
            Debug.LogWarning("Something wrong with code flow!");
            return;
        }

        uiComponents.dialogueName.text = npc.npcName;
        uiComponents.dialogueName.font = npc.npcFont;
        uiComponents.dialogueTextBox.font = npc.npcFont;

        uiComponents.dialogueBubble.gameObject.SetActive(true);
        uiComponents.dialogueBubble.transform.position = cam.WorldToScreenPoint(npc.transform.position);

        BubbleCoroutine = BubbleAnimation(true);
        StartCoroutine(BubbleCoroutine);

        currentlyOnDialogue = true;
        NextDialogue(dialogueText);

        npc.StartAnimator();
        this.currentTalkingNPC = npc;
    }

    public bool NextDialogue(string dialogueText)
    {
        if(displayingText)
        {
            if (DisplayTextCoroutine != null) StopCoroutine(DisplayTextCoroutine);
            uiComponents.dialogueTextBox.text = currentDialogueText;
            displayingText = false;
            return false;
        }

        DisplayTextCoroutine = TextAnimation(dialogueText);
        StartCoroutine(DisplayTextCoroutine);
        return true;
    }

    public void CloseDialogue()
    {
        BubbleCoroutine = BubbleAnimation(false);
        StartCoroutine(BubbleCoroutine);
        //uiComponents.dialogueBubble.gameObject.SetActive(false);
        currentlyOnDialogue = false;
        
        currentTalkingNPC.StopAnimator();
        this.currentTalkingNPC = null;
    }

    private IEnumerator BubbleAnimation(bool open)
    {
        Transform toAnimate = uiComponents.dialogueBubble.transform;
        float scale = (open) ? 0 : 1f;
        int direction = (open) ? 1 : -1;

        float scalePerFrame = 1f / bubbleFrameRate;
        float vertTranslatePerFrame = 350f / bubbleFrameRate;

        for(int i = 0; i < bubbleFrameRate; ++i)
        {
            scale += direction * scalePerFrame;
            toAnimate.localScale = new Vector3(scale, scale, 1);
            toAnimate.position += new Vector3(0, vertTranslatePerFrame, 0);
            yield return null;
        }

        //if (open)
        //{
        //    StartCoroutine(DisplayTextCoroutine);
        //}
    }

    private IEnumerator TextAnimation(string dialogueText)
    {
        if (!displayingText)
        {
            displayingText = true;

            currentDialogueText = dialogueText;

            uiComponents.dialogueTextBox.text = "";
            foreach (char letter in dialogueText)
            {
                uiComponents.dialogueTextBox.text += letter;

                yield return new WaitForSeconds(textLetterPause / textSpeed);
            }

            displayingText = false;
        }
    }
    #endregion Dialogue

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
