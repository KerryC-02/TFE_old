using DG.Tweening;
using System.Collections;
using UnityEngine;

public class ChatHintBehaviour : MonoBehaviour
{
    public CanvasGroup cg;
    private ChatTrigger _chatTrigger;
    public static ChatHintBehaviour instance;
    private void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (cg.alpha == 0)
            return;

        if (Input.GetKeyDown("e"))
        {
            TriggerChat();

            cg.DOKill();
            cg.alpha = 0;
        }
    }

    public void Show(ChatTrigger chatTrigger)
    {
        cg.DOFade(1, 0.5f);
        _chatTrigger = chatTrigger;
    }

    public void Hide()
    {
        cg.DOKill();
        cg.alpha = 0;
        _chatTrigger = null;
    }

    void TriggerChat()
    {
        _chatTrigger.ShowDialog();
    }
}