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
    public void OnVaccum(InputAction.CallbackContext context)
    {
        VccumInput(context.performed);
    }
    public void OnHyperVaccum(InputAction.CallbackContext context)
    {
        HyperVccumInput(context.performed);
    }
    public void OnVaccumRelese(InputAction.CallbackContext context)
    {
        VccumReleaseInput(context.performed);
    }
    public void OnAttack(InputAction.CallbackContext context)
    {
        AttackInput(context.performed);
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
    public void VccumInput(bool newState)
    {
        vaccum = newState;
    }
    public void HyperVccumInput(bool newState)
    {
        hyperVaccum = newState;
    }
    public void VccumReleaseInput(bool newState)
    {
        vaccumRelese = newState;
    }
    public void AttackInput(bool newState)
    {
        attack = newState;
    }
}

