using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterControllerScript : MonoBehaviour
{
    private CharacterController controller;
    private PlayerInputActions playerInputActions;
    private InputAction movement;

    [SerializeField] private GameObject mainCamera;

    [SerializeField] float speed = 7.5f;
    private Vector3 velocity = new Vector3(0, 0, 0);

    private Vector3 moveVector;

    [SerializeField] float LookSpeed = 0.75f;

    [SerializeField] private GameObject groundCheckObject;
    private bool isGrounded = true;
    [SerializeField] float JumpHeight        = 1;
    [SerializeField] float gravityMultiplier = 1.8f;

    private void Awake()
    {
        playerInputActions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        movement = playerInputActions.Player.Movement;
        movement.Enable();

        playerInputActions.Player.Jump.performed += Jump;
        playerInputActions.Player.Jump.Enable();
    }

    private void OnDisable()
    {
        playerInputActions.Player.Jump.Disable();
        playerInputActions.Player.Jump.Disable();
    }

    void Start()
    {
        controller = this.GetComponent<CharacterController>();
    }

    void Gravity()
    {
        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = 0f;
        }
        else
        {
            velocity.y += Physics.gravity.y * gravityMultiplier * Time.deltaTime;
        }
    }

    void GroundCheck()
    {
        RaycastHit hit;
        float distance = 0.5f;
        Vector3 dir = new Vector3(0, -1);

        if (Physics.Raycast(transform.position, dir, out hit, distance))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }

    void Move()
    {

        moveVector = Quaternion.Euler(0, mainCamera.transform.eulerAngles.y, 0) * new Vector3(movement.ReadValue<Vector2>().x, 0, movement.ReadValue<Vector2>().y);

        controller.Move(speed * Time.deltaTime * moveVector);
        controller.Move(velocity * Time.deltaTime);

        if (moveVector != Vector3.zero)
        {
            transform.forward = Vector3.Lerp(transform.forward, moveVector, LookSpeed);
        }
    }

    void Jump(InputAction.CallbackContext obj)
    {
        if (isGrounded)
        {
            isGrounded = false;
            velocity.y = 0;
            velocity.y += Mathf.Sqrt(JumpHeight * -2f * Physics.gravity.y);
        }
    }

    void FixedUpdate()
    {
        GroundCheck();
        Gravity();
        Move();
    }
}
