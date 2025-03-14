using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmotionManager : MonoBehaviour
{
    public enum Emotion {NONE, TEAR, DEATH}
    static public EmotionManager Instance;

    [SerializeField] private List<Sprite> emotionList = new List<Sprite>(); 
    private Dictionary<string, Sprite> emotionDictionary = new Dictionary<string, Sprite>();
    
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        SubscribeEvents();
        SetUp();
    }

    private void OnDestroy() {
        UnSubscribeEvents();
    }

    private void SubscribeEvents() {
        PlayerStat.OnPlayerEmotionChangedEvent += PlayerStat_OnPlayerEmotionChangedEvent;
    }
    
    private void UnSubscribeEvents() {
        PlayerStat.OnPlayerEmotionChangedEvent -= PlayerStat_OnPlayerEmotionChangedEvent;
    }


    private void PlayerStat_OnPlayerEmotionChangedEvent(object sender, PlayerStat.OnPlayerEmotionChangedEventArgs e) {
        if (e.emotion == Emotion.NONE)
            HideEmotion(e.sprite);
        else
            ShowEmotion(e.sprite, e.emotion,3f);
    }

   
    private void SetUp() {
        foreach (var emotion in emotionList) {
            emotionDictionary.Add(emotion.name, emotion);
        }
    }

    public void ShowEmotion(SpriteRenderer _sprite, Emotion _emotion, float _time = 0f)
    {
        //find appropriate emotion from list and change emotion
        _sprite.sprite = emotionDictionary[_emotion.ToString()];

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
