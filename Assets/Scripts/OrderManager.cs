using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderManager : MonoBehaviour
{
    #region Singleton
    private void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(this.gameObject);
            instance = this;
        }
        else
            Destroy(this.gameObject);
    }
    #endregion Singleton
    static public OrderManager instance;

    private PlayerManager thePlayer;
    private List<MovingObject> characters;



    void Start()
    {
        thePlayer = FindObjectOfType<PlayerManager>();
    }

    public void PreLoadCharacter()
    {
        //save list of all MovingObject in game
        characters = ToList();
    }

    public List<MovingObject> ToList()
    {
        //find and save all MovingObject(npc) and add to list
        List<MovingObject> tempList = new List<MovingObject>();
        MovingObject[] temp = FindObjectsOfType<MovingObject>();

        for (int i = 0; i < temp.Length; i++)
        {
            tempList.Add(temp[i]);
        }
        //return list of all MovingObject
        return tempList;
    }

    public void Move(string _name, string _dir)
    {
        //when given an order, find the same name and corresponding order to move
        for (int i = 0; i < characters.Count; i++)
        {
            if(_name == characters[i].characterName)
            {
                characters[i].Move(_dir);
            }
        }
    }
    public void ForceStop(int choice)
    {
        switch (choice)
        {
            case 1:
                //force stop player movement and coroutine (used when moving scene)
                StopAllCoroutines();
                thePlayer.notMove = true;
                thePlayer.anim.SetBool("isWalking", false);
                thePlayer.currentWalkCount = 0;
                break;
            case 2:
                //process current input and stop further input
                thePlayer.notMove = true;
                break;
        }
    }
    public void ContinueMove()
    {
        //set canMove to true to receive later input
        thePlayer.inputDelay = true;
        thePlayer.notMove = false;
    }

    public void Turn(string _name , string _dir) 
    {
        //when given an order, find the same name and corresponding order to turn
        for (int i = 0; i < characters.Count; i++)
        {
            if (_name == characters[i].characterName)
            {
                characters[i].anim.SetFloat("DirX", 0f);
                characters[i].anim.SetFloat("DirY", 0f);

                switch (_dir)
                {
                    case "UP":
                        characters[i].anim.SetFloat("DirY",1f);
                        break;
                    case "DOWN":
                        characters[i].anim.SetFloat("DirY",-1f);
                        break;
                    case "RIGHT":
                        characters[i].anim.SetFloat("DirX",1f);
                        break;
                    case "LEFT":
                        characters[i].anim.SetFloat("DirX",-1f);
                        break;
                }
                
            }
        }
    }

    public void SetTransparent(string _name)
    {
        for (int i = 0; i < characters.Count; i++)
        {
            if (_name == characters[i].characterName)
            {
                characters[i].gameObject.SetActive(false);
            }
        }
    }
    public void SetUnTransparent(string _name)
    {
        for(int i = 0; i < characters.Count; i++)
        {
            if(_name == characters[i].characterName)
            {
                characters[i].gameObject.SetActive(true);
            }
        }
    }
}
