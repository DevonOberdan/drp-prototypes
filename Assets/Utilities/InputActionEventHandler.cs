using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class InputActionEventHandler : MonoBehaviour
{
    [SerializeField] private InputActionProperty actionProperty;
    [SerializeField] private UnityEvent OnActionPerformed;

    public bool Performable { get; set; }

    private void Awake()
    {
        Performable = true;
        actionProperty.action.performed += ActionPerformed;
    }

    public void ActionPerformed(InputAction.CallbackContext c)
    {
        if (Performable)
        {
            OnActionPerformed.Invoke();
        }
    }
}