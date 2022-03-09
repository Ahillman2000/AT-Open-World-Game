using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }

    [Range(100, 678)]
    public float drawDistance = 100f;

    public int health = 100;
    public int xp = 0;

    public List<Quest> quests;

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    private void Update()
    {
        foreach(Quest quest in quests)
        {
            if(quest.questType == QuestType.REACH && Vector3.Distance(this.transform.position, quest.questItem.transform.position) <= 10)
            {
                quest.Complete();
            }
        }
    }
}
