using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputs : MonoBehaviour
{
    [Header("Character Input Values")]
    public Vector2 move;
    public Vector2 look;
    public bool jump;
    public bool sprint;
    public bool vaccum;
    public bool hyperVaccum;
    public bool vaccumRelese;
    public bool attack;

    [Header("Movement Settings")]
    public bool analogMovement;

    [Header("Mouse Cursor Settings")]
    public bool cursorLocked = true;
    public bool cursorInputForLook = true;

    public float Vaccumtime = 0f;
    public PlayerContoroller playerContoroller;

#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
    public void OnMove(InputAction.CallbackContext context)
    {
        MoveInput(context.ReadValue<Vector2>());
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        if (cursorInputForLook)
        {
            LookInput(context.ReadValue<Vector2>());
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        JumpInput(context.performed);
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        SprintInput(context.performed);
    }
   
#endif


    public void MoveInput(Vector2 newMoveDirection)
    {
        move = newMoveDirection;
    }

    public void LookInput(Vector2 newLookDirection)
    {
        look = newLookDirection;
    }

    public void JumpInput(bool newJumpState)
    {
        jump = newJumpState;
    }

    public void SprintInput(bool newSprintState)
    {
        sprint = newSprintState;
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        SetCursorState(cursorLocked);
    }

    private void SetCursorState(bool newState)
    {
        Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
    }
    private void Update()
    {
        if(!playerContoroller.Grounded)
        {
            return;
        }

        if(Input.GetButtonDown("Fire1"))
        {
            vaccum = true;
            Vaccumtime = 0f;
        }
        else
        {
            vaccum = false;
        }
        if(Input.GetButton("Fire1"))
        {
            Vaccumtime += Time.deltaTime;
            if(Vaccumtime >= 1f)
            {
                hyperVaccum = true;
            }
        }
        if(Input.GetButtonUp("Fire1"))
        {
            vaccum = false;
            hyperVaccum = false;
            Vaccumtime = 0f;
            vaccumRelese = true;

        }
        else
        {
            vaccumRelese = false;
        }
    }
}

