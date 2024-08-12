using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class EnemyStat : MonoBehaviour
{
    [Header("Enemy Info")]
    public int hp;
    public int dropItemID;
    public int dropItemCount;
    public float dropRate;
    private int currentHp;
    private bool isDead;

    [Header("References")]
    public RuntimeAnimatorController[] animCon;
    public GameObject prefab_itemdrop;
    private Collider2D coll;
    public UnityEvent onMonsterDeath;

 
    private void Awake()
    {
        coll = GetComponent<Collider2D>();
    }
    private void OnEnable()
    {
        isDead = false;
        currentHp = hp;
        coll.enabled = true;
    }
    public void Init(SpawnData data)
    {
        hp = data.hp;
        currentHp = hp;
        dropItemID = data.dropItemID;
        dropItemCount = data.dropItemCount;
        dropRate = data.dropRate;
        prefab_itemdrop = data.itemDrop_prefab;
    }

    public void Hit()
    {
        if (isDead)
            return;

        currentHp = currentHp - 1;

        //if health hits 0 or lower, die and give exp
        if (currentHp <= 0) {
            EnemyOnDead();
        }
    }

    private void EnemyOnDead()
    {
        if (isDead)
            return;

        // set collider, rigid, live to false
        isDead = true;
        coll.enabled = false;


        // drop item logic
        if (dropItemID != 0)
        {
            DropItem();
        }

        onMonsterDeath.Invoke();

        gameObject.SetActive(false);
    }

    private void DropItem() {

        if (Random.Range(0, 100) <= dropRate)
        {
            var dropitem = Instantiate(prefab_itemdrop, this.gameObject.transform.position, Quaternion.Euler(Vector3.zero));
            dropitem.GetComponent<ItemPickup>().itemID = dropItemID;
            dropitem.GetComponent<ItemPickup>().count = dropItemCount;
        }
    }
}
