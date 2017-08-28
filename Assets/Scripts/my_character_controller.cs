using UnityEngine;
using System.Collections;


[RequireComponent(typeof(CharacterController))]
public class my_character_controller : MonoBehaviour 
{
	private CharacterController controller;

	// General variables
	public bool canMoveStart = true;					// Used by CharacterSwitching
	public bool canMoveText = true;						// Used by TextBoxManager
	public string movementType = "NormalMovement";		// To switch between diffrent movement options. Currently there is only NormalMovement and ClimbMovement
	public bool horizontalMoveSideways = false;

	// Moving variables
	public float speed = 6.0F;							// Standard Max Character Speed
	public float runSpeed = 12.0F;						// Max Speed when pressing Run-Button (Shift)
	public float accelerationSpeed = 1.0F;				// How fast the character will reach max speed. Must be greater than 0
	public float runAccelerationSpeed = 1.0F;			// Same as above, but for run speed
	public float decelerationSpeed = 1.0F;				// How fast the character will stop
	public float rotateSpeed = 3.0F;					// Standard Rotate Speed
	public float moveSidewaysSpeed = 4.0F;
	public float moveSidewaysAccelerationSpeed = 1.0f; 

	private float currentSpeed;							// Used by the function CalcCurrrentSpeed as a global return value
	private Vector3 moveDirection = Vector3.zero;		// Direction Vector for character movement. Reset each frame!
	private Vector3 moveDirectionLast = Vector3.zero;

	// Jumping variables
	public float jumpSpeed = 8.0F;						// How high the character will jump
	public float continueJumping = 1.0F;				// How long the character can continue to gain height
	public float gravity = 20.0F;						// How fast the character comes back down
	public float maxGravity = 20.0F;					// Max Fall Speed
	public float floatSpeed = -0.5F;

	private float lastFrameJumpSpeed;					// Stores Y-Direction from last frame
	private bool hasJumped;								// Did we jump just now?
	private float continueJumpingRemainingTime;			// Counts remaining time to gain height
	private bool canFloat;


	void Start()
	{
		controller = GetComponent<CharacterController>();
		hasJumped = false;
		continueJumpingRemainingTime = continueJumping;
		canFloat = false;
		currentSpeed = 0.0F;
	}

	void Update()
	{
		// Disabled all movement due to textbox
		if (!canMoveText)
			return;
		// Disabled all movement due to character switch
		// WIP, as this disables "gravity" too, which is most likely undiserable
		if (!canMoveStart)
		{
			if(transform.parent == null)
			{
				JustFalling ();
				controller.Move(moveDirection * Time.deltaTime);
				// Apply Vector
				//transform.Translate(moveDirection * Time.deltaTime);
			}
			//else
			//{
			//	controller.enabled = false;
			//}

			return;
		}


		moveDirection = Vector3.zero;

			
		if (movementType == "ClimbMovement" && Input.GetButton ("Jump"))
			ClimbMovement ();
		else
			NormalMovement ();

		// Apply Vector
		controller.Move(moveDirection * Time.deltaTime);

	}



	/**
	 * Does somewhat work now
	 */
	void ClimbMovement()
	{
		float horizontal = Input.GetAxis("Horizontal");
		float vertical = Input.GetAxis ("Vertical");

		// Raycast from player bottom
		// If hit, rotate towards the normal of the collider
		Vector3 transformBottom = transform.position;
		transformBottom.y = transformBottom.y - Mathf.Abs(transform.localScale.y);
		Ray forward = new Ray (transformBottom, transform.forward);
		RaycastHit hit;
		if (Physics.Raycast (forward, out hit, 100)) {
			Debug.DrawLine (forward.origin, hit.point, Color.red);
			if (hit.collider.gameObject.tag == "Climbable") {

				var targetRotation = Quaternion.LookRotation (-hit.normal);
				transform.rotation = Quaternion.Slerp (transform.rotation, targetRotation, Time.deltaTime * 2.0F);

			}
		}

		// If moving forward
		if(vertical > 0)
		{
			moveDirection.y = vertical;
			moveDirection.z = vertical;	//Also move forward to cling to wall
		}
		// If moving backwards
		else if(vertical < 0)
		{
			if(controller.isGrounded)
			{
				moveDirection.z = vertical; 	//Get away from the wall
			}
			else
			{
				moveDirection.y = vertical;
			}
		}
		// Horizontal Movement
		moveDirection.x = horizontal;

		// The Rest
		moveDirection = transform.TransformDirection(moveDirection);

		// CalcCurrentSpeed won't work for climbing, as it requires the player to move forward
		//CalcCurrentSpeed ();
		//moveDirection *= currentSpeed;
		moveDirection *= 2.0F;

	}

