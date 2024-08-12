using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Singleton
    public static GameManager instance;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(instance.gameObject);
        }
    }
    #endregion

    [Header("References")]
    public EmotionManager theEmo;
    public CameraManager theCamera;
    public FadeManager theFade;
    public DialogueManager theDM;
    public QuestManager theQuest;
    public BGMManager theBGM;
    public OrderManager theOrder;
    public DatabaseManager theData;
    public Camera cam;

    [Header("Variables")]
    public GameObject hpBar;
    public GameObject mpBar;
    public bool loading;
    public bool isLive=false;

    public enum Location { StartingIsland, Village, Forest }

    public void  LoadStart()
    {
        StartCoroutine(LoadWaitTime());
    }

    IEnumerator LoadWaitTime()
    {
        yield return new WaitForSeconds(0.5f);
        theData = FindObjectOfType<DatabaseManager>();
        theOrder= FindObjectOfType<OrderManager>();
        theEmo = FindObjectOfType<EmotionManager>();
        theCamera = FindObjectOfType<CameraManager>();
        theFade = FindObjectOfType<FadeManager>();
        theDM = FindObjectOfType<DialogueManager>();
        cam = FindObjectOfType<Camera>();
        theQuest = FindObjectOfType<QuestManager>();
        theBGM = FindObjectOfType<BGMManager>();
    

        theCamera.target = GameObject.Find("Player");
        theDM.GetComponent<Canvas>().worldCamera = cam;

        CameraManager.instance.FindBound();

        theBGM.Play(0,0.25f);
        theFade.FadeIn();

        hpBar.SetActive(true);
        mpBar.SetActive(true);
        mpBar.GetComponent<Animator>().SetBool("Appear", true);
        theQuest.CheckQuest();

        yield return new WaitForSeconds(0.5f);
        isLive = true;
    }
}
