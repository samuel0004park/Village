using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    private MovingObject movingObject;
    private PlayerStat playerStat;
    private Vector3Int targetCell; 
    public event EventHandler OnPlayerMoveEvent;

    public const int RUN_STAMINA = 5;

    private void Awake() {
        playerStat = GetComponent<PlayerStat>();
        movingObject = GetComponent<MovingObject>();
    }

    private void Start() {
        SubscribeEvents();
    }

    private void OnDestroy() {
        UnSubscribeEvents();
    }

   
    private void Update() {
        if(!playerStat.isDead && GameManager.Instance.isLive)
            HandleInput();
    }

    public Vector3Int GetGridPosition() {
        return movingObject.currentCell;
    }

    

    public void PlaceAtStartPoint(Vector3Int startPoint) {
        movingObject.TryFindGrid();
        targetCell = movingObject.grid.WorldToCell(startPoint);
        movingObject.SnapObjectToGrid(targetCell);
    }

    public void PlaceAtStartPoint(Grid grid, Vector3Int startPoint) {
        // Snap player to nearest grid cell at the start
        movingObject.SetGrid(grid);
        targetCell = movingObject.grid.WorldToCell(startPoint);
        movingObject.SnapObjectToGrid(targetCell);
    }

    private void HandleInput() {
        Vector3Int moveDirection = Vector3Int.zero;
        bool doRun = false;
        if (Input.GetKey(KeyCode.UpArrow)) moveDirection = Vector3Int.up;
        if (Input.GetKey(KeyCode.DownArrow)) moveDirection = Vector3Int.down;
        if (Input.GetKey(KeyCode.LeftArrow)) moveDirection = Vector3Int.left;
        if (Input.GetKey(KeyCode.RightArrow)) moveDirection = Vector3Int.right;

        if (Input.GetKey(KeyCode.LeftShift)) doRun = true;

        if (moveDirection != Vector3Int.zero) {

            movingObject.SetFaceDirection(moveDirection);
            Vector3Int newCell = targetCell + moveDirection;

            if (movingObject.TryMove(newCell, doRun)) {
                playerStat.DecreaseStamina(doRun==true? RUN_STAMINA:0);
                targetCell = newCell;
                OnPlayerMoveEvent?.Invoke(this, EventArgs.Empty);
            }
        }
    }


    private void SubscribeEvents() {
        Inventory.Instance.OnInventoryStateChangedEvent += Instance_OnInventoryStateChangedEvent;
        UIManager.Instance.OnOptionPanelHideEvent += Instance_OnOptionPanelHideEvent;
        UIManager.Instance.OnOptionPanelShowEvent += Instance_OnOptionPanelShowEvent;
        StartPoint.OnStartMoveEvent += StartPoint_OnStartMoveEvent;
        ObjectDialogue.OnObjectDialogueStateChangedEvent += ObjectDialogue_OnObjectDialogueStateChangedEvent;
        SceneTransfer.OnSceneTransferEvent += SceneTransfer_OnSceneTransferEvent;
    }

   

    private void UnSubscribeEvents() {
        Inventory.Instance.OnInventoryStateChangedEvent -= Instance_OnInventoryStateChangedEvent;
        UIManager.Instance.OnOptionPanelHideEvent -= Instance_OnOptionPanelHideEvent;
        UIManager.Instance.OnOptionPanelShowEvent -= Instance_OnOptionPanelShowEvent;
        StartPoint.OnStartMoveEvent -= StartPoint_OnStartMoveEvent;
        ObjectDialogue.OnObjectDialogueStateChangedEvent -= ObjectDialogue_OnObjectDialogueStateChangedEvent;
        SceneTransfer.OnSceneTransferEvent -= SceneTransfer_OnSceneTransferEvent;
    }

 
    private void Instance_OnInventoryStateChangedEvent(object sender, Inventory.OnInventoryStateChangedEventArgs e) {
        if(e.isOpen)
            movingObject.StopMove();
        else
            movingObject.ContinueMove();
    }

    private void SceneTransfer_OnSceneTransferEvent(object sender, SceneTransfer.OnSceneTransferEventArgs e) {
        if (e.isTransfering)
            movingObject.StopMove();
        else
            movingObject.ContinueMove();
    }

    private void ObjectDialogue_OnObjectDialogueStateChangedEvent(object sender, ObjectDialogue.OnObjectDialogueStateChangedEventArgs e) {
        if (e.isTalking)
            movingObject.StopMove();
        else
            movingObject.ContinueMove();
    }

    private void StartPoint_OnStartMoveEvent(object sender, EventArgs e) {
        movingObject.ContinueMove();
    }
    
    private void Instance_OnOptionPanelShowEvent(object sender, System.EventArgs e) {
        movingObject.StopMove();
    }

    private void Instance_OnOptionPanelHideEvent(object sender, System.EventArgs e) {
        movingObject.ContinueMove();
    }

    private void Instance_OnInventoryOpenEvent(object sender, System.EventArgs e) {
        movingObject.StopMove();
    }

    private void Instance_OnInventoryCloseEvent(object sender, System.EventArgs e) {
        movingObject.ContinueMove();
    }

}
