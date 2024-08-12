using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmotionManager : MonoBehaviour

{
    #region Singleton
    private void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(this.gameObject);
            instance = this;
        }
        else
            Destroy(this.gameObject);
    }
    #endregion Singleton
    static public EmotionManager instance;

    [Header("Data Structures")]
    public List<Emotion> emotionList = new List<Emotion>(); //list that contains all items in game

    private void Start()
    {
        //load all emotions in resources
        emotionList.Add(new Emotion("angry"));
        emotionList.Add(new Emotion("talk"));
        emotionList.Add(new Emotion("confused"));
        emotionList.Add(new Emotion("exclamation"));
        emotionList.Add(new Emotion("love"));
        emotionList.Add(new Emotion("question"));
        emotionList.Add(new Emotion("tear"));
        emotionList.Add(new Emotion("death"));
    }

    public void ShowEmotion(SpriteRenderer _sprite, string _emotionID, float _time = 0f)
    {
        //find appropriate emotion from list and change emotion
        for (int i = 0; i < emotionList.Count; i++)
        {
            if (emotionList[i].emotionID == _emotionID)
                _sprite.sprite = emotionList[i].emotionSprite;
        }
        //show emotion for a period of time
        StartCoroutine(StartEmotionCoroutine(_sprite,_time));
    }

    public void HideEmotion(SpriteRenderer _sprite)
    {
        StopAllCoroutines();
        Color temp = _sprite.color;
        temp.a = 0f;
        _sprite.color = temp;
    }

    IEnumerator StartEmotionCoroutine(SpriteRenderer _sprite,float _time)
    {
        //show sprite
        Color temp = _sprite.color;
        temp.a = 1f;
        _sprite.color = temp;

        if(_time != 0f)
        {
            //show for given time
            yield return new WaitForSeconds(_time);

            //hide sprite
            temp.a = 0f;
            _sprite.color = temp;
        }
    }
}
