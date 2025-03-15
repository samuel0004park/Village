using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeOutBGM : MonoBehaviour
{
    private BGMManager bgmManager;
    private void Start()
    {
        bgmManager = FindFirstObjectByType<BGMManager>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        StartCoroutine(FadeOut());
        this.gameObject.SetActive(false);
    }

    IEnumerator FadeOut()
    {
        bgmManager.FadeOutMusic();

        yield return new WaitForSeconds(3f);
    }
}
