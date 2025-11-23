using System.Collections;
using UnityEngine;

public class ChatTrigger : MonoBehaviour
{
    public ChatPrototype chat;

    public int maxTriggerTimes = 1;
    int _triggeredTimes = 0;
    public bool turnbackWhenShowDialog = true;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_triggeredTimes >= maxTriggerTimes)
            return;

        if (other.gameObject.tag == "Player")
        {
            Trigger();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_triggeredTimes >= maxTriggerTimes)
            return;

        if (other.gameObject.tag == "Player")
        {
            Trigger();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (ChatHintBehaviour.instance != null)
                ChatHintBehaviour.instance.Hide();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (ChatHintBehaviour.instance != null)
                ChatHintBehaviour.instance.Hide();
        }
    }

    private void Trigger()
    {
        if (ChatHintBehaviour.instance != null)
        {
            ChatHintBehaviour.instance.Show(this);
        }
        else
        {
            ShowDialog();
        }
    }

    public void ShowDialog()
    {
        PlayerBehaviour.instance.turnback.TryTurnBack();
        ChatSystem.instance.ShowChat(chat);
        _triggeredTimes++;
    }
}