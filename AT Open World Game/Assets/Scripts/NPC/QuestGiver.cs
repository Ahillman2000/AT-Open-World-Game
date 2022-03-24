using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestGiver : MonoBehaviour
{
    public Quest quest;

    private void Start()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        if(enabled && other.CompareTag("Player") && !quest.handedOut)
        {
            HandOutQuest();
        }
    }

    public void HandOutQuest()
    {
        quest.handedOut = true;
        quest.active = true;
        Player.Instance.quests.Add(quest);

        if (quest.questType == QuestType.KILL)
        {
            //quest.questItem.AddComponent<Enemy>();
        }
        else if (quest.questType == QuestType.COLLECT)
        {
            quest.questItem.AddComponent<Collect>();
        }
    }

    void Update()
    {
        
    }
}
