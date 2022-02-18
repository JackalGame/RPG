using RPG.Quests;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestListUI : MonoBehaviour
{
    [SerializeField] QuestItemUI questPrefab;

    // Start is called before the first frame update
    void Start()
    {
        transform.DetachChildren();
        QuestList questList = GameObject.FindGameObjectWithTag("Player").GetComponent<QuestList>();
        foreach (QuestStatus status in questList.GetStatuses())
        {
            QuestItemUI uiInstance = Instantiate(questPrefab, transform);
            uiInstance.Setup(status);
        }
    }
}
