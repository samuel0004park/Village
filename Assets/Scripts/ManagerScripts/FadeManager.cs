using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeManager : MonoBehaviour
{
    public static FadeManager Instance;
    
    public SpriteRenderer white;
    public SpriteRenderer black;

    private Color color;
    private WaitForSeconds waitTime = new WaitForSeconds(0.01f);

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        }
        else {
            Destroy(Instance.gameObject);
        }
    }


    private void Start() {
        SubscribeEvents();
    }

    private void OnDestroy() {
        UnSubscribeEvents();
    }


    private void SubscribeEvents() {
        GameManager.Instance.OnStartGameEvent += Instance_OnStartGameEvent;
    }

    private void UnSubscribeEvents() {
        GameManager.Instance.OnStartGameEvent -= Instance_OnStartGameEvent;
    }

    private void Instance_OnStartGameEvent(object sender, System.EventArgs e) {
        FadeIn();
    }

    public void FadeOut(float _speed = 0.02f)
    {
        StopAllCoroutines();
        StartCoroutine(FadeOutCoroutine(_speed));
    }

    IEnumerator FadeOutCoroutine(float _speed)
    {
        color = black.color;

        while(color.a < 1f)
        {
            color.a += _speed;
            black.color = color;
            yield return waitTime;
        }
    }
    public void FadeIn(float _speed = 0.02f)
    {
        StopAllCoroutines();
        StartCoroutine(FadeInCoroutine(_speed));
    }

    IEnumerator FadeInCoroutine(float _speed)
    {
        color = black.color;

        while (color.a > 0f)
        {
            color.a -= _speed;
            black.color = color;
            yield return waitTime;
        }
    }
    public void Flash(float _speed = 0.1f)
    {
        StopAllCoroutines();
        StartCoroutine(FlashCoroutine(_speed));
    }
    IEnumerator FlashCoroutine(float _speed)
    {
        color = white.color;

        while (color.a < 1f)
        {
            color.a += _speed;
            white.color = color;
            yield return waitTime;
        }
        while (color.a > 0f)
        {
            color.a -= _speed;
            white.color = color;
            yield return waitTime;
        }
    }
    public void FlashOut(float _speed = 0.02f)
    {
        StopAllCoroutines();
        StartCoroutine(FlashOutCoroutine(_speed));
    }
    IEnumerator FlashOutCoroutine(float _speed)
    {
        color = white.color;

        while (color.a < 1f)
        {
            color.a += _speed;
            white.color = color;
            yield return waitTime;
        }
    }
    public void FlashIn(float _speed = 0.02f)
    {
        StopAllCoroutines();
        StartCoroutine(FlashInCoroutine(_speed));
    }

    IEnumerator FlashInCoroutine(float _speed)
    {
        color = white.color;

        while (color.a > 0f)
        {
            color.a -= _speed;
            white.color = color;
            yield return waitTime;
        }
    }
}
