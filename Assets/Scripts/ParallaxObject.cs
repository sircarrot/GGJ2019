using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ParallaxObject : MonoBehaviour 
{
	private Image image;
    private Transform cameraTransform;
	public float moveSpeed = 1f;
	private void Start()
	{
		image = GetComponent<Image>();
        cameraTransform =  GameObject.Find("Main Camera").transform;
	}
	private void Update()
	{
		image.rectTransform.localPosition = image.rectTransform.localPosition.WithX(cameraTransform.position.x * moveSpeed);
	}
}
