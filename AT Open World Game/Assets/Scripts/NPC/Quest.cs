using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Quest
{
    public bool handedOut;
    public bool active;
    public bool completed;

    public string title;
    public string description;

    public int expReward;

    public QuestType questType;

    public GameObject questItem;

    public void Complete()
    {
        if(!completed)
        {
            Player.Instance.xp += expReward;
            completed = true;
        }
    }
}

public enum QuestType
{
    REACH,
    KILL,
    COLLECT,
}
