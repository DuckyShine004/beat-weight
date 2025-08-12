using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    public float groundDrag;
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    private bool readyToJump;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    private bool grounded;

    public Transform orientation;

    private float horizontalInput;
    private float verticalInput;

    private Vector3 moveDirection;

    private Rigidbody rigidBody;

    private void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        rigidBody.freezeRotation = true;

        readyToJump = true;
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void Update()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

        MyInput();
        SpeedControl();
        print(rigidBody.linearVelocity.magnitude);

        if (grounded)
        {
            rigidBody.linearDamping = groundDrag;
        }
        else
        {
            rigidBody.linearDamping = 0.0f;
        }
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        if (Input.GetKey(jumpKey) && readyToJump && grounded)
        {
            readyToJump = false;

            Jump();

            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    private void MovePlayer()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if (grounded)
        {
            rigidBody.AddForce(10.0f * moveSpeed * moveDirection.normalized, ForceMode.Force);
        }
        else if (!grounded)
        {
            rigidBody.AddForce(10.0f * airMultiplier * moveSpeed * moveDirection.normalized, ForceMode.Force);
        }
    }

    private void SpeedControl()
    {
        Vector3 flatVelocity = new Vector3(rigidBody.linearVelocity.x, 0.0f, rigidBody.linearVelocity.z);

        if (flatVelocity.magnitude > moveSpeed)
        {
            Vector3 limitedVelocity = flatVelocity.normalized * moveSpeed;

            rigidBody.linearVelocity = new Vector3(limitedVelocity.x, rigidBody.linearVelocity.y, limitedVelocity.z);
        }
    }

    private void Jump()
    {
        rigidBody.linearVelocity = new Vector3(rigidBody.linearVelocity.x, 0.0f, rigidBody.linearVelocity.z);

        rigidBody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        readyToJump = true;
    }
}
