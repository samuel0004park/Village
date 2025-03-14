using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTransfer : MonoBehaviour
{
    [SerializeField] private Vector3Int targetVector;
    [SerializeField] private Grid targetGrid;
    [SerializeField] private BoxCollider2D targetBound;
    [SerializeField] private Location.MapNames transferMapName; //destination name the player will be transfered to 
    
    private CameraManager theCamera;

    public static event EventHandler<OnSceneTransferEventArgs> OnSceneTransferEvent;
    public class OnSceneTransferEventArgs : EventArgs {
        public bool isTransfering;
    }

    private void Start()
    {
        theCamera = FindObjectOfType<CameraManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //change the name of the currentMapName to the destination name
            PlayerManager.Instance.SetLocationInfo(PlayerManager.Instance.currentSceneName, transferMapName);

            StartCoroutine(TransferCoroutine());
        }
    }

    IEnumerator TransferCoroutine()
    {

        //stop player movement and fade out scene
        OnSceneTransferEvent?.Invoke(this, new OnSceneTransferEventArgs { isTransfering = true });
        FadeManager.Instance.FadeOut();

        yield return new WaitForSeconds(0.7f);

        //set new camera position and bound for tarnfering in same scene
        theCamera.transform.position = new Vector3(targetVector.x, targetVector.y, theCamera.transform.position.z);
        theCamera.SetBound(targetBound);

        PlayerManager.Instance.PlayerMovement.PlaceAtStartPoint(targetGrid,targetVector);

        yield return new WaitForSeconds(0.5f);
        FadeManager.Instance.FadeIn();

        yield return new WaitForSeconds(0.5f);
        OnSceneTransferEvent?.Invoke(this, new OnSceneTransferEventArgs { isTransfering = false });
    }


}
