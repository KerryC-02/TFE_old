using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChatSystem : MonoBehaviour
{
    public static ChatSystem instance { get; private set; }

    private ChatPrototype _chat;

    public string flag;


    private void Awake()
    {
        instance = this;
    }


    public ChatPrototype testStartingChat;

    private void Start()
    {
        ChatPanelBehaviour.instance.Hide();

        StartCoroutine(TestChat());
    }

    IEnumerator TestChat()
    {
        yield return new WaitForSeconds(1);
        if (testStartingChat != null)
            ShowChat(testStartingChat);
    }

    public void ShowChat(ChatPrototype chat)
    {
        if (chat == null)
        {
            Debug.LogWarning("show empty chat!");
            return;
        }

        PauseSystem.instance.Pause();
        _chat = chat;
        flag = _chat.flag;
        ChatPanelBehaviour.instance.Show(_chat);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ChatPanelBehaviour.instance.UserTapped();
        }
    }

    public void EndChat()
    {
        PauseSystem.instance.Resume();

        ChatPanelBehaviour.instance.Hide();

        switch (_chat.chatSpecialAction)
        {
            case ChatPrototype.ChatSpecialAction.None:
                break;

            case ChatPrototype.ChatSpecialAction.SwitchScene:
                SceneManager.LoadScene(flag);
                break;
        }
        if (flag == "kick girl")
        {

        }

        _chat = null;
        PlayerBehaviour.instance.turnback.TryReverseTurnBack();
    }

    public bool IsChating()
    {
        return _chat != null;
    }

    public void OnChatEnd()
    {
        if (_chat == null)
            return;
        flag = _chat.flag;

        if (_chat.next == null)
        {
            if (_chat.options.Length > 0)
            {
                //这个对话没有选项，但是有事先配置好的下一个对话，等待用户点击选项
                ShowOptions();
                _chat = null;
                return;
            }
            else
            {
                //这个对话没有选项，也没有事先配置好的下一个对话，点击结束对话
                EndChat();
                return;
            }

        }
        //这个有事先配置好的下一个对话，点击播放下一个对话
        ShowChat(_chat.next);
    }


    public ChatOptionBehaviour optionPrefab;//对话的选项的预制体
    List<ChatOptionBehaviour> _options = new List<ChatOptionBehaviour>();
    void ShowOptions()
    {
        ClearOptions();

        foreach (var optionData in _chat.options)
        {
            var newOption = Instantiate(optionPrefab, optionPrefab.transform.parent);
            newOption.Initialize(optionData);
            newOption.gameObject.SetActive(true);
            _options.Add(newOption);
        }
    }

    public void OnClickOption(ChatOptionBehaviour optionBehaviour)
    {
        ClearOptions();
        ShowChat(optionBehaviour.data.next);
    }

    void ClearOptions()
    {
        foreach (var opt in _options)
        {
            Destroy(opt.gameObject);
        }
        _options = new List<ChatOptionBehaviour>();
    }
}