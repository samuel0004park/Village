using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NumberSystem : MonoBehaviour
{
    [Header("Audio")]
    private string key_sound= "switch_sound";
    private string enter_sound= "enter_sound";
    private string cancel_sound = "cancel_sound";
    private string correct_sound= "open_sound";

    [Header("Variables")]
    private int correctNumber;
    private int count; //size of array (ex. 1000 is 3)
    private int selectedTextBox; //the location of current selected box
    private int result; //player answer
    private string tempNumber;

    [Header("References")]
    public GameObject superObject;
    public GameObject[] panel;
    public Text[] Number_Text;

    public Animator anim;

    //public float space;

    public bool activated; //to stop other player movement
    private bool keyInput; //to enable keyinput or movement

    private bool correctFlag;

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

        //move panel to center depending on length of answer
        //superObject.transform.position = new Vector3(superObject.transform.position.x + space * count, superObject.transform.position.y, superObject.transform.position.z);

        //show panel and allow keyInput
        AudioManager.instance.Play(enter_sound);
        SetColor();
        anim.SetBool("Appear", true);
        StartCoroutine(WaitCoroutine());
    }
    IEnumerator WaitCoroutine()
    {
        yield return new WaitForSeconds(0.5f);
        keyInput = true;
    }
    public void SetNumber(string _arrow)
    {
        //increment or decrement selected number

        int temp = int.Parse(Number_Text[selectedTextBox].text);

        if(_arrow == "Down")
        {
            if (temp == 0)temp = 9;
            else temp--;
        }
        else if (_arrow == "Up")
        {
            if (temp == 9) temp = 0;
            else temp++;

        }

        Number_Text[selectedTextBox].text = temp.ToString();
    }
    private void SetColor()
    {
        //change alpha value of all panels other than selected panel
        Color temp = Number_Text[0].color;
        temp.a = 0.3f;
        for(int i = 0; i <= count; i++)
        {
            Number_Text[i].color = temp;
        }
        temp.a = 1f;
        Number_Text[selectedTextBox].color = temp;
    }

    private void Update()
    {
        if (keyInput)
        {
            //decrement number
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                AudioManager.instance.Play(key_sound);
                SetNumber("Down");
            }
            //increment number
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                AudioManager.instance.Play(key_sound);
                SetNumber("Up");
            }
            //move right
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                AudioManager.instance.Play(key_sound);
                if (selectedTextBox > 0)
                    selectedTextBox--;
                else
                    selectedTextBox = count;
                SetColor();
            }
            //move left
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                AudioManager.instance.Play(key_sound);
                if (selectedTextBox < count)
                    selectedTextBox++;
                else
                    selectedTextBox = 0;
                SetColor();
            }
            //enter answer to check if true
            else if (Input.GetKeyDown(KeyCode.Z))
            {
                AudioManager.instance.Play(enter_sound);
                keyInput = false;
                StartCoroutine(OXCoroutine());
            }
            //cancel number pad 
            else if (Input.GetKeyDown(KeyCode.X))
            {
                AudioManager.instance.Play(cancel_sound);
                keyInput = false;
                StartCoroutine(ExitCoroutine());
            }
        }
    }

    public bool GetResult()
    {
        return correctFlag;
    }

    IEnumerator OXCoroutine()
    {
        //change back to original alpha value for all panels
        Color temp = Number_Text[0].color;
        temp.a = 1f;
        for (int i = count; i >=0; i--)
        {
            Number_Text[i].color = temp;
            tempNumber += Number_Text[i].text;
        }
        // wait a little
        yield return new WaitForSeconds(1f);

        //check answer
        result = int.Parse(tempNumber);

        if (result == correctNumber)
        {
            AudioManager.instance.Play(correct_sound);
            correctFlag = true;
        }
        if (result != correctNumber)
        {
            AudioManager.instance.Play(cancel_sound);
            correctFlag = false;
        }

        //start exit coroutine
        StartCoroutine(ExitCoroutine());
    }
  
    IEnumerator ExitCoroutine()
    {
        Debug.Log("Our answer" + result+ " / Correct Answer:" + correctNumber);

        //reset used values
        selectedTextBox = 0;
        result = 0;
        tempNumber = "";
        anim.SetBool("Appear", false);

        //wait a little for animation to end and then disable panels 
        yield return new WaitForSeconds(0.1f);

        for (int i = 0; i < count; i++)
        {
            panel[i].SetActive(false);
        }
        activated = false;

        //move panel to original position   
        //superObject.transform.position = new Vector3(superObject.transform.position.x - space * count, superObject.transform.position.y, superObject.transform.position.z);

    }
}
