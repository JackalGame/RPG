using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Quests;
using TMPro;

public class QuestItemUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI title;
    [SerializeField] TextMeshProUGUI progress;

    Quest quest;

    public void Setup(Quest quest)
    {
        this.quest = quest;
        title.text = quest.GetTitle();
        progress.text = $"0/{quest.GetObjectiveCount()}";
    }

    public Quest GetQuest()
    {
        return quest;
    }
}
