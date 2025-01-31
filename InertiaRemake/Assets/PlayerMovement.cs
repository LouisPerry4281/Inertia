using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    CharacterController characterController;

    float xInput, yInput;
    Vector3 playerInput;

    Vector3 cameraRelativeMovement;
    Vector3 finalVelocity;

    [SerializeField] float movementSpeed;
    [SerializeField] float rotationSpeed;

    //Gravity
    float gravity = -9.8f;
    float groundedGravity = -0.05f;

    //Jump Variables
    bool isJumpPressed = false;
    float initialJumpVelocity;
    float maxJumpHeight = 1;
    float maxJumpTime = 0.5f;
    bool isJumping = false;

    private void Awake()
    {
        SetupJump();
    }

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        HandleInput();
        HandleRotation();
        HandleGravity();
        HandleJump();
    }

    private void HandleRotation()
    {
        Vector3 positionToLookAt;

        positionToLookAt = new Vector3(cameraRelativeMovement.x, 0, cameraRelativeMovement.z);

        Quaternion currentRotation = transform.rotation;

        if (characterController.velocity.magnitude > 0)
        {
            Quaternion targetRotation = Quaternion.LookRotation(positionToLookAt);
            transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    private void HandleInput()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            isJumpPressed = true;
        }
        else
            isJumpPressed = false;

        xInput = Input.GetAxis("Horizontal");
        yInput = Input.GetAxis("Vertical");

        playerInput.x = xInput;
        playerInput.z = yInput;

        cameraRelativeMovement = ConvertToCameraSpace(playerInput);
        finalVelocity = new Vector3(cameraRelativeMovement.x, finalVelocity.y, cameraRelativeMovement.z);
        characterController.Move(movementSpeed * finalVelocity * Time.deltaTime);
    }

    void HandleJump()
    {
        print(isJumping + " " + characterController.isGrounded + " " + isJumpPressed);
        if (!isJumping && characterController.isGrounded && isJumpPressed)
        {
            isJumping = true;
            finalVelocity.y = initialJumpVelocity;
        }
    }

    void HandleGravity()
    {
        if (characterController.isGrounded)
        {
            finalVelocity.y = groundedGravity;
        }
        else
        {
            finalVelocity.y = gravity;
        }
    }


    Vector3 ConvertToCameraSpace(Vector3 vectorToRotate)
    {
        float currentYValue = vectorToRotate.y;

        //Grab the camera forward and camera right from the main camera
        Vector3 cameraForward = Camera.main.transform.forward;
        Vector3 cameraRight = Camera.main.transform.right;

        //Flatten the camera's y so it only indicates an xz plane
        cameraForward.y = 0;
        cameraRight.y = 0;

        //Normalize the now lengthened vectors so they have a magnitude of 1
        cameraForward.Normalize();
        cameraRight.Normalize();

        Vector3 cameraForwardZProduct = vectorToRotate.z * cameraForward;
        Vector3 cameraRightXProduct = vectorToRotate.x * cameraRight;

        Vector3 vectorRotatedToCameraSpace = cameraForwardZProduct + cameraRightXProduct;
        vectorRotatedToCameraSpace.y = currentYValue;
        return vectorRotatedToCameraSpace;
    }
    
    void SetupJump()
    {
        //The jump is always a parabola, therefore max height is exactly half the time
        float timeToApex = maxJumpTime / 2;
        gravity = (-2 * maxJumpHeight) / Mathf.Pow(timeToApex, 2);
        initialJumpVelocity = (2 * maxJumpHeight) / timeToApex;
    }
}
