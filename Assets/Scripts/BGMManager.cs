using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMManager : MonoBehaviour
{
    static public BGMManager instance;

    //BGM music clips
    public AudioClip[] clips;

    private AudioSource audioSource;

    //single wait time to be used to enhance performance (avoid creating new waitforsec every iteration)
    private WaitForSeconds waitTime = new WaitForSeconds(0.01f);

    private void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(this.gameObject);
            instance = this;
        }
        else
            Destroy(gameObject);
    }
    void Start()
    {
        audioSource =GetComponent<AudioSource>();
    }

    public void Play(int _playMusicTrack, float _volume=1f)
    {
        audioSource.volume = _volume;
        audioSource.clip = clips[_playMusicTrack];
        audioSource.Play();
    }
    public void Stop()
    {
        audioSource.Stop();
    }
    public void FadeOutMusic(float _volume=0f)
    {
        //stop all coroutines
        StopAllCoroutines();
        //fade out a music using coroutine
        StartCoroutine(FadeOutMusicCoroutine(_volume));
    }

    IEnumerator FadeOutMusicCoroutine(float _volume)
    {
        //in loop, decrease volume and wait for 0.01 seconds to decrease further
        for (float i = audioSource.volume; i >= _volume; i-=0.01f)
        {
            audioSource.volume = i;
            yield return waitTime;
        }
    }
    public void FadeInMusic(float _volume=1f)
    {
        //stop all coroutines
        StopAllCoroutines();

        //fade in a music using coroutine
        StartCoroutine(FadeInMusicCoroutine(_volume));
    }

    IEnumerator FadeInMusicCoroutine(float _volume)
    {
        //in loop, increase volume and wait for 0.01 seconds to increase further
        for (float i = audioSource.volume; i <= _volume; i+=0.01f)
        {
            audioSource.volume = i;
            yield return waitTime;
        }
    }

    public void SetVolume(float _volume)
    {
        audioSource.volume = _volume;
    }

    public void Pause()
    {
        audioSource.Pause();
    }
    public void UnPause()
    {
        audioSource.UnPause();
    }
}
