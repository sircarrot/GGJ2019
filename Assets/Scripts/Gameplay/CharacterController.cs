using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
	public float maxSpeed = 0.5f;
	public float jumpForce = 400f;
	public float moveForce = 500f;
    public Transform raycastSource;
    public Transform eyeTransform;
    public float eyeMaxMoveDistance = 0.2f;
    public float eyeMoveSpeed = 0.1f;
	private static float groundCheckTolerance = 0.1f;
	private new Rigidbody2D rigidbody2D;
	private LayerMask groundLayer;
    private int jumpCount;
    void Start()
    {
		this.groundLayer = 1 << LayerMask.NameToLayer("Terrain");
		this.rigidbody2D = this.GetComponent<Rigidbody2D>();
        this.jumpCount = 0;
    }

    void Update()
    {
		if(Physics2D.Raycast(this.raycastSource.position, Vector2.down, CharacterController.groundCheckTolerance, this.groundLayer).collider != null)
		{
            this.jumpCount = 0;
        }

        var targetEyePosition = (Vector3)this.rigidbody2D.velocity.normalized * this.eyeMaxMoveDistance;
        this.eyeTransform.localPosition = targetEyePosition * this.eyeMoveSpeed + this.eyeTransform.localPosition * (1 - this.eyeMoveSpeed);
        
		this.rigidbody2D.velocity = this.rigidbody2D.velocity.WithX(0);
    }

    public void MoveCharacter(MoveDirection moveDirection)
    {
        this.rigidbody2D.AddForce(new Vector2(this.moveForce * (int) moveDirection, 0f));
        if (this.rigidbody2D.velocity.x < this.maxSpeed * (int) moveDirection)
        {
            this.rigidbody2D.velocity = this.rigidbody2D.velocity.WithX(this.maxSpeed * (int) moveDirection);
        }
    }

    public void Jump()
	{
		if(Physics2D.Raycast(this.raycastSource.position, Vector2.down, CharacterController.groundCheckTolerance, this.groundLayer).collider == null)
		{
            if(this.jumpCount == 0)
            {
                this.jumpCount++;
            }
            else
            {
                return;
            }
		}
        
        this.rigidbody2D.velocity = this.rigidbody2D.velocity.WithY(0);
        this.rigidbody2D.AddForce(new Vector2(0f, this.jumpForce));
	}
}

public enum MoveDirection
{
    Left = -1,
    Right = 1
}