using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class NotificationUIManager : MonoBehaviour
{
    [Header("UI content")]
    [SerializeField] private TMP_Text textField;
    [SerializeField] private Image image;

    [SerializeField] private UnityEvent OnQueueStart;
    [SerializeField] private UnityEvent OnQueueComplete;
    [SerializeField] private UnityEvent<bool> OnQueueActive;

    private Queue<NotificationSO> queue;
    private CanvasGroup group;

    private bool continueText;
    private bool canContinue;

    private AudioSource audioSource;

    private NotificationSO currentNotification;

    private void Awake()
    {
        group = GetComponent<CanvasGroup>();
        queue = new Queue<NotificationSO>();

        audioSource = GetComponent<AudioSource>();

        continueText = true;
    }

    public void QueueNotification(NotificationSO notification)
    {
        queue.Enqueue(notification);
        if (queue.Count == 1)
        {
            group.alpha = 1;
            group.blocksRaycasts = true;

            OnQueueStart.Invoke();
            OnQueueActive.Invoke(true);

            GetNextNotification();
        }
    }

    private async void GetNextNotification()
    {
        while(queue.Count > 0)
        {
            currentNotification = queue.Peek();
            continueText = false;
            canContinue = false;

            await DisplayNotification(currentNotification);
            
            canContinue = true;
            queue.Dequeue();

            await AwaitableMethods.WaitUntil(() => (!PauseManager.PauseState || currentNotification.PlayWhenPaused) && continueText);
        }

        Close();
    } 

    private async Awaitable DisplayNotification(NotificationSO notification)
    {
        image.color = notification.Character != null ? Color.white : Color.clear;
        image.sprite = notification.Character != null ? notification.Character.Icon : null;
        await textField.GetComponent<ITextDisplay>().SetText(notification, audioSource);
    }

    public void Continue()
    {
        if (PauseManager.PauseState && !currentNotification.PlayWhenPaused)
        {
            return;
        }

        if (canContinue)
        {
            continueText = true;
            if (audioSource != null && audioSource.clip != null)
            {
                audioSource.Stop();
            }
        }
        else
        {
            textField.GetComponent<ITextDisplay>().CompleteText();
        }
    }

    private void Close()
    {
        group.alpha = 0;
        group.blocksRaycasts = false;

        OnQueueComplete.Invoke();
        OnQueueActive.Invoke(false);
    }
}
