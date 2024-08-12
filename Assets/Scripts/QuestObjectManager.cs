using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestObjectManager : MonoBehaviour
{
    public GameObject[] questObject;

    public void ShowQuestObject(int _id)
    {
        questObject[_id].SetActive(true);
    }
    public void DisableDialogue(int _id)
    {
        questObject[_id].GetComponent<ObjectDialogue>().SetDisposable();
    }
}
