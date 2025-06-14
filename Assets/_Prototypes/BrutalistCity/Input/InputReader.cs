using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static FPS_Actions;

public interface IInputReader
{
    Vector2 MoveDirection { get; }
    Vector2 LookVector { get; }
    void EnablePlayerActions();
    void DisablePlayerActions();
    void SetEnablePlayerActions(bool enable);
    void SetDisablePlayerActions(bool disable);
}

[CreateAssetMenu(fileName = "InputReader", menuName = "Scriptable Objects/InputReader")]
public class InputReader : ScriptableObject, IFPSActions, IInputReader
{
    public Action onJump;
    public Action<bool> onSprint;
    public Action onAlternateFire;
    public Action onAbilityOne;
    public Action onAttack;
    public Action<Vector2> onMove;
    public Action<Vector2> onLook;
    public Action onTogglePause;

    public FPS_Actions playerInputActions;

    private bool AcceptingInput = true;

    public bool IsJumpPressed => AcceptingInput && playerInputActions.FPS.Jump.IsPressed();
    public bool IsSprintPressed => AcceptingInput && playerInputActions.FPS.Sprint.IsPressed();
    public bool IsAbilityOnePressed => AcceptingInput && playerInputActions.FPS.AbilityOne.IsPressed();

    public Vector2 MoveDirection => AcceptingInput ? playerInputActions.FPS.Move.ReadValue<Vector2>() : Vector2.zero;
    public Vector2 LookVector => AcceptingInput ? playerInputActions.FPS.Look.ReadValue<Vector2>() : Vector2.zero;

    public void OnJump(InputAction.CallbackContext context)
    {
        if (AcceptingInput && context.performed)
        {
            onJump?.Invoke();
        }
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        if (!AcceptingInput)
            return;

        onLook?.Invoke(context.ReadValue<Vector2>());
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (!AcceptingInput)
            return;

        onMove?.Invoke(context.ReadValue<Vector2>());
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        if (!AcceptingInput)
            return;

        if (context.performed)
        {
            onSprint?.Invoke(true);
        }
        else if (context.canceled)
        {
            onSprint?.Invoke(false);
        }
    }

    public void OnAbilityOne(InputAction.CallbackContext context)
    {
        if (AcceptingInput && context.performed)
        {
            onAbilityOne?.Invoke();
        }
    }

    public void OnAlternateFire(InputAction.CallbackContext context)
    {
        if (AcceptingInput && context.performed)
        {
            onAlternateFire?.Invoke();
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (AcceptingInput && context.performed)
        {
            onAttack?.Invoke();
        }
    }

    public void EnablePlayerActions()
    {
        if (playerInputActions == null)
        {
            playerInputActions = new FPS_Actions();
            playerInputActions.FPS.SetCallbacks(this);
            playerInputActions.Enable();
        }

        AcceptingInput = true;
        Debug.Log("Enabled player input!");
    }

    public void DisablePlayerActions()
    {
        Debug.Log("Disabled player input!");
        AcceptingInput = false;
    }

    public void SetEnablePlayerActions(bool enable)
    {
        if (enable)
        {
            EnablePlayerActions();
        }
        else
        {
            DisablePlayerActions();
        }
    }

    public void SetDisablePlayerActions(bool disable)
    {
        SetEnablePlayerActions(!disable);
    }
}
