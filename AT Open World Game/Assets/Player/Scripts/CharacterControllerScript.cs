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
    
    Animator animationController;

    // inplement a list of weapons
    bool weaponEquipped;
    [SerializeField] GameObject sword;
    [SerializeField] GameObject attackPoint;
    [SerializeField] float attackRange;
    [SerializeField] int attackDamage;
    [SerializeField] LayerMask enemyLayer;
    int attackAnim = 2;

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

        playerInputActions.Player.Interact.performed += Interact;
        playerInputActions.Player.Interact.Enable();

        playerInputActions.Player.EquipWeapon.performed += EquipWeapon;
        playerInputActions.Player.EquipWeapon.Enable();

        playerInputActions.Player.Attack.performed += Attack;
        playerInputActions.Player.Attack.Enable();
    }

    private void OnDisable()
    {
        playerInputActions.Player.Movement.Disable();
        playerInputActions.Player.Jump.Disable();
        playerInputActions.Player.Interact.Disable();
        playerInputActions.Player.EquipWeapon.Disable();
        playerInputActions.Player.Attack.Disable();
    }

    private void Start()
    {
        controller = this.GetComponent<CharacterController>();

        animationController = this.GetComponent<Animator>();
    }

    private void Gravity()
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

    private void GroundCheck()
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

    private void Move()
    {
        moveVector = Quaternion.Euler(0, mainCamera.transform.eulerAngles.y, 0) * new Vector3(movement.ReadValue<Vector2>().x, 0, movement.ReadValue<Vector2>().y);

        controller.Move(speed * Time.deltaTime * moveVector);
        controller.Move(velocity * Time.deltaTime);

        if (moveVector != Vector3.zero)
        {
            transform.forward = Vector3.Lerp(transform.forward, moveVector, LookSpeed);
            animationController.SetBool("Move", true);
        }
        else
        {
            animationController.SetBool("Move", false);
        }
    }

    private void Jump(InputAction.CallbackContext obj)
    {
        if (isGrounded)
        {
            isGrounded = false;
            velocity.y = 0;
            velocity.y += Mathf.Sqrt(JumpHeight * -2f * Physics.gravity.y);
        }
    }

    private void Interact(InputAction.CallbackContext obj)
    {
        Debug.Log("Interact");
    }

    private void EquipWeapon(InputAction.CallbackContext obj)
    {
        if(!weaponEquipped)
        {
            animationController.SetBool("WeaponDrawn", true);
        }
        else
        {
            animationController.SetBool("WeaponDrawn", false);
        }
    }


    public void DrawWeapon()
    {
        sword.SetActive(true);
        weaponEquipped = true;
    }

    public void SheathWeapon()
    {
        sword.SetActive(false);
        weaponEquipped = false;
    }

    public void Attack(InputAction.CallbackContext obj)
    {
        if(weaponEquipped)
        {
            //int anim = Random.Range(1, 3);
            switch(attackAnim)
            {
                case 1:
                    attackAnim = 2;
                    break;
                case 2:
                    attackAnim = 1;
                    break;
            }
            //Debug.Log(anim);

            animationController.SetTrigger("Attack");
            animationController.SetInteger("AttackAnim", attackAnim);

            attackPoint.SetActive(true);
            if (attackPoint.activeInHierarchy)
            {
                Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.transform.position, attackRange, enemyLayer);

                foreach (Collider enemy in hitEnemies)
                {
                    if (enemy.GetComponent<Enemy>() != null)
                    {
                        //Debug.Log("Enemy hit " + enemy.gameObject.name);
                        enemy.GetComponent<Enemy>().TakeDamage(attackDamage);
                        attackPoint.SetActive(false);
                    }
                }
            }
        }
    }

    private void FixedUpdate()
    {
        GroundCheck();
        Gravity();
        Move();
    }
}
