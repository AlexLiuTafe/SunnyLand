using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Prime31;

[RequireComponent(typeof(CharacterController2D))]
public class Player : MonoBehaviour
{
    public CharacterController2D controller;
    public float movementSpeed = 10f;
    public float gravity = -10f;

    private Vector3 motion; //Store the difference in movement
    private void Reset()
    {
        controller = GetComponent<CharacterController2D>();
    }
    
    void Update()
    {
        //Get Horizontal input (left / right)
        float inputH = Input.GetAxis("Horizontal");
        //move left or right
        motion.x = inputH * movementSpeed;
        //if the controller is touching the ground
        if(controller.isGrounded)
        {
            //Reset Y
            motion.y = 0f;
        }
        //Apply Gravity
        motion.y += gravity * Time.deltaTime;
        //Apply movement with motion
        controller.move(transform.right * inputH * movementSpeed * Time.deltaTime);
    }
}
