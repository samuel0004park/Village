using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPoint : MonoBehaviour
{
    public Location.MapNames startPoint; //the name of the current scene

    private CameraManager theCamera;

    void Start()
    {
        theCamera = FindObjectOfType<CameraManager>();

        //if the address matches, then move the position.
        if (startPoint == PlayerManager.instance.currentMapName)
        { 
            if (GameManager.instance.loading)
            {
                //if moving between scenes, change camera and player position to spart point
                theCamera.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, theCamera.transform.position.z);
                PlayerManager.instance.transform.position = this.transform.position;
            }

            //set bound
            theCamera.FindBound();
            StartCoroutine(StartMove());            
        }
    }
    IEnumerator StartMove()
    {
        //Get character info
        OrderManager.instance.PreLoadCharacter();

        //wait and enable move
        yield return new WaitForSeconds(1f);

        GameManager.instance.loading = false;
        OrderManager.instance.ContinueMove();
    }

}
