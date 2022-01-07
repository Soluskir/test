using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float runSpeed = 5f;
    [SerializeField] float jumpRange = 15f;
    [SerializeField] Transform gun;
    [SerializeField] GameObject bullet;

    Vector2 moveInput;
    Rigidbody2D rb2d;
    Animator myAnimator;
    BoxCollider2D feetCollider;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        feetCollider = GetComponent<BoxCollider2D>();

    }

    void Update()
    {
        Run();
        FlipSprite();
        JumpingAnimation();
    }

    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    void Run()
    {
        Vector2 playerVelocity = new Vector2(moveInput.x * runSpeed, rb2d.velocity.y);
        rb2d.velocity = playerVelocity;

        bool playerHasHorizontalSpeed = Mathf.Abs(rb2d.velocity.x) > Mathf.Epsilon;
        myAnimator.SetBool("isRunning", playerHasHorizontalSpeed);
    }

    void FlipSprite()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(rb2d.velocity.x) > Mathf.Epsilon;

        if (playerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(rb2d.velocity.x), 1f);
        }
    }

    void OnJump(InputValue value)
    {
        if (!feetCollider.IsTouchingLayers(LayerMask.GetMask("Platforms")))
        {
            return;
        }

        if (value.isPressed)
        {
            rb2d.velocity += new Vector2(0f, jumpRange);
        }

    }

    void OnFire(InputValue value)
    {

        if (value.isPressed & !myAnimator.GetBool("isRunning"))
        {
            Instantiate(bullet, gun.position, transform.rotation);
            myAnimator.Play("Base Layer.Shoot Arny", 0, 0f);
        }

        if (value.isPressed & myAnimator.GetBool("isRunning"))
        {
            Instantiate(bullet, gun.position, transform.rotation);
            myAnimator.Play("Base Layer.Walk Shooting", 0, 0f);
        }
    }

    void JumpingAnimation()
    {
        if (!feetCollider.IsTouchingLayers(LayerMask.GetMask("Platforms")))
        {
            myAnimator.SetBool("isJumping", true);
        }

        if (feetCollider.IsTouchingLayers(LayerMask.GetMask("Platforms")))
        {
            myAnimator.SetBool("isJumping", false);
        }
    }

}

