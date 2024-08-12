using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    #region Singleton
    public static UIController instance;
    private void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(this.gameObject);
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

    }
    #endregion 

    //variables to control UI
    [Header("Variables")]
    public bool isLive;
    public bool pannelUP; //true means there is a pannel active

    [Header("Sounds")]
    public string pause_in;
    public string pause_out;

    [Header("References")]
    public GameObject Option_Panel;
    public FadeManager theFade;
    public GameObject Die_Panel;

    public GameObject[] gos;

    void Start()
    {

        theFade = FindObjectOfType<FadeManager>();
        isLive = true;
    }
    private void Update()
    {
        if (!GameManager.instance.isLive || PlayerStat.instance.isDead || Inventory.instance.stopKeyInput)
            return;

        //call function if esc is pressed 
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            EscPress();
            StartCoroutine(WaitTime());
        }
    }
    IEnumerator WaitTime()
    {
        yield return new WaitForSeconds(0.01f);
    }
    private void EscPress()
    {
        //when pressed esc... close esc pannel if not live / open esc pannel if live
        if (isLive)
        {
            //if one of the other pannels are up, then close them and do nothing to ecs pannel
            if (pannelUP)
            {
                pannelUP = false;
                //sound
            }
            else
            {
                //if other pannels are all down open
                OpenOptionPanel();
            }
        }
        else if (!isLive)
        {
            // close when option pannel is already on
            CloseOptionPanel();
        }

    }
    private void OpenOptionPanel()
    {
        //stop player and scene
        isLive = false;
        OrderManager.instance.ForceStop(2);
        Time.timeScale = 0; //stop scene time
        Option_Panel.SetActive(true);
        AudioManager.instance.Play(pause_in);
    }
    public void CloseOptionPanel()
    {
        //close option panel, is called when press continue or esc 
        isLive = true;
        OrderManager.instance.ContinueMove();
        Option_Panel.SetActive(false);
        Time.timeScale = 1f; //set scene time to normal                         
        AudioManager.instance.Play(pause_out);
    } 
    public void ToTitle()
    {
        for (int i = 0; i < gos.Length; i++)
        {
            Destroy(gos[i]);
        }
        isLive = true;
        Option_Panel.SetActive(false);
        Time.timeScale = 1f; //set scene time to normal     
        SceneManager.LoadScene("Title");
    }
    public void Exit()
    {
        //quit game application
        Application.Quit();
    } 
}
