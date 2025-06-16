using UnityEngine;

public class CursorManager : MonoBehaviour
{
    [SerializeField] bool startLocked;

    private void Start()
    {
        SetLock(startLocked);
    }

    public void SetLock(bool lockState)
    {
        Cursor.lockState = lockState ? CursorLockMode.Locked : CursorLockMode.None;
        Debug.Log(Cursor.lockState);
    }

    public void SetUnlocked(bool unlocked)
    {
        SetLock(!unlocked);
    }
}
