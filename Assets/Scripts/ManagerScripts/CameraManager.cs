using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance;
    private const float CAM_MOVE_SPEED = 100;
    
    [SerializeField] private GameObject target;
    [SerializeField] private Camera Camera;
    [SerializeField] private Vector3 targetPosition;
    private Vector3 minBound;
    private Vector3 maxBound;
    private float halfHeight;
    private float halfWidth;

    [SerializeField] private BoxCollider2D bound;
    [SerializeField] private SceneBound[] bounds;


    private void Awake() {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Update() {
        if (target == null || Camera == null) {
            return;
        }

        UpdateCameraTransform();
    }

    private Vector3 velocity = Vector3.zero;

    private void UpdateCameraTransform() {
        targetPosition.Set(target.transform.position.x, target.transform.position.y, Camera.transform.position.z);

        // Smooth movement
        Camera.transform.position = Vector3.SmoothDamp(Camera.transform.position, targetPosition, ref velocity, 0.2f);

        // Clamping to stay within bounds
        float clampedX = Mathf.Clamp(Camera.transform.position.x, minBound.x + halfWidth, maxBound.x - halfWidth);
        float clampedY = Mathf.Clamp(Camera.transform.position.y, minBound.y + halfHeight, maxBound.y - halfHeight);

        Camera.transform.position = new Vector3(clampedX, clampedY, Camera.transform.position.z);
    }


    public void SetTarget(GameObject selectTarget) {
        target = selectTarget;
    }

    public void FindBound()
    {
        bounds = FindObjectsOfType<SceneBound>();

        for (int i = 0; i < bounds.Length; i++)
        {
            if (PlayerManager.Instance.CurrentMapName == bounds[i].boundName)
            {
                SetBound(bounds[i].GetComponent<BoxCollider2D>());
                break;
            }
        }
    }

    public void SetBound(BoxCollider2D newBound) {
        Camera = GameObject.FindGameObjectWithTag("MainCamera")?.GetComponent<Camera>();

        if (Camera == null) {
            Debug.LogError("MainCamera not found! Make sure your camera has the correct tag.");
            return;
        }

        bound = newBound;
        minBound = bound.bounds.min;
        maxBound = bound.bounds.max;

        halfHeight = Camera.orthographicSize;
        halfWidth = halfHeight * Screen.width / Screen.height;
    }
}
