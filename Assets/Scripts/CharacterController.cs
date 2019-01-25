using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
	public float maxSpeed = 0.5f;
	public float jumpForce = 200f;
	public float moveForce = 200f;
	private static float groundCheckTolerance = 0.1f;
	private Rigidbody2D rigidbody2D;
	private LayerMask groundLayer;
    void Start()
    {
		this.groundLayer = 1 << LayerMask.NameToLayer("Terrain");
		this.rigidbody2D = this.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
		this.rigidbody2D.velocity = this.rigidbody2D.velocity.WithX(0);
		if(Input.GetKeyDown(KeyCode.W))
		{
			this.Jump();
		}
		if(Input.GetKey(KeyCode.A))
		{
			this.MoveLeft();
		}
		if(Input.GetKey(KeyCode.D))
		{
			this.MoveRight();
		}
		Debug.DrawRay(this.transform.position, Vector2.down, Color.green);
    }

	void MoveLeft()
	{
		this.rigidbody2D.AddForce(new Vector2(-this.moveForce, 0f));
		if(this.rigidbody2D.velocity.x < -this.maxSpeed)
		{
			this.rigidbody2D.velocity = this.rigidbody2D.velocity.WithX(-this.maxSpeed);
		}
	}
	void MoveRight()
	{
		this.rigidbody2D.AddForce(new Vector2(this.moveForce, 0f));
		if(this.rigidbody2D.velocity.x > this.maxSpeed)
		{
			this.rigidbody2D.velocity = this.rigidbody2D.velocity.WithX(this.maxSpeed);
		}
	}
	void Jump()
	{
		if(Physics2D.Raycast(this.transform.position, Vector2.down, CharacterController.groundCheckTolerance, this.groundLayer).collider != null)
		{
			this.rigidbody2D.AddForce(new Vector2(0f, this.jumpForce));
		}
	}
}
