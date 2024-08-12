using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkManager : MonoBehaviour
{
    Dictionary<int, string[]> talkData;

    private void Awake()
    {
        talkData = new Dictionary<int, string[]>();
        GenerateData();
    }

    public void GenerateData(){
        talkData.Add(1000,new string[] { "당신은 이방인 입니다.", "주민들의 의뢰를 안성하여 신뢰도를 쌓으십시오." });
    }

    public string GetTalk(int id, int talkIndex)
    {
        return talkData[id][talkIndex];
    }
}
