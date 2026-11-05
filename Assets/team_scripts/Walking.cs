using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class Walking : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float sneakSpeed = 2f;
    private float hazardSlowMultiplier = 1f;

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Animator animator;
    private bool isSneaking;

    public bool IsSneaking => isSneaking;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

    }

    
    void Update()
    {
        if(PauseController.IsGamePaused || MenuController.IsMenuOpen){
            rb.linearVelocity = Vector2.zero;
            animator.SetBool("isWalking", false);
            return;
        }
        float currentSpeed = isSneaking ? sneakSpeed : moveSpeed;
        currentSpeed *= hazardSlowMultiplier;
        rb.linearVelocity = moveInput * currentSpeed;

        //for sneaking animation
        animator.SetBool("isSneaking", isSneaking);
    }

    public void Move(InputAction.CallbackContext context){
        //pauses movement while any menu is open
        if(PauseController.IsGamePaused || MenuController.IsMenuOpen){
            moveInput = Vector2.zero;
            animator.SetBool("isWalking", false);
            return;
        }
        
        animator.SetBool("isWalking", true);

        if(context.canceled){
            animator.SetBool("isWalking", false);
            animator.SetFloat("LastInputX", moveInput.x);
            animator.SetFloat("LastInputY", moveInput.y);
        }

        moveInput = context.ReadValue<Vector2>();
        animator.SetFloat("InputX", moveInput.x);
        animator.SetFloat("InputY", moveInput.y);
    }

    public void Sneak(InputAction.CallbackContext context)
    {
        if(PauseController.IsGamePaused || MenuController.IsMenuOpen) return;

        if (context.performed)
        {
            isSneaking = true;
        }
        else if (context.canceled)
        {
            isSneaking = false;
        }
    }
    public void SetHazardSlow(float multiplier)
    {
        hazardSlowMultiplier = multiplier;
    }
}
