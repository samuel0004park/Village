using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    [SerializeField] private Canvas Canvas;
    public CameraManager theCamera;
    public Camera cam;
    public DatabaseManager DataManager;
    public QuestManager QuestManager;
    public UIManager UIManager;

    public Vector3Int playerSpawnPoint { get; private set; }
    public bool savedPoint { get; private set; }
    public bool isLive { get; private set; }

    public event EventHandler OnStartGameEvent;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(Instance.gameObject);

    }

    private void Start() {
        SubscribeEvents();
    }

    private void OnDestroy() {
        UnSubscribeEvents();
    }

    #region EventHandlers

    private void Instance_OnInventoryStateChangedEvent(object sender, Inventory.OnInventoryStateChangedEventArgs e) {
        if (e.isOpen) {
            isLive = false;
        }
        else
            isLive = true;
    }

    private void PlayerStat_OnPlayerKilledEvent(object sender, EventArgs e) {
        isLive = false;
    }

    private void Title_OnGameStartEvent(object sender, EventArgs e) {
        StartCoroutine(LoadWaitTime());
    }

    private void TransferMap_OnTransformMapEvent(object sender, EventArgs e) {
        isLive = false;
    }

    private void StartPoint_OnStartMoveEvent(object sender, EventArgs e) {
        isLive = true;
    }
    #endregion

    public void SetSpawnPoint(Vector3Int vector3Int) {
        playerSpawnPoint = vector3Int;
        savedPoint = true;
    }

    public void ResetSpawnPoint() {
        playerSpawnPoint = default;
        savedPoint = false;
    }

    IEnumerator LoadWaitTime()
    {
        yield return new WaitForSeconds(1f);
        
        CameraManager.Instance.SetTarget(PlayerManager.Instance.gameObject);
        Canvas.worldCamera = cam;

        OnStartGameEvent?.Invoke(this, EventArgs.Empty);

        yield return new WaitForSeconds(0.5f);

        PlayerManager.Instance.ShowVisuals();
        SceneManager.LoadScene(PlayerManager.Instance.currentSceneName.ToString());
        isLive = true;
    }

    private void SubscribeEvents() {
        StartPoint.OnStartMoveEvent += StartPoint_OnStartMoveEvent;
        TransferMap.OnTransformMapEvent += TransferMap_OnTransformMapEvent;
        Title.OnGameStartEvent += Title_OnGameStartEvent;
        PlayerStat.OnPlayerKilledEvent += PlayerStat_OnPlayerKilledEvent;
        Inventory.Instance.OnInventoryStateChangedEvent += Instance_OnInventoryStateChangedEvent;

    }


    private void UnSubscribeEvents() {
        StartPoint.OnStartMoveEvent -= StartPoint_OnStartMoveEvent;
        TransferMap.OnTransformMapEvent -= TransferMap_OnTransformMapEvent;
        Title.OnGameStartEvent -= Title_OnGameStartEvent;
        PlayerStat.OnPlayerKilledEvent -= PlayerStat_OnPlayerKilledEvent;
        Inventory.Instance.OnInventoryStateChangedEvent -= Instance_OnInventoryStateChangedEvent;

    }

}
