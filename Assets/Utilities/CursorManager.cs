using UnityEngine;

public class CursorManager : MonoBehaviour
{
    [SerializeField] bool startLocked;

    private void Awake()
    {
        SetLock(startLocked);
    }

    public void SetLock(bool lockState)
    {
        Cursor.lockState = lockState ? CursorLockMode.Locked : CursorLockMode.None;
    }

    public void SetUnlocked(bool unlocked)
    {
        SetLock(!unlocked);
    }
}
