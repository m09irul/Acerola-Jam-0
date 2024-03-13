using UnityEngine;
using System.Collections;

public class NormalPlayerMovement : MonoBehaviour
{
	public bool drawDebugRaycasts = true;	

	[Header("Movement Properties")]
	public float speed = 8f;				
	public float coyoteDuration = .05f;		
	public float maxFallSpeed = -25f;	

	[Header("Jump Properties")]
	public float jumpForce = 6.3f;			
	public float jumpHoldForce = 1.9f;		
	public float jumpHoldDuration = .1f;	

	[Header("Environment Check Properties")]
	public float footOffset = .4f;			
	public float groundDistance = .2f;		
	public LayerMask groundLayer;			

	[Header ("Status Flags")]
	public bool isOnGround;					
	public bool isJumping;					

	PlayerInput input;						
	Rigidbody2D rigidBody;                  
	Animator playerAnimator;
	
	float jumpTime;							
	float coyoteTime;						

	float originalXScale;					
	int direction = 1;						



	void Start ()
	{
		//Get a reference to the required components
		input = GetComponent<PlayerInput>();
		rigidBody = GetComponent<Rigidbody2D>();
		playerAnimator = GetComponent<Animator>();

		originalXScale = transform.localScale.x;
		
	}

	void FixedUpdate()
	{
		PhysicsCheck();

		GroundMovement();		
		MidAirMovement();
	}

	void PhysicsCheck()
	{
		//Start by assuming the player isn't on the ground and the head isn't blocked
		isOnGround = false;

		//Cast rays for the left and right foot
		RaycastHit2D leftCheck = Raycast(new Vector2(-footOffset, 0f), Vector2.down, groundDistance);
		RaycastHit2D rightCheck = Raycast(new Vector2(footOffset, 0f), Vector2.down, groundDistance);

		//If either ray hit the ground, the player is on the ground
		if (leftCheck || rightCheck)
			isOnGround = true;
	}

	void GroundMovement()
	{

		if(input.horizontal !=0)
		{
			if(isOnGround)
				playerAnimator.Play(AllString.PLAYER_WALK_ANIM);
			else
				playerAnimator.Play(AllString.NONE);
		}
		else
		{
			if(isOnGround)
				playerAnimator.Play(AllString.PLAYER_IDLE_ANIM);
			else
				playerAnimator.Play(AllString.NONE);
		}

		float xVelocity = speed * input.horizontal;

		if (xVelocity * direction < 0f)
			FlipCharacterDirection();

		rigidBody.velocity = new Vector2(xVelocity, rigidBody.velocity.y);

		if (isOnGround)
			coyoteTime = Time.time + coyoteDuration;
	}

	void MidAirMovement()
	{

		if (input.jumpPressed && !isJumping && (isOnGround || coyoteTime > Time.time))
		{
			Audio_Manager.instance.Play(AllString.JUMP_AUDIO);

			StartCoroutine(JumpSqueeze(1.15f * direction, 0.85f, 0.1f));

			isOnGround = false;
			isJumping = true;

			jumpTime = Time.time + jumpHoldDuration;

			//...add the jump force to the rigidbody...
			rigidBody.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);

			//...and tell the Audio Manager to play the jump audio
			//AudioManager.PlayJumpAudio();
		}
		else if (isJumping)
		{
			if (input.jumpHeld)
				rigidBody.AddForce(new Vector2(0f, jumpHoldForce), ForceMode2D.Impulse);

			if (jumpTime <= Time.time)
				isJumping = false;
		}

		if (rigidBody.velocity.y < maxFallSpeed)
			rigidBody.velocity = new Vector2(rigidBody.velocity.x, maxFallSpeed);
	}

	IEnumerator JumpSqueeze(float xSqueeze, float ySqueeze, float seconds)
	{
		Vector3 originalSize = transform.localScale;
		Vector3 newSize = new Vector3(xSqueeze, ySqueeze, originalSize.z);
		float t = 0f;
		while (t <= 1.0)
		{
			t += Time.deltaTime / seconds;
			transform.localScale = Vector3.Lerp(originalSize, newSize, t);
			yield return null;
		}
		t = 0f;
		while (t <= 1.0)
		{
			t += Time.deltaTime / seconds;
			transform.localScale = Vector3.Lerp(newSize, originalSize, t);
			yield return null;
		}

	}

	void FlipCharacterDirection()
	{
		direction *= -1;
		Vector3 scale = transform.localScale;

		scale.x = originalXScale * direction;
		transform.localScale = scale;
	}
	RaycastHit2D Raycast(Vector2 offset, Vector2 rayDirection, float length)
	{
		return Raycast(offset, rayDirection, length, groundLayer);
	}

	RaycastHit2D Raycast(Vector2 offset, Vector2 rayDirection, float length, LayerMask mask)
	{
		Vector2 pos = transform.position;

		RaycastHit2D hit = Physics2D.Raycast(pos + offset, rayDirection, length, mask);

		if (drawDebugRaycasts)
		{
			Color color = hit ? Color.red : Color.green;
			Debug.DrawRay(pos + offset, rayDirection * length, color);
		}

		return hit;
	}
}
