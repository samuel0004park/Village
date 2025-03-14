using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChoiceManager : MonoBehaviour
{
   

    static public ChoiceManager Instance;

    [SerializeField] private Animator anim;
    [SerializeField] private CanvasGroup CanvasGroup;
    [SerializeField] private Text question_Text; 
    [SerializeField] private Text[] answer_Text; 
    [SerializeField] private GameObject[] answer_Panel;

    public bool choiceIng { get; private set; } 
    private string question; //question from choice class
    private bool enableKeyInput; 
    private int count; 
    private int choiceNumber;

    private List<string> answerList;

    public event EventHandler OnKeyEnterEvent; 
    public event EventHandler OnPressEnterEvent; 

    private WaitForSeconds waitTime = new WaitForSeconds(0.01f);

    private void Awake() {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        HideVisuals();
        SetUp();
    }

    private void Update() {
        if (!enableKeyInput) return;

        if (Input.GetKeyDown(KeyCode.UpArrow)) ChangeSelection(-1);
        else if (Input.GetKeyDown(KeyCode.DownArrow)) ChangeSelection(1);
        else if (Input.GetKeyDown(KeyCode.Z)) SelectOption();
    }


    private void SetUp() {
        answerList = new List<string>();
        for (int i = 0; i < answer_Text.Length; i++) {
            answer_Text[i].text = "";
            answer_Panel[i].SetActive(false);
        }
        question_Text.text = "";
    }

    public void ShowChoice(Choice _choice)
    {
        ShowVisuals();

        choiceNumber = 0;
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

        yield return new WaitForSeconds(1f);
        HideVisuals();
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
 
        yield return new WaitForSeconds(0.7f);

        //toggle to true so it can move and choose option
        enableKeyInput = true;
    }

    IEnumerator TypeQuestion()
    {
        for(int i = 0; i < question.Length; i++)
        {
            question_Text.text += question[i];
            if (i % 10 == 1)
            {
                OnPressEnterEvent?.Invoke(this, EventArgs.Empty);
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


    public void HideVisuals() {
        choiceIng = false;
        CanvasGroup.interactable = false;
        CanvasGroup.alpha = 0f;
    }

    public void ShowVisuals() {
        choiceIng = true;
        CanvasGroup.interactable = true;
        CanvasGroup.alpha = 1f;
    }

    private void ChangeSelection(int direction) {
        OnKeyEnterEvent?.Invoke(this, EventArgs.Empty);

        choiceNumber = (choiceNumber + direction + count + 1) % (count + 1);
        Selection();
    }

    private void SelectOption() {
        OnPressEnterEvent?.Invoke(this, EventArgs.Empty);
        enableKeyInput = false;
        StartCoroutine(ExitChoice());
    }

    public int GetResult()
    {
        int temp = choiceNumber;
        choiceNumber = 0;
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
        answer_Panel[choiceNumber].GetComponent<Image>().color = temp;
    }
}
