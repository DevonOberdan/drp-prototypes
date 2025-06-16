using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NotificationTriggerEvent : MonoBehaviour
{
    [Header("UI content")]
    [SerializeField] private TMP_Text notificationTextUI;
    [SerializeField] private Image characterIconUI;

    [Header("Message Customisation")]
    [SerializeField] private Sprite yourIcon;
    [SerializeField][TextArea] private string notificationMessage;

    [Header("Notification Removal")]
    [SerializeField] private bool removeAfterExit = false;
    [SerializeField] private bool disableAfterTimer = false;
    [SerializeField] float disableTimer = 1.0f;

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
        if (other.CompareTag("Player") && removeAfterExit)
        {
            RemoveNotification();
        }
    }

    IEnumerator EnableNotification()
    {
        notificationAnim.Play("NotificationFadeIn");
        notificationTextUI.text = notificationMessage;
        characterIconUI.sprite = yourIcon;

        if(disableAfterTimer)
        {
            yield return new WaitForSeconds(disableTimer);
            RemoveNotification();
        }
    }

    void RemoveNotification()
    {
        notificationAnim.Play("NotificationFadeOut");
        gameObject.SetActive(false);
    }
}
