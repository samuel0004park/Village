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
        talkData.Add(1000,new string[] { "����� �̹��� �Դϴ�.", "�ֹε��� �Ƿڸ� �ȼ��Ͽ� �ŷڵ��� �����ʽÿ�." });
    }

    public string GetTalk(int id, int talkIndex)
    {
        return talkData[id][talkIndex];
    }
}
