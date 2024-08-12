using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransferMap : MonoBehaviour
{
    [SerializeField] private Location.SceneNames transferSceneName; //destination name the player will be transfered to 
    [SerializeField] private Location.MapNames transferMapName;

    private FadeManager theFade;

    private void Start()
    {
        theFade = FindObjectOfType<FadeManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            StartCoroutine(TransferCoroutine());
        }

    }
      
    IEnumerator TransferCoroutine()
    {
        //stop player movement and fade out scene
        OrderManager.instance.ForceStop(1);
        theFade.FadeOut();

        yield return new WaitForSeconds(0.7f);//wait so fade in does not occur instantly

        //change the name of the currentMapName to the destination name
        PlayerManager.instance.currentSceneName = transferSceneName;
        PlayerManager.instance.currentMapName = transferMapName;

        //load new scene
        GameManager.instance.loading = true;
        SceneManager.LoadScene(transferSceneName.ToString());

        //fadein and let player move
        theFade.FadeIn();
    }
}
