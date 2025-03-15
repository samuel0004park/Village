using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    [SerializeField] private PlayerManager playerManager;
    [SerializeField] private SaveNLoad saveNLoad;
    [SerializeField] private Animator animator;
    [SerializeField] private CanvasGroup ButtonCanvasGroup;

    public static event EventHandler OnTitleButtonPressedEvent;
    public static event EventHandler OnGameStartEvent;
    #region Button Method

    public void StartNewGame() {
        LoadNewGame();
        ButtonPress();
    }

    public void LoadGame() {
        LoadSavedGame();
        ButtonPress();
    }

    #endregion

    private void ButtonPress() {
        ButtonCanvasGroup.interactable = false;
        OnTitleButtonPressedEvent?.Invoke(this, EventArgs.Empty);
    }



    private void LoadNewGame() {
        LoadNewPlayer();

        QuestManager.Instance.ResetQuestProgress();

        animator.SetTrigger("doHide");
        OnGameStartEvent?.Invoke(this, EventArgs.Empty);
    }

    private void LoadSavedGame() {
        LoadExingPlayer();

        animator.SetTrigger("doHide");
        OnGameStartEvent?.Invoke(this, EventArgs.Empty);
    }


    private void LoadNewPlayer() {
        playerManager.SetLocationInfo(Location.SceneNames.Init, Location.MapNames.StartingIsland);
        playerManager.playerStat.LoadStat(PlayerStat.MAX_HP, PlayerStat.MAX_STAMINA);
    }

    private void LoadExingPlayer() {
        saveNLoad.Load();
    }

    public void ExitGame()
    {
        OnTitleButtonPressedEvent?.Invoke(this, EventArgs.Empty);
        Application.Quit();
    }
}
