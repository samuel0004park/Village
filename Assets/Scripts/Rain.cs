using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rain : MonoBehaviour
{
    private WeatherManager theWeather;
    public bool flag=true;

    private void Start()
    {
        theWeather = FindObjectOfType<WeatherManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player"&& flag)
        {

            theWeather.StartRain();
            flag = false;
            
        }
    }

}
