using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatusUIManager : MonoBehaviour
{
    public static PlayerStatusUIManager Instance;

    [SerializeField] private PlayerStat PlayerStat;
    [SerializeField] public Image[] playerLives;
    [SerializeField] private Slider stamina_slider;
    [SerializeField] private CanvasGroup CanvasGroup;
    [SerializeField] private GameObject Die_Panel;
 
    private void Awake() {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        
        HideVisuals();
        SubscribeEvents();
        InitializePlayerStaminaBar(PlayerStat.MAX_STAMINA);
    }
    
    private void OnDestroy() {
        UnSubscribeEvents();
    }

    #region Event Handlers

    private void Instance_OnInventoryStateChangedEvent(object sender, Inventory.OnInventoryStateChangedEventArgs e) {
        if (e.isOpen)
            HideVisuals();
        else
            ShowVisuals();
    }

    private void ObjectDialogue_OnObjectDialogueStateChangedEvent(object sender, ObjectDialogue.OnObjectDialogueStateChangedEventArgs e) {
        if (e.isTalking)
            HideVisuals();
        else
            ShowVisuals();
    }

    private void Instance_OnStartGameEvent(object sender, System.EventArgs e) {
        Invoke(nameof(ShowVisuals),1f);
    }

    private void PlayerStat_OnPlayerHeartBeatEvent(object sender, PlayerStat.OnPlayerHeartBeatEventArgs e) {
        UpdatePlayerHealth(e.currentStamina, e.currentLifeCount);
    }

    #endregion

    private void InitializePlayerStaminaBar(int stamina) {
        stamina_slider.maxValue = stamina;

    }

    public void UpdatePlayerHealth(int stamina, int currentHp) {
        if (currentHp == 0) {
            Die_Panel.gameObject.SetActive(true);
            return;
        }
        stamina_slider.value = stamina;
        for (int i = 0; i < currentHp; i++) {
            playerLives[i].color = new Color(255, 255, 255, 255);
        }
        for (int i = currentHp; i < playerLives.Length; i++) {
            playerLives[i].color = new Color(1, 0, 0, 0.4f);
        }
    }

    #region Helper Methods

    private void ShowVisuals() {
        CanvasGroup.alpha = 1f;
    }

    private void HideVisuals() {
        CanvasGroup.alpha = 0f;
    }

    private void SubscribeEvents() {
        PlayerStat.OnPlayerHeartBeatEvent += PlayerStat_OnPlayerHeartBeatEvent;
        GameManager.Instance.OnStartGameEvent += Instance_OnStartGameEvent;
        Inventory.Instance.OnInventoryStateChangedEvent += Instance_OnInventoryStateChangedEvent;
        ObjectDialogue.OnObjectDialogueStateChangedEvent += ObjectDialogue_OnObjectDialogueStateChangedEvent;
    }

    

    private void UnSubscribeEvents() {
        Inventory.Instance.OnInventoryStateChangedEvent -= Instance_OnInventoryStateChangedEvent;
        PlayerStat.OnPlayerHeartBeatEvent -= PlayerStat_OnPlayerHeartBeatEvent;
        GameManager.Instance.OnStartGameEvent -= Instance_OnStartGameEvent;
        ObjectDialogue.OnObjectDialogueStateChangedEvent -= ObjectDialogue_OnObjectDialogueStateChangedEvent;
    }

    #endregion
}
