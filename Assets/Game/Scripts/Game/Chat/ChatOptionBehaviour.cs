using System.Collections;
using TMPro;
using UnityEngine;


public class ChatOptionBehaviour : MonoBehaviour
{
    public TextMeshProUGUI txt;
    public ChatOptionData data { get; private set; }

    public void Initialize(ChatOptionData d)
    {
        data = d;
        txt.text = d.txt;
    }


    public void OnClickOption()
    {
        Debug.Log("OnClickOption");
        ChatSystem.instance.OnClickOption(this);
    }
}