using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    CharacterController characterController;

    float xInput, yInput;
    Vector3 playerInput;

    [SerializeField] float movementSpeed;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        HandleInput();
    }

    private void HandleInput()
    {
        xInput = Input.GetAxis("Horizontal");
        yInput = Input.GetAxis("Vertical");

        playerInput.x = xInput;
        playerInput.z = yInput;

        Vector3 cameraRelativeMovement = ConvertToCameraSpace(playerInput);
        characterController.Move(movementSpeed * cameraRelativeMovement * Time.deltaTime);
    }

    Vector3 ConvertToCameraSpace(Vector3 vectorToRotate)
    {
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
        return vectorRotatedToCameraSpace;
    }
}
