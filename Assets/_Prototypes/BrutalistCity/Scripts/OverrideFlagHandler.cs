using FinishOne.GeneralUtilities;
using System.Collections.Generic;
using System.Linq;

public class OverrideFlagHandler
{
    private List<bool> StatusFlags = new();

    public bool AnyFlags => StatusFlags.Where(f => f).Any();

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
        StatusFlags.Add(flag);
        return StatusFlags.Count-1;
    }

    public void RemoveFlag(int idx)
    {
        StatusFlags.RemoveAt(idx);
    }
}
