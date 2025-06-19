using UnityEngine;

[CreateAssetMenu(fileName = nameof(NotificationSO), menuName = "FinishOne/" + nameof(NotificationSO), order = 0)]
public class NotificationSO : ScriptableObject
{
    [Header("Message Customisation")]
    [field: SerializeField] public Sprite Icon { get; private set; }

    [TextArea] [field: SerializeField] public string NotificationMessage { get; private set; }

    [Header("Notification Removal")]
    [field: SerializeField] public bool RemoveAfterExit { get; private set; } = false;
    [field: SerializeField] public bool DisableAfterTimer { get; private set; } = false;
    [field: SerializeField] public float DisableTime { get; private set; } = 1.0f;

    [field: SerializeField] public AudioClip VoiceClip { get; private set; }

    [MinMaxSlider(-3, 3)]
    [SerializeField] private Vector2 pitchRange;

    public Vector2 PitchRange => pitchRange;
}