	// Move player around with WASD and jump with Space
	void NormalMovement()
	{
		// Calculate Speed 
		CalcCurrentSpeed();

		// Rotation
		if (!horizontalMoveSideways)
			transform.Rotate (0, Input.GetAxis ("Horizontal") * rotateSpeed, 0);
		else
			moveDirection.x = Input.GetAxis ("Horizontal");
		// Movement
		moveDirection.z = Input.GetAxis("Vertical");
		moveDirection = transform.TransformDirection(moveDirection);
		// Alter by speed
		moveDirection *= currentSpeed;


		// Apply jumpspeed from last frame for smooth jumping
		moveDirection.y = moveDirectionLast.y;		



		// Apply Gravity
		if (moveDirection.y < maxGravity)
			moveDirection.y -= gravity * Time.deltaTime;
		else
			moveDirection.y = maxGravity;




		// Actions only possible when player is grounded
		if (controller.isGrounded) 
		{		
			// Stop Gravity from going to negative infinity while grounded
			// Must set this to a value below 0, or "isGrounded" will fail
			moveDirection.y = -0.1F;//0.0F;

			// Start Jump
			if (Input.GetButtonDown ("Jump"))
			{
				moveDirection.y = jumpSpeed;
				hasJumped = true;
			}
		}

		// Allow for gaining more height by holding Jump
		if (Input.GetButton ("Jump") && hasJumped && continueJumpingRemainingTime >= 0.0F)
		{
			moveDirection.y = jumpSpeed;
			continueJumpingRemainingTime = continueJumpingRemainingTime - Time.deltaTime;
		} else {
			hasJumped = false;
			continueJumpingRemainingTime = continueJumping;
		}
			
		if (!controller.isGrounded && Input.GetButtonUp ("Jump"))
			canFloat = true;
		else if (controller.isGrounded)
			canFloat = false;
		if(!controller.isGrounded && canFloat && Input.GetButton("Jump"))
		{
			moveDirection.y = floatSpeed;
		}


		// Need to remember the jumpspeed for next frame for smooth jumping
		moveDirectionLast = moveDirection;


	}

	// Was intended to be used for making inactive characters fall to the ground
	void JustFalling()
	{
		moveDirection = moveDirectionLast;

		//CalcCurrentSpeed ();
		//moveDirection *= currentSpeed;
		moveDirection.y = moveDirectionLast.y;

		// Apply Gravity
		if (moveDirection.y < maxGravity)
			moveDirection.y -= gravity * Time.deltaTime;
		else
			moveDirection.y = maxGravity;

		if (controller.isGrounded) 
		{		
			// Stop Gravity from going to negative infinity while grounded
			// Must set this to a value below 0, or "isGrounded" will fail
			moveDirection.y = -0.1F;//0.0F;
			moveDirection.x = 0.0F;
			moveDirection.z = 0.0F;
		}

		moveDirectionLast = moveDirection;
	}

	// Calculates speed for this frame and stores it in global variable
	// Smoothes acceleration and deceleration
	// Implements running (Shift)
	void CalcCurrentSpeed()
	{
		bool movinSideways = false;
		float curAccSpeed;
		float topSpeed;

		if (horizontalMoveSideways && Input.GetAxis ("Vertical") == 0 && Input.GetAxis ("Horizontal") != 0)
			movinSideways = true;

		// If only moving sideways
		if(movinSideways)
		{
			topSpeed = moveSidewaysSpeed;
			curAccSpeed = moveSidewaysAccelerationSpeed;
		}
		// If running
		else if (Input.GetKey (KeyCode.LeftShift))
		{
			curAccSpeed = runAccelerationSpeed;
			topSpeed = runSpeed;
		}
		// Not running
		else
		{
			topSpeed = speed;
			curAccSpeed = accelerationSpeed;
		}


		// If moving
		if((Input.GetAxis("Vertical") != 0 || movinSideways) && currentSpeed < topSpeed)
		{
			currentSpeed = currentSpeed + curAccSpeed * Time.deltaTime;
		}
		// Not moving
		else 
		{
			if (currentSpeed > 0)
				currentSpeed = currentSpeed - decelerationSpeed * Time.deltaTime;
			else
				currentSpeed = 0;
		}


	}



	void OnTriggerEnter(Collider collider)
	{
		// Teleport back to start
		if(collider.gameObject.name == "ResetPlayer")
		{
			transform.position = new Vector3 (1, 1, 1);
		}

		// Climbing
		if (collider.tag == "Climbable")
		{
			print ("Climbing");
			movementType = "ClimbMovement";
		}
	}




	void OnTriggerExit(Collider collider)
	{
		// Stop Climbing
		if(collider.tag == "Climbable")
		{
			print ("Not Climbing");
			movementType = "NormalMovement";
		}
	}

}