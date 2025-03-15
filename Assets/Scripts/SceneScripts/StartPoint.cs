using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPoint : MonoBehaviour
{
    public Location.MapNames startPoint; //the name of the current scene

    [SerializeField] private Vector3Int vector;
    [SerializeField] private Grid grid;
    private CameraManager theCamera;
    
    public static event EventHandler OnStartMoveEvent;
    
    void Start()
    {
        theCamera = FindObjectOfType<CameraManager>();

        //if the address matches, then move the position.
        if (startPoint == PlayerManager.Instance.CurrentMapName)
        { 

            if (GameManager.Instance.savedPoint == true) {
                theCamera.transform.position = new Vector3(GameManager.Instance.playerSpawnPoint.x, GameManager.Instance.playerSpawnPoint.y, GameManager.Instance.playerSpawnPoint.z);
                PlayerManager.Instance.PlayerMovement.PlaceAtStartPoint(GameManager.Instance.playerSpawnPoint);
                GameManager.Instance.ResetSpawnPoint();
            }
            else {
                theCamera.transform.position = new Vector3(vector.x, vector.y, vector.z);
                PlayerManager.Instance.PlayerMovement.PlaceAtStartPoint(grid, vector); 
            }

            //set bound
            theCamera.FindBound();
            StartCoroutine(StartMove());            
        }
    }
    IEnumerator StartMove()
    {
        //wait and enable move
        yield return new WaitForSeconds(1f);

        OnStartMoveEvent?.Invoke(this, EventArgs.Empty);
    }

}
