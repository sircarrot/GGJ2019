using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CarrotPack;

public class ScenePortal : MonoBehaviour 
{
	public string targetScene;
	public string targetAnchor;

	private void OnTriggerEnter2D(Collider2D other)
	{
		Toolbox.Instance.GetManager<GameManager>().ChangeScene(this.targetScene, this.targetAnchor);
	}
}
