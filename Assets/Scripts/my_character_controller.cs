using UnityEngine;
using System.Collections;


[RequireComponent(typeof(CharacterController))]
public class my_character_controller : MonoBehaviour 
{
	public CharacterController controller;

	public float speed = 6.0F;							// Standard Character Speed
	public float runSpeed = 12.0F;						// Speed when pressing Run-Button (Shift)
	public float accelerationSpeed = 1.0F;				// How fast the character will reach top speed
														// Must be greater than 0
	public float runAccelerationSpeed = 1.0F;			
	public float decelerationSpeed = 1.0F;
	public float rotateSpeed = 3.0F;					// Standard Rotate Speed
	public float jumpSpeed = 8.0F;						// How high the character will jump
	public float continueJumping = 1.0F;				// How long the character can continue to gain height
	public float gravity = 20.0F;						// How fast the character comes back down
	public float maxGravity = 20.0F;					// Max Fall Speed
	public bool canMoveStart = true;
	public bool canMoveText = true;
	public string movementType = "NormalMovement";

	private float lastFrameJumpSpeed;					// Stores Y-Direction from last frame
	private bool hasJumped;								// Did we jump just now?
	private float continueJumpingRemainingTime;			// Counts remaining time to gain height
	private float currentSpeed;
	//private float currentAcceleration;


	private Vector3 moveDirection = Vector3.zero;		// Direction Vector for each frame

	void Start()
	{
		controller = GetComponent<CharacterController>();
		hasJumped = false;
		continueJumpingRemainingTime = continueJumping;
		currentSpeed = 0.0F;
		//currentAcceleration = 0.0F;
	}

	void Update()
	{
		if (!canMoveText)
			return;
		if (!canMoveStart)
			return;

			
		if (movementType == "ClimbMovement" && Input.GetButton ("Jump"))
			ClimbMovement ();
		else
			NormalMovement ();

	}

	void ClimbMovement()
	{
		float horizontal = Input.GetAxis("Horizontal");
		float vertical = Input.GetAxis ("Vertical");

		moveDirection = Vector3.zero;

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
		CalcCurrentSpeed ();
		moveDirection *= currentSpeed;

		controller.Move(moveDirection * Time.deltaTime);


		/**
		 * 
		Vector3 transformBottom = transform.position;
		transformBottom.y = transformBottom.y - Mathf.Abs(transform.localScale.y);
		Ray forward = new Ray (transformBottom, transform.forward);
		RaycastHit hit;

		if(Physics.Raycast (forward, out hit, 100))
		{
			Debug.DrawLine (forward.origin, hit.point, Color.red);
			if(hit.collider.gameObject.tag == "Climbable")
			{
				 
				var targetRotation = Quaternion.LookRotation (-hit.normal);
				transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 2.0F);


				CalcCurrentSpeed ();

				moveDirection = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
				moveDirection = transform.TransformDirection(moveDirection);

				moveDirection *= currentSpeed;

				controller.Move(moveDirection * Time.deltaTime);

			}
		}
		else{
			NormalMovement ();
		}
		*/



	}

	void NormalMovement()
	{
		// Calculate Speed 
		CalcCurrentSpeed();



		transform.Rotate(0, Input.GetAxis("Horizontal") * rotateSpeed, 0);
		moveDirection = new Vector3(0, 0, Input.GetAxis("Vertical"));
		moveDirection = transform.TransformDirection(moveDirection);

		/**
		if(Input.GetKey(KeyCode.LeftShift))
		{
			moveDirection *= runSpeed;	
		} else {
			moveDirection *= speed;
		}
		*/
		moveDirection *= currentSpeed;


		// Need to remember the jumpspeed from before for smooth jumping
		moveDirection.y = lastFrameJumpSpeed;		

		if (controller.isGrounded) 
		{		
			// Stop Gravity from going to negative infinity while grounded
			moveDirection.y = 0.0F;

			// Start Jump
			if (Input.GetButton ("Jump"))
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

		// Gravity
		if (moveDirection.y < maxGravity)
			moveDirection.y -= gravity * Time.deltaTime;
		else
			moveDirection.y = maxGravity;

		lastFrameJumpSpeed = moveDirection.y;

		// Apply Vector
		controller.Move(moveDirection * Time.deltaTime);
	}


	void CalcCurrentSpeed()
	{
		if(Input.GetKey(KeyCode.LeftShift))
		{
			if(Input.GetAxis("Vertical") != 0 && currentSpeed < runSpeed)
			{
				currentSpeed = currentSpeed + runAccelerationSpeed * Time.deltaTime;
			}
			else 
			{
				if (currentSpeed > 0)
					currentSpeed = currentSpeed - decelerationSpeed * Time.deltaTime;
				else
					currentSpeed = 0;
			}
		} else {
			if(Input.GetAxis("Vertical") != 0 && currentSpeed < speed)
			{
				currentSpeed = currentSpeed + accelerationSpeed * Time.deltaTime;
			}
			else
			{
				if (currentSpeed > 0)
					currentSpeed = currentSpeed - decelerationSpeed * Time.deltaTime;
				else
					currentSpeed = 0;
			}
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
			// Finds the point on the collider that is closest to us

			transform.LookAt (collider.ClosestPointOnBounds (transform.position));
			Debug.DrawLine (transform.position, collider.ClosestPointOnBounds (transform.position), Color.red, 2, false);

			// As we need only the y rotation
			Quaternion temp = transform.rotation;
			temp.eulerAngles = new Vector3 (0, transform.rotation.eulerAngles.y, 0);
			transform.rotation = temp;
		}
	}

	/**
	void OnTriggerStay(Collider collider)
	{
		// Climbing
		if (collider.tag == "Climbable")
		{
			// Finds the point on the collider that is closest to us
			transform.LookAt (collider.ClosestPointOnBounds (transform.position));
			Debug.DrawLine (transform.position, collider.ClosestPointOnBounds (transform.position), Color.red, 2, false);

			// As we need only the y rotation
			Quaternion temp = transform.rotation;
			temp.eulerAngles = new Vector3 (0, transform.rotation.eulerAngles.y, 0);
			transform.rotation = temp;
		}
	}
	*/


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