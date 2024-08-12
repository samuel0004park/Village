using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public int questId;//current quest
    public int questActionIndex; //current index on quest
   
    Dictionary<int, QuestData> questList;

    #region Singleton
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);

        questList = new Dictionary<int, QuestData>();
        GenerateData();
    }
    static public QuestManager instance;
    #endregion 

    private void GenerateData()
    {
        questList.Add(10, new QuestData("루나와 루도 만나기", 
            new int[] { 2000, 3000 },
            new Location.MapNames[]{ Location.MapNames.StartingIsland, Location.MapNames.StartingIsland }));
        questList.Add(20, new QuestData("신사에 바칠 동전 찾기", 
            new int[] { 3000, 4000},
            new Location.MapNames[] { Location.MapNames.StartingIsland, Location.MapNames.StartingIsland, Location.MapNames.StartingIsland }));
        questList.Add(30, new QuestData("마을 조사하기", 
            new int[] { 3000, 1000, 1000, 5000, 4000, },
            new Location.MapNames[] { Location.MapNames.VillageTop , Location.MapNames.VillageTop, Location.MapNames.VillageTop, Location.MapNames.VillageTop, Location.MapNames.VillageTop}));
        questList.Add(40, new QuestData("비밀번호 알아내기",
          new int[] { 3000},
          new Location.MapNames[] {Location.MapNames.KingsRoadTop}));
        questList.Add(50, new QuestData("대장간 방문하기",
          new int[] { 3000, 2000, 3000, 4000, 2000 },
          new Location.MapNames[] { Location.MapNames.CityTop, Location.MapNames.CityTop, Location.MapNames.CityTop, Location.MapNames.CityBottom, Location.MapNames.CityBottom }));
        questList.Add(60, new QuestData("몬스터 처치하기",
          new int[] { 2000,4000 },
          new Location.MapNames[] { Location.MapNames.CityBottom , Location.MapNames.CityBottom }));
        questList.Add(70, new QuestData("루도와 공원에서 만나기",
         new int[] { 3000,3000 },
         new Location.MapNames[] { Location.MapNames.CityBottom, Location.MapNames.CityBottom }));
        questList.Add(80, new QuestData("앞으로 나아가기",
         new int[] { 5000,0 },
         new Location.MapNames[] { Location.MapNames.Cave, Location.MapNames.Cave }));
    }
    public void ResetAll()
    {
        questId = 10;
        questActionIndex = 0;
    }
    public int GetMapName()
    {
        return (int)questList[questId].location[questActionIndex];
    }
    public int GetQuestTalkIndex()
    {
        // GetQuestTalkIndex only called when in correct quest dialogue, so increment quest action index
        int temp_quest = questId;
        int temp_index = questActionIndex++;

        //if at the end of current quest, go to next quest
        if (questActionIndex == questList[questId].npcId.Length)
            NextQuest();

        //Make quest objects visible
        ControlObject();

        //return old info
        return temp_quest + temp_index;
    }

    public bool CheckQuest(int _id)
    {
        bool flag = false;

        //if talking to correct npc and correct location, increment index 
        if (_id == questList[questId].npcId[questActionIndex] && (int)PlayerManager.instance.currentMapName == GetMapName())
        {
            flag = true;
            //if beginning of quest, show quest name 
            if (questActionIndex == 0)
                Debug.Log(string.Format("퀘스트: {0}",questList[questId].questName));
        }
        return flag;
    }

    public void CheckQuest()
    {
         Debug.Log(string.Format("퀘스트: {0}", questList[questId].questName));
    }
    public void ControlObject()
    {
        QuestObjectManager questObjectManager = FindObjectOfType<QuestObjectManager>();
        switch (questId)
        {
            case 20:
                if (questActionIndex == 1)
                    questObjectManager.ShowQuestObject(0);
                break;
            case 30:
                if(questActionIndex == 4)
                    questObjectManager.ShowQuestObject(1);
                break;
            case 70:
                if (questActionIndex == 1)
                {
                    questObjectManager.ShowQuestObject(4);
                    questObjectManager.DisableDialogue(5);
                }
                break;
            default:
                break;
        }
    }
    private void NextQuest()
    { 
        questId += 10;
        questActionIndex = 0;
    }
}
