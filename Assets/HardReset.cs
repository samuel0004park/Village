using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HardReset : MonoBehaviour
{
    private WeatherManager weatherManager;

    private void Start()
    {
        weatherManager = FindObjectOfType<WeatherManager>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            weatherManager.StopRain();
            QuestManager.instance.ResetAll();
            Inventory.instance.EmptyInventory();
            PlayerStat.instance.Heal_HP(3);
        }
    }
}
