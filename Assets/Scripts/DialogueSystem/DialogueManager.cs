using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CarrotPack;

public class DialogueManager : MonoBehaviour, IManager
{
    [SerializeField] private DialogueScriptableObject DialogueScriptableObject;

    public void InitializeManager()
    {
        DialogueScriptableObject.InitializeDictionary();
    }





}
