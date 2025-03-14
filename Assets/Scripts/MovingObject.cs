using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MovingObject : MonoBehaviour
{
    [SerializeField] protected BoxCollider2D boxCollider;
    [SerializeField] private LayerMask obstacleLayers;
    public Vector3Int faceDirection { get; private set; }

    public Grid grid { get; private set; }
    public Vector3Int currentCell { get; private set; }

    private bool canMove = false;
    private const int WALK_SPEED = 5;
    private const int RUN_SPEED = 8;

    public event EventHandler<OnMovingObjectMoveEventArgs> OnMovingObjectMoveStatusChangedEvent;

    public class OnMovingObjectMoveEventArgs : EventArgs {
        public float x;
        public float y;
        public bool isMoving;
    }

    public void TryFindGrid() {
        grid = FindObjectOfType<Grid>();
    }

    public void SetGrid(Grid targetGrid) {
        grid = targetGrid;
    }

    public void SnapObjectToGrid(Vector3Int targetCell) {
        transform.position = grid.GetCellCenterWorld(targetCell);
        currentCell = targetCell;
    }

    public void ContinueMove() {
        canMove = true;
    }

    public void StopMove() {
        StopAllCoroutines();
        canMove = false;
    }


    public bool TryMove(Vector3Int targetCell, bool doRun) {
        if (!canMove || IsCellBlocked(targetCell))
            return false;

        StartCoroutine(SmoothMoveToCell(targetCell, doRun==true? RUN_SPEED:WALK_SPEED));
        return true;
    }

    private bool IsCellBlocked(Vector3Int cell) {
        Vector3 targetPos = grid.GetCellCenterWorld(cell);

        // Cast a ray from current position to target cell to check for obstacles
        RaycastHit2D hit = Physics2D.Raycast(transform.position, targetPos - transform.position, 1f, obstacleLayers);

        InvokeWalkAnimationEvent(cell, false);
        
        return hit.collider != null; // Returns true if an obstacle is detected
    }

    private IEnumerator SmoothMoveToCell(Vector3Int targetCell, int speed) {
        InvokeWalkAnimationEvent(targetCell, true);
     
        canMove = false;
        currentCell = targetCell;

        Vector3 targetPos = grid.GetCellCenterWorld(targetCell);

        float elapsedTime = 0f;
        Vector3 startPos = transform.position;

        while (elapsedTime < 1f) {
            elapsedTime += speed * Time.deltaTime;
            transform.position = Vector3.Lerp(startPos, targetPos, elapsedTime);
            yield return null;
        }

        InvokeStopAnimationEvent();

        transform.position = targetPos; // Ensure precise snapping
        canMove = true;
    }

    private void InvokeWalkAnimationEvent(Vector3Int targetCell, bool isWalking) {
        int xdir = Mathf.Clamp(targetCell.x - currentCell.x, -1, 1);
        int ydir = Mathf.Clamp(targetCell.y - currentCell.y, -1, 1);

        OnMovingObjectMoveStatusChangedEvent?.Invoke(this, new OnMovingObjectMoveEventArgs { x = xdir, y = ydir, isMoving = isWalking });
    }

    private void InvokeStopAnimationEvent() {
        OnMovingObjectMoveStatusChangedEvent?.Invoke(this, null);
    }


    public void SetFaceDirection(Vector3Int direction) {
        faceDirection = direction;
    }
}

