using UnityEngine;

[CreateAssetMenu(fileName = nameof(Vector3Atom), menuName = "Atoms/"+nameof(Vector3Atom), order =0)]
public class Vector3Atom : ScriptableObject
{
    public Vector3 Value;
}
