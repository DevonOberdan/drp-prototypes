using FinishOne.GeneralUtilities;
using System.Collections.Generic;

public class OverrideFlagHandler
{
    private List<bool> StatusFlags = new();

    public bool AnyFlags
    {
        get 
        {
            for(int i=0; i<StatusFlags.Count; i++)
            {
                if(!RemovedFlags.Contains(i) && StatusFlags[i])
                {
                    return true;
                }
            }

            return false;
        }
    }

    private List<int> RemovedFlags = new();

    public void SetFlag(int flag, bool value)
    {
        if (!StatusFlags.IsValidIndex(flag))
        {
            return;
        }

        StatusFlags[flag] = value;
    }

    public int AddFlag(bool flag=false)
    {
        int newIdx = GetFirstAvailableSlot();

        if (newIdx == StatusFlags.Count)
        {
            StatusFlags.Add(flag);
        }

        return newIdx;
    }

    public void RemoveFlag(int idx)
    {
        StatusFlags[idx] = false;
        RemovedFlags.Add(idx);
    }

    public void Clear()
    {
        StatusFlags.Clear();
        RemovedFlags.Clear();
    }

    private int GetFirstAvailableSlot()
    {
        for (int i = 0; i < StatusFlags.Count; i++)
        {
            if (RemovedFlags.Contains(i))
            {
                RemovedFlags.Remove(i);
                return i;
            }
        }

        return StatusFlags.Count;
    }
}
