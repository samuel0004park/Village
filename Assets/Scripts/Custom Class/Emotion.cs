using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Emotion 
{

    public string emotionID;
    public Sprite emotionSprite;

    public Emotion(string _emotionID)
    {
        emotionID = _emotionID;
        emotionSprite = Resources.Load("Emotion/" + emotionID, typeof(Sprite)) as Sprite;
    }


}
