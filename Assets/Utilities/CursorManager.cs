/*
 * Attach to empty object and parent with other managers
 */
 
using UnityEngine;

public class CursorManager : MonoBehaviour
{

    [SerializeField] bool startLocked;

    private void Start()
    {
        SetLock(startLocked);
    }


    //call to set cursor state to confined or locked
    public void SetLock(bool lockState)
    {
        Cursor.lockState = lockState ? CursorLockMode.Locked : CursorLockMode.None;
    }

}
