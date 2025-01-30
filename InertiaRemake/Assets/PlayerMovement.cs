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

    [SerializeField] float movementSpeed;
    [SerializeField] float rotationSpeed;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        HandleInput();
        HandleRotation();
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
        xInput = Input.GetAxis("Horizontal");
        yInput = Input.GetAxis("Vertical");

        playerInput.x = xInput;
        playerInput.z = yInput;

        cameraRelativeMovement = ConvertToCameraSpace(playerInput);
        characterController.Move(movementSpeed * cameraRelativeMovement * Time.deltaTime);
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
}
