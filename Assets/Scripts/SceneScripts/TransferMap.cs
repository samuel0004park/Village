using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransferMap : MonoBehaviour
{
    [SerializeField] private Location.SceneNames transferSceneName; //destination name the player will be transfered to 
    [SerializeField] private Location.MapNames transferMapName;

    private FadeManager theFade;

    public static event EventHandler OnTransformMapEvent;

    private void Start()
    {
        theFade = FindObjectOfType<FadeManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            StartCoroutine(TransferCoroutine());
        }

    }
      
    IEnumerator TransferCoroutine()
    {
        FadeManager.Instance.FadeOut();
        yield return new WaitForSeconds(0.7f);//wait so fade in does not occur instantly
        FadeManager.Instance.FadeIn();

        PlayerManager.Instance.SetLocationInfo(transferSceneName, transferMapName);
        SceneManager.LoadScene(transferSceneName.ToString());

        OnTransformMapEvent?.Invoke(this, EventArgs.Empty);
    }
}
