using UnityEngine;

[CreateAssetMenu(fileName = nameof(BoolAtom), menuName = "Atoms/" + nameof(BoolAtom), order = 0)]
public class BoolAtom : ScriptableObject
{
    [field: SerializeField] public bool Value { get; set; }
}
