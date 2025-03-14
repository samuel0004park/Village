using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherManager : MonoBehaviour
{
    static public WeatherManager Instance;

    [SerializeField] private ParticleSystem rain;


    public event EventHandler<OnWeatherChangedEventArgs> OnWeatherChangedEvent;
    public class OnWeatherChangedEventArgs : EventArgs {
        public string weather;
    }



    private void Awake() {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }


    public void StartRain()
    {
        OnWeatherChangedEvent?.Invoke(this, new OnWeatherChangedEventArgs { weather= "rain_sound" });
        rain.Play();
    }


    public void StopRain()
    {
        OnWeatherChangedEvent?.Invoke(this, new OnWeatherChangedEventArgs { weather = "" });
        rain.Stop();
    }
}
