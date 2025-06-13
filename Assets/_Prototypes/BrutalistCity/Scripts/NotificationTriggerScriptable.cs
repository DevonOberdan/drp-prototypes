using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NotificationTriggerScriptable : MonoBehaviour
{
    [Header("UI content")]
    [SerializeField] private TMP_Text notificationTextUI;
    [SerializeField] private Image characterIconUI;

    [Header("Scriptable Object")]
    [SerializeField] private NotificationScriptable noteScriptable;

    [Header("Notification Animation")]
    [SerializeField] private Animator notificationAnim;
    private BoxCollider objectCollider;

    private void Awake()
    {
        objectCollider = this.gameObject.GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(EnableNotification());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && noteScriptable.removeAfterExit)
        {
            RemoveNotification();
        }
    }

    IEnumerator EnableNotification()
    {
        notificationAnim.Play("NotificationFadeIn");
        notificationTextUI.text = noteScriptable.notificationMessage;
        characterIconUI.sprite = noteScriptable.yourIcon;

        if (noteScriptable.disableAfterTimer)
        {
            yield return new WaitForSeconds(noteScriptable.disableTimer);
            RemoveNotification();
        }
    }

    void RemoveNotification()
    {
        notificationAnim.Play("NotificationFadeOut");
        gameObject.SetActive(false);
    }
}
