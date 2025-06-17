using System;
using UnityEngine;

public static class AwaitableMethods
{
    public static async Awaitable WaitUntil(Func<bool> condition)
    {
        while (!condition())
            await Awaitable.NextFrameAsync(); // or await Awaitable.FixedUpdateAsync();
    }
}
