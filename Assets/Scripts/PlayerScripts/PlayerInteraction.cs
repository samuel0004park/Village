using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private LayerMask scanLayers;

    private PlayerStat playerStat;
    private MovingObject MovingObject;
    public GameObject scanObject { get; private set; }

    private void Awake() {
        playerStat = GetComponent<PlayerStat>();
        MovingObject = GetComponent<MovingObject>();
    }

    private void Update() {
        if (!playerStat.isDead && GameManager.Instance.isLive)
            HandleInput();
    }

    private void FixedUpdate() {
        ScanObject();
    }

    private void ScanObject() {
        Vector2 direction = new Vector2(MovingObject.faceDirection.x, MovingObject.faceDirection.y).normalized; // Ensure it's a unit vector

        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, 1f, scanLayers);

        if (hit.collider != null) {
            scanObject = hit.collider.gameObject; // Store detected object
        }
        else {
            scanObject = null; // Reset when nothing is hit
        }
    }

    public void HandleInput() {
        
        if (Input.GetKeyDown(KeyCode.Z)) InteractionLogic();
    }

    private void InteractionLogic() {
        if (scanObject == null)
            return;

        if(scanObject.TryGetComponent(out IInteract interactableObject)) {
            interactableObject.Interact();
        }
    }

}
