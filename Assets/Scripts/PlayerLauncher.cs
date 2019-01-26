using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CarrotPack;

public class PlayerLauncher : MonoBehaviour 
{
	public Vector2 launchForce;
	private void OnTriggerEnter2D(Collider2D other)
	{
		var otherRigidbody = other.GetComponent<Rigidbody2D>();
		if(otherRigidbody != null)
		{
			otherRigidbody.velocity = Vector2.zero;
			otherRigidbody.AddForce(launchForce);
		}
	}
}
