using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestGiver : MonoBehaviour
{
    public Quest quest;

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
        Player.Instance.quests.Add(quest);
    }

    void Update()
    {
        
    }
}
