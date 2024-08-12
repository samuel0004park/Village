using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("Timer Variables")]
    private float timer;
    private float nextSpawnTime;
    [SerializeField] private float min_spawnRate;
    [SerializeField] private float max_spawnRate;

    [Header("Spawn Variables")]
    [SerializeField] private int deaths;
    [SerializeField] private int death_Goal;
    [SerializeField] private int maxSpawn;
    [SerializeField] private int spawn_Index;
    [SerializeField] private int spawn_count;
    private bool isActivated;
    private LayerMask slime_layerMask;
    private LayerMask player_layerMask;

    [Header("References")]
    private Transform[] spawnPoint;
    private List<GameObject>[] pools;
    public GameObject[] monster_prefabs;
    public SpawnData[] spawnData;

    private void Awake()
    {
        isActivated = true;
        timer = 0;
        deaths = 0;
        spawn_count = 0;
        nextSpawnTime = Random.Range(min_spawnRate, max_spawnRate);
        // get layermask
        slime_layerMask = LayerMask.GetMask("NoPass");
        player_layerMask = LayerMask.GetMask("Player");
        spawnPoint = GetComponentsInChildren<Transform>(); //get all transform of spawnpoints

        //initialize list of pools
        pools = new List<GameObject>[monster_prefabs.Length];

        for (int i = 0; i < monster_prefabs.Length; i++)
        {
            pools[i] = new List<GameObject>();
        }
    }

    public void IncrementDeath()
    {
        if (PlayerStat.instance.isDead)
            return;
        spawn_count--;
        deaths++;

        if (death_Goal!=0 && deaths >= death_Goal)
        {
            DeathAccomplish();
        }
    }

    public void DeathAccomplish()
    {
        if (PlayerStat.instance.isDead || !isActivated)
            return;

        isActivated = false;

        //if there is a fence component, open it 
        OpenFence fence = GetComponent<OpenFence>();
        if (fence == null)
            return;

        fence.Open();
    }

    public GameObject Get(int index)
    {
        GameObject select=null;

        //for each item in pool, check if item is active, if found an not active item, set it true
        foreach(GameObject item in pools[index])
        {
            if (!item.activeSelf)
            {
                select = item;
                select.SetActive(true);
                break;
            }
        }

        //if there are no unactive items
        if (!select)
        {
            select = Instantiate(monster_prefabs[index],transform);
            pools[index].Add(select);
        }

        return select;
    }

    private void Update()
    {
        if (PlayerStat.instance.isDead || !isActivated)
            return;

        timer += Time.deltaTime;

        if(timer >= nextSpawnTime)
        {
            timer = 0;
            nextSpawnTime = Random.Range(min_spawnRate, max_spawnRate);
            if(spawn_count < maxSpawn)
            {
                Spawn();
            }
        }
    }

    private void Spawn()
    {
        Transform temp = spawnPoint[Random.Range(1, spawnPoint.Length)];
        //if not touching with smile's layermask, spawn
        if (CheckCollision(temp))
        {
            GameObject monster = Get(spawn_Index);
            monster.transform.position = spawnPoint[Random.Range(1, spawnPoint.Length)].position;
            monster.GetComponent<EnemyStat>().Init(spawnData[spawn_Index]);
            monster.GetComponent<EnemyStat>().onMonsterDeath.AddListener(IncrementDeath);
            spawn_count++;
        }
    }

    private bool CheckCollision(Transform spawnPoint)
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(spawnPoint.position, 2f, Vector2.zero,0,slime_layerMask|player_layerMask) ;
        return (hits.Length == 0);
    }
}

[System.Serializable]
public class SpawnData
{
    public int hp;
    public int dropItemID;
    public int dropItemCount;
    public float dropRate;
    public int spriteType;
    public GameObject itemDrop_prefab;
}
