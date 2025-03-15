using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityAnimation : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private MovingObject MovingObject;
    [SerializeField] private SpriteRenderer SpriteRenderer;

    private enum State { IDLE, WALKING, RUNNING}
    private State state;

    private void Start() {
        SubscribeEvents();
    }
    private void OnDestroy() {
        UnSubscribeEvents();
    }



    private void MovingObject_OnMovingObjectDieEvent(object sender, System.EventArgs e) {
        animator.SetTrigger("isDead");
    }

    private void MovingObject_OnMovingObjectWalkEvent(object sender, MovingObject.OnMovingObjectMoveEventArgs e) {
        if (e == null) {
            SetIdle();
            return;
        }

        if (e.isRunning && state == State.RUNNING)
            return;

        if (!e.isRunning && state == State.WALKING)
            return;

        state = e.isRunning==true? State.RUNNING:State.WALKING;
       
        SetAnimation(true);
    }

    private void MovingObject_OnMovingObjectLookDirectionEvent(object sender, MovingObject.OnMovingObjectLookDirectionEventArgs e) {
        FlipSprite(e.xDir);
    }

    private void FlipSprite(int xDir) {
        switch (xDir) {
            case -1:
                SpriteRenderer.flipX = true;
                break;
            case 1:
                SpriteRenderer.flipX = false;
                break;
            default:
                break;
        }
    }

    private void SetIdle() {
        animator.SetBool("isRunning", false);
        animator.SetBool("isWalking", false);
        state = State.IDLE;
    }

    private void SetAnimation(bool target) {
        animator.SetBool("isRunning", false);
        animator.SetBool("isWalking", false);
        switch (state) {
            case State.WALKING:
                animator.SetBool("isWalking", target);
                break;
            case State.RUNNING:
                animator.SetBool("isRunning", target);
                break;
        }
    }

    private void UnSubscribeEvents() {
        MovingObject.OnMovingObjectWalkEvent -= MovingObject_OnMovingObjectWalkEvent;
        MovingObject.OnMovingObjectDieEvent -= MovingObject_OnMovingObjectDieEvent;
        MovingObject.OnMovingObjectLookDirectionEvent -= MovingObject_OnMovingObjectLookDirectionEvent;

    }

    private void SubscribeEvents() {
        MovingObject.OnMovingObjectWalkEvent += MovingObject_OnMovingObjectWalkEvent;
        MovingObject.OnMovingObjectDieEvent += MovingObject_OnMovingObjectDieEvent;
        MovingObject.OnMovingObjectLookDirectionEvent += MovingObject_OnMovingObjectLookDirectionEvent;
    }

}
