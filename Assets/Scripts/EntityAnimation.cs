using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityAnimation : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private MovingObject MovingObject;

    private void Start() {
        SubscribeEvents();
    }
    private void OnDestroy() {
        UnSubscribeEvents();
    }

    private void SubscribeEvents() {
        MovingObject.OnMovingObjectMoveStatusChangedEvent += MovingObject_OnMovingObjectMoveStatusChangedEvent;

    }
    private void UnSubscribeEvents() {
        MovingObject.OnMovingObjectMoveStatusChangedEvent -= MovingObject_OnMovingObjectMoveStatusChangedEvent;

    }
    private void MovingObject_OnMovingObjectMoveStatusChangedEvent(object sender, MovingObject.OnMovingObjectMoveEventArgs e) {
        if(e == null) {
            animator.SetBool("isWalking", false);
        }
        else {
            animator.SetBool("isWalking", e.isMoving);
            animator.SetFloat("DirX",e.x);
            animator.SetFloat("DirY",e.y);
        }
    }
}
