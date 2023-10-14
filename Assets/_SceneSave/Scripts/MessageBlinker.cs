using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageBlinker : MonoBehaviour
{
    [SerializeField]
    private Text messageBlinker;

    [SerializeField]
    private float displayDuration;

    public void ShowMessage(string message)
    {
        StartCoroutine(ShowMessageAndAutoHide(message));
    }

    IEnumerator ShowMessageAndAutoHide(string message)
    {
        messageBlinker.gameObject.SetActive(true);
        messageBlinker.text = message;

        yield return new WaitForSeconds(displayDuration);

        messageBlinker.text = string.Empty;
        messageBlinker.gameObject.SetActive(false);
    }
}
