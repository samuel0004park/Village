using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NPCMove
{
    [Tooltip("Checking true will allow NPC movement")] 
    //public bool NPCmove;
    public string[] direction; //direction NPC will move 
    [Range(1,5)] //enables a scrollbar in inspector window
    public int frequency; //the frequency NPC will move to a certain direction
}
public class NPCManager : MovingObject
{
    [SerializeField]
    public NPCMove npc;
    bool checkCollisionFLag;

    void Start()
    {
        queue = new Queue<string>();
        boxCollider = GetComponent<BoxCollider2D>();
        StartCoroutine(MoveCoroutine());
    }

    public void SetNotMove()
    {
        StopAllCoroutines();
    }
    IEnumerator MoveCoroutine()
    {
        //if there is a direction input for npc to move
        if (npc.direction.Length != 0)
        {
            for (int i = 0; i < npc.direction.Length; i++)
            {
                //wait until there are either one or zero items in queue
                yield return new WaitUntil(()=> queue.Count < 2);
                //move
                base.Move(npc.direction[i], npc.frequency); 
               
                //when at the last iteration, reset back to -1 
                if (i == npc.direction.Length - 1)
                    i = -1;
            }
        }

        
        
    }
  
}
