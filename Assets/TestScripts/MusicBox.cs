using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicBox : MonoBehaviour
{

    private BGMManager bgmManager;

    public int playMusicTrack;

    private void Start()
    {
        bgmManager = FindFirstObjectByType<BGMManager>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        bgmManager.Play(playMusicTrack);
        this.gameObject.SetActive(false);
    }
}
