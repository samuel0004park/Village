using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OOCManager : MonoBehaviour
{

    public static OOCManager Instance;

    [SerializeField] private GameObject up_Panel;
    [SerializeField] private GameObject down_Panel;

    [SerializeField] private Text up_Text;
    [SerializeField] private Text down_Text;

    public bool result { get; private set; }
    public bool activated { get; private set; }

    private bool keyInput;
    
    public event EventHandler OnOOCNavigateEvent;
    public event EventHandler<OnOOCPressButtonEventArgs> OnOOCPressButtonEvent;
    public class OnOOCPressButtonEventArgs : EventArgs {
        public bool result;
    }

    private void Awake() {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Update() {
        if (!keyInput) return;

        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow)) Selected();
        else if (Input.GetKeyDown(KeyCode.Z)) UseItem();
        else if (Input.GetKeyDown(KeyCode.X)) CancelChoice();
    }


    public void ShowChoice(string _upText, string _downText)
    {
        activated = true;
        result = true;
        up_Text.text = _upText;
        down_Text.text = _downText;

        TogglePanels(result);

        Invoke(nameof(EnableKeyInput), 0.5f);
    }

    private void EnableKeyInput()
    {
        keyInput = true;
    }

    private void Selected() {
        result = !result;
        TogglePanels(result);
        OnOOCNavigateEvent?.Invoke(this, EventArgs.Empty);
    }

    private void TogglePanels(bool state) {
        up_Panel.gameObject.SetActive(state);
        down_Panel.gameObject.SetActive(!state);
    }

    private void UseItem() {
        OnOOCPressButtonEvent?.Invoke(this, new OnOOCPressButtonEventArgs { result = true });
        keyInput = false;
        activated = false;
    }

    private void CancelChoice() {
        OnOOCPressButtonEvent?.Invoke(this, new OnOOCPressButtonEventArgs { result = false});
        result = false;
        activated = false;
        keyInput = false;
    }

}
