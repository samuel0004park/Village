using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChoiceManager : MonoBehaviour
{
    #region Singleton
    private void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(this.gameObject); //prevent player gameobject from beying destroyed when loaded in other scene
            instance = this;
        }
        else
            Destroy(this.gameObject);

    }
    #endregion Singleton
    static public ChoiceManager instance;

    [Header("References")]
    public Animator anim;
    public GameObject go; //to disable choice screen when not needed;
    public Text question_Text; //question panel's text
    public Text[] answer_Text; // answer panels' text
    public GameObject[] answer_Panel; //answer panel gameobject

    private string question; //question from choice class

    [Header("Values")]
    public bool choiceIng; //bool var to determine if choosing to stop other movement
    private bool keyInput; //enable keyinput or movement
    private int count; //size of list
    private int result; //the choice #

    public string keySound;
    public string enterSound;
    private List<string> answerList; //list of answers from choice clas


    private WaitForSeconds waitTime = new WaitForSeconds(0.01f);


    void Start()
    {
        //initiate list and texts
        answerList = new List<string>();
        for(int i = 0; i < answer_Text.Length; i++)
        {
            answer_Text[i].text = "";
            answer_Panel[i].SetActive(false);
        }
        question_Text.text = "";
    }

    public void ShowChoice(Choice _choice)
    {
        choiceIng = true;
        go.SetActive(true);
        //initiate, set result to 0 and choiceing to true
        result = 0;
        //load question from custom class
        question = _choice.question;
        //load answers by adding to list and activate corresponding answer panel
        for(int i = 0; i < _choice.answers.Length; i++)
        {
            answerList.Add(_choice.answers[i]);
            answer_Panel[i].SetActive(true);
            count = i;
        }
        anim.SetBool("Appear", true);
        Selection();
        //start coroutine to show the panels
        StartCoroutine(ChoiceCoroutine());
    }

    IEnumerator ExitChoice()
    {
        //reset all list 
        for(int i=0; i <= count; i++)
        {
            answer_Text[i].text = "";
            answer_Panel[i].SetActive(false);
        }
        question_Text.text = "";
        answerList.Clear();
        //close animation
        anim.SetBool("Appear", false);
        //allow other movement
        yield return new WaitForSeconds(0.5f);
        go.SetActive(false);
        choiceIng = false;
    }

    IEnumerator ChoiceCoroutine()
    {
        yield return new WaitForSeconds(0.2f);

        //show question and answer depending on # of answers
        StartCoroutine(TypeQuestion());
        StartCoroutine(TypeAnswer_0());
        if(count>=1)
            StartCoroutine(TypeAnswer_1());
        if (count >= 2)
            StartCoroutine(TypeAnswer_2());
        if (count >= 3)
            StartCoroutine(TypeAnswer_3());

        yield return new WaitForSeconds(0.7f);

        //toggle to true so it can move and choose option
        keyInput = true;
    }

    IEnumerator TypeQuestion()
    {
        for(int i = 0; i < question.Length; i++)
        {
            question_Text.text += question[i];
            if (i % 10 == 1)
            {
                AudioManager.instance.Play(enterSound);
            }
            yield return waitTime;
        }
       
    }
    IEnumerator TypeAnswer_0()
    {
        yield return new WaitForSeconds(0.4f);
        for (int i = 0; i < answerList[0].Length; i++)
        {
            answer_Text[0].text += answerList[0][i];
            yield return waitTime;
        }
    }
    IEnumerator TypeAnswer_1()
    {
        yield return new WaitForSeconds(0.5f);
        for (int i = 0; i < answerList[1].Length; i++)
        {
            answer_Text[1].text += answerList[1][i];
            yield return waitTime;
        }
    }
    IEnumerator TypeAnswer_2()
    {
        yield return new WaitForSeconds(0.6f);
        for (int i = 0; i < answerList[2].Length; i++)
        {
            answer_Text[2].text += answerList[2][i];
            yield return waitTime;
        }
    }
    IEnumerator TypeAnswer_3()
    {
        yield return new WaitForSeconds(0.7f);
        for (int i = 0; i < answerList[3].Length; i++)
        {
            answer_Text[3].text += answerList[3][i];
            yield return waitTime;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (keyInput)
        {
            //keydown and up to move choice options
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                AudioManager.instance.Play(keySound);
                if (result > 0)
                    result--;
                else
                    result = count;
                Selection();
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                AudioManager.instance.Play(keySound);
                if (result < count)
                    result++;
                else
                    result = 0;
                Selection();
            }
            //z to select option
            else if (Input.GetKeyDown(KeyCode.Z))
            {
                AudioManager.instance.Play(enterSound);
                keyInput = false;
                StartCoroutine(ExitChoice());
            }
        }
    }
    public int GetResult()
    {
        int temp = result;
        result = 0;
        return temp;
    }
    private void Selection()
    {
        //change alpha value of all panels other than selected panel
        Color temp = answer_Panel[0].GetComponent<Image>().color;
        temp.a = 0.75f;
        for(int i =0; i <= count; i++)
        {
            answer_Panel[i].GetComponent<Image>().color = temp;
        }
        temp.a = 1f;
        answer_Panel[result].GetComponent<Image>().color = temp;
    }
}
