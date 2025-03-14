using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public bool isLive { get; private set; }
    public bool pannelUP { get; private set; }

    [SerializeField] private GameObject Option_Panel;

    public event EventHandler OnOptionPanelShowEvent;
    public event EventHandler OnOptionPanelHideEvent;
    public event EventHandler OnReturnToTitleEvent;
 

    private void Awake() {
        if (Instance == null) {
            DontDestroyOnLoad(this.gameObject);
            Instance = this;
        }
        else  Destroy(this.gameObject);

        
    }

    private void OnDestroy() {
        UnSubscribeEvents();
    }

    void Start()
    {
        isLive = true;
        SubscribeEvents();
    }

    
    private void SubscribeEvents() {
        Inventory.Instance.OnInventoryStateChangedEvent += Instance_OnInventoryStateChangedEvent; ;
        OnReturnToTitleEvent += UIManager_OnReturnToTitleEvent;
    }

    
    private void UnSubscribeEvents() {
        Inventory.Instance.OnInventoryStateChangedEvent -= Instance_OnInventoryStateChangedEvent; ;
        OnReturnToTitleEvent -= UIManager_OnReturnToTitleEvent;
    }

    private void UIManager_OnReturnToTitleEvent(object sender, EventArgs e) {
        Destroy(gameObject);
    }

    private void Instance_OnInventoryStateChangedEvent(object sender, Inventory.OnInventoryStateChangedEventArgs e) {
        if (e.isOpen) {
            isLive = false;
            pannelUP = true;
        }
        else {
            isLive = true;
            pannelUP = false;
        }
    }


    private void Update()
    {
        if (!GameManager.Instance.isLive || PlayerManager.Instance.CheckPlayerDead() || Inventory.Instance.stopKeyInput)
            return;

        //call function if esc is pressed 
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            EscPress();
        }
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
        isLive = false;
        Time.timeScale = 0; //stop scene time
        Option_Panel.SetActive(true);
        OnOptionPanelShowEvent?.Invoke(this, EventArgs.Empty);
    }

    public void CloseOptionPanel()
    {
        isLive = true;
        Time.timeScale = 1f; //set scene time to normal                         
        Option_Panel.SetActive(false);
        OnOptionPanelHideEvent?.Invoke(this, EventArgs.Empty);
    }

    public void ToTitle()
    {
        OnReturnToTitleEvent?.Invoke(this,EventArgs.Empty);
        isLive = true;
        Time.timeScale = 1f; //set scene time to normal     
        SceneManager.LoadScene("Title");
    }


    public void Exit()
    {
        //quit game application
        Application.Quit();
    } 
}
