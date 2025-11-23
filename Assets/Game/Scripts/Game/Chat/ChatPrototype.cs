using System;
using UnityEngine;

[CreateAssetMenu]
public class ChatPrototype : ScriptableObject
{
    public PeopleId people;
    [Multiline]
    public string content;
    public string soundName;

    public ChatSpecialAction chatSpecialAction;

    public ChatPrototype next;
    public enum ChatSpecialAction
    {
        None = 0,
        SwitchScene = 10,
        OpenShop_1 = 100,
        OpenShop_2 = 101,
        OpenShop_3 = 102,

        ShowInteraction_begger = 110,
        ShowInteraction_streetBoy = 115,
        ShowInteraction_teacher = 120,
        ShowInteraction_lover = 125,
        ShowInteraction_exam = 130,
    }

    public string flag;

    public ChatOptionData[] options;
}

[Serializable]
public class ChatOptionData
{
    public string txt;
    public ChatPrototype next;
}

public enum PeopleId
{
    None = 0,
    Anna =17,
    Terry =19,
    Ethan = 1,
    StrangerMark = 11,
    Lupita = 12,
    BBQRobot = 13,
    Nora = 21,
    Natasha = 22,
    V = 23,
    Monk = 30,
    NAPD =31,
    Worktable =23,
    Balcony =14,
    IDCard =15,
    Pistol =26,
    NeonGirl =69,
}