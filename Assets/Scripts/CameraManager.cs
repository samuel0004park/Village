using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    #region Singleton
    static public CameraManager instance;
    private void Awake()
    {
        //keep singleton property
        if (instance == null)
        {
            DontDestroyOnLoad(this.gameObject);
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
       
    }
    #endregion

    [Header("Variables")]
    public GameObject target; //gameobject that camera will follow
    [SerializeField] private float moveSpeed; 
    private Vector3 targetPosition; //the position of the taget the camera will follow
    private Vector3 minBound;
    private Vector3 maxBound;
    private float halfHeight;
    private float halfWidth;

    [Space]
    [Header("References")]
    [SerializeField] private BoxCollider2D bound;
    private Camera Camera;
    public SceneBound[] bounds;

    
    void Update()
    {
        if (target.gameObject != null)
        {
            targetPosition.Set(target.transform.position.x, target.transform.position.y, this.transform.position.z); //set the positon of target (z is camera's axis)
            this.transform.position = Vector3.Lerp(this.transform.position, targetPosition, moveSpeed * Time.deltaTime);//move movespeed per second

            //Clamp returns restricted output in between specified values
            float clampedX = Mathf.Clamp(this.transform.position.x, minBound.x + halfWidth, maxBound.x - halfWidth);
            float clampedY = Mathf.Clamp(this.transform.position.y, minBound.y + halfHeight, maxBound.y - halfHeight);
            //set position camera defiend by clamping the postion
            this.transform.position = new Vector3(clampedX, clampedY, this.transform.position.z);
        }
    }
    public void FindBound()
    {
        bounds = FindObjectsOfType<SceneBound>();

        for (int i = 0; i < bounds.Length; i++)
        {
            if (PlayerManager.instance.currentMapName == bounds[i].boundName)
            {
                SetBound(bounds[i].GetComponent<BoxCollider2D>());
                break;
            }
        }
    }
    public void SetBound(BoxCollider2D newBound)
    {
        Camera = GetComponent<Camera>();

        // set new bound for camera
        bound = newBound;
        minBound = bound.bounds.min;
        maxBound = bound.bounds.max;

        //calculate half h and w of camera
        halfHeight = Camera.orthographicSize;
        halfWidth = halfHeight * Screen.width / Screen.height; //formula to get camera's width
    }
}
