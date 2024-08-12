using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEmotion : MonoBehaviour
{

    public bool flag = false;

    public string emotionID;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!flag && collision.gameObject.name == "Player")
        {
            flag = true;
            SpriteRenderer temp = collision.transform.Find("Emotion").GetComponent<SpriteRenderer>();

            EmotionManager.instance.ShowEmotion(temp, emotionID,2f);

        }
    }
}
