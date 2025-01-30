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

        characterController.Move(movementSpeed * playerInput * Time.deltaTime);
    }
}
