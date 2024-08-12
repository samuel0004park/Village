using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtEnemy : MonoBehaviour
{
    [Header("References")]
    public GameObject parent;
    public GameObject prefab_Effect;
    
    [Header("Values")]
    private string monster_sound = "melee_sound";
    private string bush_sound = "bush_sound";
    private string box_sound = "box_sound";
    public bool wait=false;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!wait)
        {
            switch (collision.gameObject.tag){
                case "enemy":
                    SwingWeapon(collision, monster_sound);
                    break;
                case "bush":
                    SwingWeapon(collision, bush_sound);
                    break;
                case "box":
                    SwingWeapon(collision, box_sound);
                    break;
            }
        }

    }
    private void SwingWeapon(Collider2D collision, string _sound)
    {
        wait = true;
        collision.gameObject.GetComponent<EnemyStat>().Hit();
        AudioManager.instance.Play(_sound);

        StartCoroutine(WaitCoroutine());
    }

    IEnumerator WaitCoroutine()
    {
        yield return new WaitUntil(() => !PlayerManager.instance.attacking);
        wait = false;
    }
 
}
