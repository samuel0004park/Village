using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionAndResult : MonoBehaviour
{
    private string beep_sound = "beep_sound";
    private NumberSystem theNumber;
    public int correctPassword;
    public bool choosing;
    private bool result;
    private bool redo;
    public string type;

    private void Start()
    {
        theNumber = FindObjectOfType<NumberSystem>();
    }

    public void ShowQuestion()
    {
        choosing = true;
        StartCoroutine(QuestionCoroutine());
    }

    IEnumerator QuestionCoroutine()
    {
        //show choice
        theNumber.ShowNumber(correctPassword);

        //wait until finish choosing
        yield return new WaitUntil(() => !theNumber.activated);
        result = theNumber.GetResult();

        CheckChoice();

        choosing = false;
        PlayerManager.instance.interact = false; //set player's state to normal 

        //if choice was negative, disable flag so can choose again 
        if (redo)
            yield return new WaitForSeconds(0.5f);
    }
    private void CheckChoice()
    {
        //if correct choice
        if (!result)
        {
            NegativeResult();
            return;
        }
        if (result)
        {
            PositiveResult();
            return;
        }
    }
    private void NegativeResult()
    {
        AudioManager.instance.Play(beep_sound);
        redo = true;
    }

    private void PositiveResult()
    {
        //get objectdialogue id from game object

        switch (type) // give out result based on id
        {
            case "Fence": //open fence if have enough required item
                FenceChoice();
                break;
            default:
                break;
        }
        CheckShrine();
    }

    private void FenceChoice()
    {
        //continue only if fence exists
        OpenFence theFence = GetComponent<OpenFence>();
        if (theFence == null)
            return;

        theFence.Open();

    }
    private void CheckShrine()
    {
        ObjectDialogue temp = gameObject.GetComponent<ObjectDialogue>();
        if (temp.id < 100)
        {
            GameObject player = GameObject.Find("Player");
            SaveNLoad save = player.GetComponent<SaveNLoad>();
            save.Save();
        }
    }
}
