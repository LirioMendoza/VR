using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]

public class Move : MonoBehaviour
{
    public float speed;
    public float runMultiplier;
    public float gravity=-9.81f;
    public float jumpHeigth;
    public float rotationSpeed;

    private CharacterController characterController;
    private Vector3 velocity;
    private PlayerInputActions inputActions;
    private Vector2 moveInput;
    private float rotateInput;

    public bool isRunning = false;
    public bool isGrounded;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        inputActions = new PlayerInputActions();

        inputActions.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Move.canceled += ctx => moveInput = Vector2.zero;

        inputActions.Player.Run.performed += ctx => isRunning = true;
        inputActions.Player.Run.canceled += ctx => isRunning = false;

        inputActions.Player.Jump.performed += ctx => Jump();
        
        inputActions.Player.Rotate.performed += ctx => rotateInput = ctx.ReadValue<float>();
        inputActions.Player.Rotate.canceled += ctx => rotateInput = 0;

    }

    void OnEnable()
    {
        inputActions.Enable();
    }

    void OnDisable()
    {
        inputActions.Disable();
    }

    void Jump()
    {
        if (isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeigth * -2f * gravity);
            Debug.Log(velocity.y);
        }
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = characterController.isGrounded;
        if ((isGrounded && velocity.y < 0))
        {
            velocity.y = -2;
        }

        Vector3 move = new Vector3(moveInput.x, 0, moveInput.y);
        move = transform.TransformDirection(move);
        float currentSpeed = isRunning ? speed * runMultiplier : speed;
        characterController.Move(move * currentSpeed * Time.deltaTime);
        float rotation = rotateInput * rotationSpeed * Time.deltaTime;
        transform.Rotate(0, rotation, 0);

        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);

        
    }
}
