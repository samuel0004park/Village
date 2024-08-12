using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeInBGM : MonoBehaviour
{
    private BGMManager bgmManager;
    public int playMusicTrack;

    public bool flag=false;
    private void Start()
    {
        bgmManager = FindObjectOfType<BGMManager>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!flag)
        {
            flag = !flag;
            StartCoroutine(FadeIn());
        }
    }

    public void StartBGM()
    {
        StartCoroutine(FadeIn());
    }


    IEnumerator FadeIn()
    {
        bgmManager.Play(playMusicTrack,0f);
        bgmManager.FadeInMusic(0.2f);

        yield return new WaitForSeconds(3f);
    }
}
