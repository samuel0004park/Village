using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherManager : MonoBehaviour
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
    static public WeatherManager instance;

    private AudioManager theAudio;
    public ParticleSystem rain;
    public string rain_sound;
    
    void Start()
    {
        theAudio = FindObjectOfType<AudioManager>();
    }

    public void Rain()
    {
        theAudio.Play(rain_sound);
        rain.Play();
    }
    public void StopRain()
    {
        theAudio.Stop(rain_sound);
        rain.Stop();
    }
}
