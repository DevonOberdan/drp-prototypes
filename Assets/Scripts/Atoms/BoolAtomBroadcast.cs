using UnityEngine;
using UnityEngine.Events;

public class BoolAtomBroadcast : MonoBehaviour
{
    [SerializeField] private BoolAtom atom;

    public UnityEvent<bool> BroadcastValue;

    public void Broadcast()
    {
        if (atom == null)
            return;

        BroadcastValue.Invoke(atom.Value);
    }
}
