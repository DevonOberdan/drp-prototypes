using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    [SerializeField] bool startLocked;

    private int requestCount;

    private void Awake()
    {
        if (startLocked)
        {
            SetLock(true);
        }
    }

    public void SetLock(bool lockState)
    {
        requestCount += lockState ? 1 : -1;
        Cursor.lockState = requestCount > 0 ? CursorLockMode.Locked : CursorLockMode.None;
    }

    public void SetUnlocked(bool unlocked)
    {
        SetLock(!unlocked);
    }
}
