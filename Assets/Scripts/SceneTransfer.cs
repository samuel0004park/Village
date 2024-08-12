using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTransfer : MonoBehaviour
{
    [Header("References")]
    public string lookDirection;
    private PlayerManager thePlayer;
    private FadeManager theFade;
    public Transform target;
    public BoxCollider2D targetBound;
    private OrderManager theOrder;
    private CameraManager theCamera;

    public Location.MapNames transferMapName; //destination name the player will be transfered to 

    private void Start()
    {
        thePlayer = FindObjectOfType<PlayerManager>();
        theCamera = FindObjectOfType<CameraManager>();
        theOrder = FindObjectOfType<OrderManager>();
        theFade = FindObjectOfType<FadeManager>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            //change the name of the currentMapName to the destination name
            thePlayer.currentMapName = transferMapName;

            StartCoroutine(TransferCoroutine());
        }
    }
    IEnumerator TransferCoroutine()
    {
        theOrder.PreLoadCharacter();
        //stop player movement and fade out scene
        theOrder.ForceStop(1);
        theFade.FadeOut();

        yield return new WaitForSeconds(0.7f);

        //set new camera position and bound for tarnfering in same scene
        theCamera.transform.position = new Vector3(target.transform.position.x, target.transform.position.y, theCamera.transform.position.z);
        theCamera.SetBound(targetBound);

        //move player to new point in scene
        if(lookDirection!="")
            theOrder.Turn("player", lookDirection);
        thePlayer.transform.position = target.transform.position;

        yield return new WaitForSeconds(0.5f);//wait so fade in does not occur instantly
        //fadein and let player move
        theFade.FadeIn();
        yield return new WaitForSeconds(0.5f);
        theOrder.ContinueMove();
    }


}
