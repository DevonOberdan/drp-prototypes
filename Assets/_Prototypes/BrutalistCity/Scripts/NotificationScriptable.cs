using UnityEngine;

[CreateAssetMenu(fileName = "NotificationSc")]
public class NotificationScriptable : ScriptableObject
{
    [Header("Message Customisation")]
    public Sprite yourIcon;
    [TextArea] public string notificationMessage;

    [Header("Notification Removal")]
    [SerializeField] public bool removeAfterExit = false;
    [SerializeField] public bool disableAfterTimer = false;
    [SerializeField] public float disableTimer = 1.0f;
}
