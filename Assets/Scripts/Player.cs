using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Prime31;

[RequireComponent(typeof(CharacterController2D))]
public class Player : MonoBehaviour
{
    [Header("Player")]
    public float movementSpeed = 10f;
    private Vector3 velocity; //Store the difference in movement

    [Header("Jump")]
    public float jumpHeight = 6f;
    public float gravity = -10f;
    private float doubleJump = 2;
    private float jump = 0;
    //bool isJumping = false;

    [Header("Climbing")]
    public bool isClimbing = false;
    public float centreRadius = 0.5f;

    [Header("Components")]
    public CharacterController2D controller;
    private Animator anim;
    private SpriteRenderer rend;



    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, centreRadius);
    }
    private void Start()
    {

        anim = GetComponent<Animator>();
        controller = GetComponent<CharacterController2D>();
        rend = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        //Get Horizontal input (left / right)
        float inputH = Input.GetAxis("Horizontal");
        float inputV = Input.GetAxis("Vertical");

        bool isCrouch = Input.GetButton("Crouch");
        // If player is NOT grounded and NOT Climbing
        if (!controller.isGrounded && !isClimbing)
        {

        }
        velocity.y += gravity * Time.deltaTime; //Apply Gravity
        //move left or right
        Move(inputH);

        if (isCrouch)//Crouching
        {
            anim.SetBool("IsCrouch", true);
            velocity.x = 0f;
        }
        else
        {
            anim.SetBool("IsCrouch", false);
        }


        Climb(inputV, inputH);

        //if the controller is touching the ground
        if (controller.isGrounded)
        {
            //Reset Y
            velocity.y = 0f;
            if (Input.GetButton("Jump"))
            {
                Jump();

            }
            else
            {

                anim.SetBool("IsJumping", false);
            }
        }
        //if (Input.GetButton("Jump"))
        //{
        //    if (isJumping && jump < doubleJump)
        //    {
        //        Jump();
        //        jump++;
        //    }
        //    else
        //    {
        //        jump -= Time.deltaTime;
        //        anim.SetBool("IsJumping", false);
        //    }
        //}


        if (anim.GetBool("IsJumping"))
        {
            anim.SetFloat("JumpY", velocity.y);
        }

        if (!isClimbing)
        {
            // Move the controller with modified motion
            controller.move(velocity * Time.deltaTime);
        }

    }
    public void Move(float inputH)
    {
        velocity.x = inputH * movementSpeed;
        anim.SetBool("IsRunning", inputH != 0);
        if (inputH != 0)
        {
            rend.flipX = inputH < 0;
        }

    }

    public void Jump()
    {
        velocity.y = jumpHeight;
        anim.SetBool("IsJumping", true);
        //isJumping = true;
    }
    public void Hurt()
    {

    }
    public void Climb(float inputV, float inputH)
    {

        bool isOverLadder = false;
        Vector3 inputDir = new Vector3(inputH, inputV, 0);
        #region Part 1 - Detecting Ladders

        // Get a list of all hit objects overlapping point
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, centreRadius);
        // Loop through all hit objects
        foreach (var hit in hits)
        {
            if (hit.tag == "Ground")
            {
                isClimbing = false;
                isOverLadder = false;
                break;
            }
            // Check if tagged "Ladder"
            if (hit.tag == "Ladder")
            {
                // Player is overlapping a Ladder!
                isOverLadder = true;
                break; // Exit just the foreach loop (works for any loop)

            }
        }

        // If the Player is overlapping AND input vertical is made
        if (isOverLadder && inputV != 0)
        {
            // The player is in Climbing state!
            isClimbing = true;
            anim.SetBool("IsClimbing", true);
        }

        #endregion

        #region Part 2 - Translating the Player
        // If player is Climbing
        if (isClimbing)
        {
            velocity.y = 0;
            // Move player up and down on the ladder (additionally move left and right)
            transform.Translate(inputDir * movementSpeed * Time.deltaTime);


        }
        #endregion
        if (!isOverLadder)
        {
            isClimbing = false;
            anim.SetBool("IsClimbing", false);
        }
        anim.SetFloat("ClimbSpeed", inputDir.magnitude * movementSpeed);

    }
}
