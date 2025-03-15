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

    public event EventHandler<OnMovingObjectLookDirectionEventArgs> OnMovingObjectLookDirectionEvent;
    public event EventHandler<OnMovingObjectMoveEventArgs> OnMovingObjectWalkEvent;
    public event EventHandler OnMovingObjectDieEvent;
    public class OnMovingObjectMoveEventArgs : EventArgs {
        public bool isRunning;
    }
    public class OnMovingObjectLookDirectionEventArgs : EventArgs {
        public int xDir;
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

    public void KillPlayer() {
        OnMovingObjectDieEvent?.Invoke(this, EventArgs.Empty);
    }

    public bool TryMove(Vector3Int targetCell, bool doRun) {
        if (!canMove || IsCellBlocked(targetCell))
            return false;

        StartCoroutine(SmoothMoveToCell(targetCell, doRun));
        return true;
    }

    private bool IsCellBlocked(Vector3Int cell) {
        Vector3 targetPos = grid.GetCellCenterWorld(cell);

        // Cast a ray from current position to target cell to check for obstacles
        RaycastHit2D hit = Physics2D.Raycast(transform.position, targetPos - transform.position, 1f, obstacleLayers);

        int xdir = Mathf.Clamp(cell.x - currentCell.x, -1, 1);
        OnMovingObjectLookDirectionEvent?.Invoke(this, new OnMovingObjectLookDirectionEventArgs { xDir = xdir });

        return hit.collider != null; // Returns true if an obstacle is detected
    }

    private IEnumerator SmoothMoveToCell(Vector3Int targetCell, bool doRun) {
        InvokeWalkAnimationEvent(targetCell, doRun);
     
        canMove = false;
        currentCell = targetCell;

        Vector3 targetPos = grid.GetCellCenterWorld(targetCell);

        float elapsedTime = 0f;
        int speed = doRun == true ? RUN_SPEED : WALK_SPEED;
        Vector3 startPos = transform.position;

        while (elapsedTime < 1f) {
            elapsedTime += speed * Time.deltaTime;
            transform.position = Vector3.Lerp(startPos, targetPos, elapsedTime);
            yield return null;
        }

        transform.position = targetPos; // Ensure precise snapping
        canMove = true;
    }


    private void InvokeWalkAnimationEvent(Vector3Int targetCell, bool isRunning) {
        int xdir = Mathf.Clamp(targetCell.x - currentCell.x, -1, 1);

        OnMovingObjectWalkEvent?.Invoke(this, new OnMovingObjectMoveEventArgs { isRunning = isRunning});
        OnMovingObjectLookDirectionEvent?.Invoke(this, new OnMovingObjectLookDirectionEventArgs { xDir = xdir});
    }

    public void InvokeStopAnimationEvent() {
        OnMovingObjectWalkEvent?.Invoke(this, null);
    }

    public void SetFaceDirection(Vector3Int direction) {
        faceDirection = direction;
    }
}

