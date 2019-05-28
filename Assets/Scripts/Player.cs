using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Prime31;

[RequireComponent(typeof(CharacterController2D))]
public class Player : MonoBehaviour
{
    public float movementSpeed = 10f;
    public float jumpHeight = 6f;
    public float gravity = -10f;

    public CharacterController2D controller;
    private Animator anim;
    private SpriteRenderer rend;
    private Vector3 motion; //Store the difference in movement


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
        

        motion.y += gravity * Time.deltaTime; //Apply Gravity
        //move left or right
        Move(inputH);

        if(isCrouch)//Crouching
        {
            anim.SetBool("IsCrouch",true);
            motion.x = 0f;
        }
        else
        {
            anim.SetBool("IsCrouch", false);
        }
        
        
        Climb(inputV);

        //if the controller is touching the ground
        if (controller.isGrounded)
        {
            //Reset Y
            motion.y = 0f;
            if (Input.GetButton("Jump"))
            {
                Jump();
                anim.SetBool("IsJumping", true);
            }
            else
            {
                anim.SetBool("IsJumping", false);
            }
        }

        if (anim.GetBool("IsJumping"))
        {
            anim.SetFloat("JumpY", motion.y);
        }

        
        //Apply movement with motion
        controller.move(motion * Time.deltaTime);
    }
    public void Move(float inputH)
    {
        motion.x = inputH * movementSpeed;
        anim.SetBool("IsRunning", inputH != 0);
        rend.flipX = inputH < 0;
    }
    
    public void Jump()
    {
        motion.y = jumpHeight;
    }
    public void Hurt()
    {

    }
    public void Climb(float inputV)
    {

    }
}
