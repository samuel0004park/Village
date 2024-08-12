using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestData 
{
   
    public Location.MapNames[] location;
    public string questName;
    public int[] npcId;
    public QuestData(string _name,int[] _npc, Location.MapNames[] _npcLocation)
    {
        questName = _name;
        npcId = _npc;
        location = _npcLocation;
    }
}
