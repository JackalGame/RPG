using RPG.Quests;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestListUI : MonoBehaviour
{
    [SerializeField] Quest[] tempQuests;
    [SerializeField] QuestItemUI questPrefab;

    // Start is called before the first frame update
    void Start()
    {
        transform.DetachChildren();
        foreach (Quest quest in tempQuests)
        {
            QuestItemUI uiInstance = Instantiate(questPrefab, transform);
            uiInstance.Setup(quest);
        }
    }
}
