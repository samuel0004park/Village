using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NumberSystem : MonoBehaviour
{

    public static NumberSystem Instance;

    public bool activated { get; private set; }
    public bool correctFlag { get; private set; }

    private int correctNumber;
    private int count; 
    private int selectedTextBox;
    private int result;
    private string tempNumber;
    private bool keyInput;

    [SerializeField] private GameObject superObject;
    [SerializeField] private GameObject[] panel;
    [SerializeField] private Text[] Number_Text;
    [SerializeField] private Animator anim;


    public event EventHandler OnNumberSystemSwitchEvent;
    public event EventHandler<OnNumberSystemEnterAnswerEventArgs> OnNumberSystemEnterAnswerEvent;
    public class OnNumberSystemEnterAnswerEventArgs : EventArgs {
        public bool isCorrect;
    }

    private void Awake() {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void ShowNumber(int _correctNumber)
    {
        //initialize all values and save correct answer 
        correctNumber = _correctNumber;
        keyInput = false;
        activated = true;
        correctFlag = false;
        selectedTextBox = 0;
        result = 0;  

        //only activate needed panels and set their text to 0
        string temp = correctNumber.ToString();
        for(int i =0; i< temp.Length; i++)
        {
            count = i;
            panel[i].SetActive(true);
            Number_Text[i].text = "0";
        }

        SetColor();
        anim.SetBool("Appear", true);
        StartCoroutine(WaitCoroutine());
    }

    IEnumerator WaitCoroutine()
    {
        yield return new WaitForSeconds(0.5f);
        keyInput = true;
    }


    private void Update() {
        if (!keyInput) return;

        if (Input.GetKeyDown(KeyCode.DownArrow)) HandleNumberChange("Down");
        else if (Input.GetKeyDown(KeyCode.UpArrow)) HandleNumberChange("Up");
        else if (Input.GetKeyDown(KeyCode.RightArrow)) MoveSelection(-1);
        else if (Input.GetKeyDown(KeyCode.LeftArrow)) MoveSelection(1);
        else if (Input.GetKeyDown(KeyCode.Z)) SubmitAnswer();
        else if (Input.GetKeyDown(KeyCode.X)) CancelInput();
    }

    private void HandleNumberChange(string direction) {
        OnNumberSystemSwitchEvent?.Invoke(this, EventArgs.Empty);
        SetNumber(direction);
    }

    private void MoveSelection(int direction) {
        OnNumberSystemSwitchEvent?.Invoke(this, EventArgs.Empty);

        selectedTextBox = (selectedTextBox + direction + count + 1) % (count + 1);
        SetColor();
    }

    private void SubmitAnswer() {
        keyInput = false;
        StartCoroutine(OXCoroutine());
    }

    private void CancelInput() {
        OnNumberSystemEnterAnswerEvent?.Invoke(this, new OnNumberSystemEnterAnswerEventArgs { isCorrect = false });
        keyInput = false;
        StartCoroutine(ExitCoroutine());
    }



    IEnumerator OXCoroutine()
    {
        ResetColor();
        // wait a little
        yield return new WaitForSeconds(1f);

        //check answer
        result = int.Parse(tempNumber);

        if (result == correctNumber)
        {
            OnNumberSystemEnterAnswerEvent?.Invoke(this, new OnNumberSystemEnterAnswerEventArgs { isCorrect = true });
            correctFlag = true;
        }
        if (result != correctNumber)
        {
            OnNumberSystemEnterAnswerEvent?.Invoke(this, new OnNumberSystemEnterAnswerEventArgs { isCorrect = false });
            correctFlag = false;
        }

        //start exit coroutine
        StartCoroutine(ExitCoroutine());
    }
  
    IEnumerator ExitCoroutine()
    {
        //reset used values
        selectedTextBox = 0;
        result = 0;
        tempNumber = "";
        anim.SetBool("Appear", false);

        yield return new WaitForSeconds(0.1f);

        for (int i = 0; i < count; i++)
        {
            panel[i].SetActive(false);
        }
        activated = false;
    }

    public void SetNumber(string _arrow) {
        //increment or decrement selected number
        int temp = int.Parse(Number_Text[selectedTextBox].text);

        if (_arrow == "Down") {
            if (temp == 0) temp = 9;
            else temp--;
        }
        else{
            if (temp == 9) temp = 0;
            else temp++;
        }

        Number_Text[selectedTextBox].text = temp.ToString();
    }


    private void ResetColor() {
        //change back to original alpha value for all panels
        Color temp = Number_Text[0].color;
        temp.a = 1f;
        for (int i = count; i >= 0; i--) {
            Number_Text[i].color = temp;
            tempNumber += Number_Text[i].text;
        }
    }

    private void SetColor() {
        //change alpha value of all panels other than selected panel
        Color temp = Number_Text[0].color;
        temp.a = 0.3f;
        for (int i = 0; i <= count; i++) {
            Number_Text[i].color = temp;
        }
        temp.a = 1f;
        Number_Text[selectedTextBox].color = temp;
    }
}
