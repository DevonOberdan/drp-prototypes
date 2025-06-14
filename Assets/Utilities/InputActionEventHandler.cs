using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class InputActionEventHandler : MonoBehaviour
{
    [SerializeField] private InputActionProperty actionProperty;
    [SerializeField] private UnityEvent OnActionPerformed;

    public bool Performable
    {
        get => actionProperty.action.enabled;
        set
        {
            if(value)
                actionProperty.action.Enable();
            else 
                actionProperty.action.Disable();
        }
    }

    private void Awake()
    {
        Performable = true;
    }

    private void OnEnable()
    {
        actionProperty.action.performed += ActionPerformed;
    }

    private void OnDisable()
    {
        actionProperty.action.performed -= ActionPerformed;
    }

    public void ActionPerformed(InputAction.CallbackContext c)
    {
        if (Performable)
        {
            OnActionPerformed.Invoke();
        }
    }
}