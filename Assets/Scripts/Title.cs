using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{

    [Header("References")]
    private FadeManager theFade;
    private GameManager theGM;
    public GameObject button_Group;

    public string click_sound;
    void Start()
    {
        theFade = FindObjectOfType<FadeManager>();
        theGM = FindObjectOfType<GameManager>();
  
        OrderManager.instance.ForceStop(2);
    }

    public void StartGame()
    {
        StartCoroutine(GameStartCoroutine());
    }
    IEnumerator GameStartCoroutine()
    {
        button_Group.SetActive(false);
        theFade.FadeOut();
        AudioManager.instance.Play(click_sound);
        yield return new WaitForSeconds(2f);

        //make player visible
        Color color = PlayerManager.instance.GetComponent<SpriteRenderer>().color;
        color.a = 1f;
        PlayerManager.instance.GetComponent<SpriteRenderer>().color = color;

        //change player location info to match 
        PlayerManager.instance.currentSceneName = Location.SceneNames.Init;
        PlayerManager.instance.currentMapName = Location.MapNames.StartingIsland;

        // initialize starting player hp and stamina
        PlayerStat.instance.hp = 3;
        PlayerStat.instance.stamina = 500;
        PlayerStat.instance.Refresh();
        QuestManager.instance.questId = 10;
        QuestManager.instance.questActionIndex = 0;

        //set border
        GameManager.instance.loading = true;
        theGM.LoadStart();

        SceneManager.LoadScene(PlayerManager.instance.currentSceneName.ToString());
    }

    public void LoadGame()
    {
        StartCoroutine(LoadGameoroutine());
    }
    IEnumerator LoadGameoroutine()
    {
        button_Group.SetActive(false);
        theFade.FadeOut();
        AudioManager.instance.Play(click_sound);
        yield return new WaitForSeconds(2f);

        // make player visible
        Color color = PlayerManager.instance.GetComponent<SpriteRenderer>().color;
        color.a = 1f;
        PlayerManager.instance.GetComponent<SpriteRenderer>().color = color;

        // load saved info and refresh
        SaveNLoad saveNLoad = FindObjectOfType<SaveNLoad>();
        saveNLoad.Load();
        PlayerStat.instance.HeartBeat();
        PlayerStat.instance.UpdateBar();
        //DialogueManager.instance.GenerateData();

        //set border
        theGM.LoadStart();

        SceneManager.LoadScene(PlayerManager.instance.currentSceneName.ToString());
    }

    public void ExitGame()
    {
        AudioManager.instance.Play(click_sound);
        Application.Quit();
    }
}
