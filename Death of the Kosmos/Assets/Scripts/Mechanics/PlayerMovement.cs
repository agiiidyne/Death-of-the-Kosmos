using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	// Camera rotation
	public float mouseSensitivity = 2f;
	private float verticalRotation = 0f;
	private Transform cameraTransform;
	
	// Ground movement
	private Rigidbody rb;
	public float MoveSpeed = 5f;
	private float moveHorizontal;
	private float moveForward;
	
	// Jumping
	public float jumpForce = 10f;
	public float fallMultiplier = 2.5f; // when falling
	public float ascendMultiplier = 2f; // when getting to the peak of the jump
	private bool isGrounded = true;
	public LayerMask groundLayer;
	private float groundCheckTimer = 0f;
	private float groundCheckDelay = 0.3f;
	private float playerHeight;
	private float raycastDistance;
	
	void Start()
	{
		rb = GetComponent<Rigidbody>();
		rb.freezeRotation = true;
		cameraTransform = Camera.main.transform;
		
		// raycast beneath player's feet
		playerHeight = GetComponentInChildren<CapsuleCollider>().height * transform.localScale.y;
		raycastDistance = (playerHeight / 2) + 0.2f;
		
		// hides mouse
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}
	
	void Update()
	{
		moveHorizontal = Input.GetAxisRaw("Horizontal");
		moveForward = Input.GetAxisRaw("Vertical");
		
		RotateCamera();
		
		if(Input.GetButtonDown("Jump") && isGrounded)
		{
			Jump();
		}
		
		// checks if the player is on the ground and keeps track of the ground check delay
		if(!isGrounded && groundCheckTimer <= 0f)
		{
			Vector3 rayOrigin = transform.position + Vector3.up * 0.1f;
			isGrounded = Physics.Raycast(rayOrigin, Vector3.down, raycastDistance, groundLayer);
		}
		else
		{
			groundCheckTimer -= Time.deltaTime;
		}
	}
	
	void FixedUpdate()
	{
		MovePlayer();
		ApplyJumpPhysics();
	}
	
	void MovePlayer()
	{
		Vector3 movement = (transform.right * moveHorizontal + transform.forward * moveForward).normalized;
		Vector3 targetVelocity = movement * MoveSpeed;
		
		// applies movement to the Rigidbody
		Vector3 velocity = rb.velocity;
		velocity.x = targetVelocity.x;
		velocity.z = targetVelocity.z;
		rb.velocity = velocity;
		
		// if the player isn't moving, stops velocity so there's no sliding
		if(isGrounded && moveHorizontal == 0 && moveForward == 0)
		{
			rb.velocity = new Vector3(0, rb.velocity.y, 0);
		}
	}
	
	void RotateCamera()
	{
		float horizontalRotation = Input.GetAxis("Mouse X") * mouseSensitivity;
		transform.Rotate(0, horizontalRotation, 0);
		
		verticalRotation -= Input.GetAxis("Mouse Y") * mouseSensitivity;
		verticalRotation = Mathf.Clamp(verticalRotation, -90f, 90f);
		
		cameraTransform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);
	}
	
	void Jump()
	{
		isGrounded = false;
		groundCheckTimer = groundCheckDelay;
		rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z); // initial jump burst
	}
	
	void ApplyJumpPhysics()
	{
		if(rb.velocity.y < 0)
		{
			// falling: make descent faster
			rb.velocity += Vector3.up * Physics.gravity.y * fallMultiplier * Time.fixedDeltaTime;
		}
		else if(rb.velocity.y > 0)
		{
			// rising: reaches peak of jump faster
			rb.velocity += Vector3.up * Physics.gravity.y * ascendMultiplier * Time.fixedDeltaTime;
		}
	}
}
