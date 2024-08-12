using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapActivated : MonoBehaviour
{
    public int id;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            TrapLogic();
        }
    }
    private void TrapLogic()
    {
        if (id != 0)
        {
            QuestObjectManager theQuest = FindObjectOfType<QuestObjectManager>();
            theQuest.ShowQuestObject(id);

        }
        PlayerStat.instance.Die();
    }
}
