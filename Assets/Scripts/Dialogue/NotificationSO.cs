using UnityEngine;

[CreateAssetMenu(fileName = nameof(NotificationSO), menuName = "FinishOne/" + nameof(NotificationSO), order = 0)]
public class NotificationSO : ScriptableObject
{
    [field: SerializeField] public CharacterSO Character { get; private set; }
    [TextArea] [field: SerializeField] public string NotificationMessage { get; private set; }
    [field: SerializeField] public bool PlayWhenPaused { get; private set; }
}
