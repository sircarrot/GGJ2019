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
		this.image = this.GetComponent<Image>();
        cameraTransform =  GameObject.Find("Main Camera").transform;
	}
	private void Update()
	{
		this.image.rectTransform.localPosition = this.image.rectTransform.localPosition.WithX(cameraTransform.position.x * this.moveSpeed);
	}
}
