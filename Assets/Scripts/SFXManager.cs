using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public static SFXManager Instance;
    
    [SerializeField] private AudioSource sfxPlayer;

    [SerializeField] private List<AudioClip> sounds=new List<AudioClip>();
    [SerializeField] private Dictionary<string, AudioClip> soundDictionary = new Dictionary<string, AudioClip>();

    private void Awake() {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        SetUp();
        SubscribeEvents();
    }

    private void OnDestroy() {
        UnSubscribeEvents();
    }


    private void SubscribeEvents() {
        OpenFence.OnOpenFenceEvent += OpenFence_OnOpenFenceEvent;
        ChoiceAndResult.OnChoiceEvent += ChoiceAndResult_OnChoiceEvent;
        Title.OnTitleButtonPressedEvent += Title_OnButtonClickEvent;
        PlayerStat.OnPlayerDamagedEvent += PlayerStat_OnPlayerDamagedEvent;
        WeatherManager.Instance.OnWeatherChangedEvent += Instance_OnWeatherChangedEvent;
        TypeEffect.OnCharacterTypeEvent += TypeEffect_OnCharacterTypeEvent;
        ChoiceManager.Instance.OnPressEnterEvent += Instance_OnPressEnterEvent;
        ChoiceManager.Instance.OnKeyEnterEvent += Instance_OnKeyEnterEvent;
        Inventory.Instance.OnItemPickUpEvent += Instance_OnItemPickUpEvent;
        Inventory.Instance.OnInventoryStateChangedEvent += Instance_OnInventoryStateChangedEvent;
        Inventory.Instance.OnInventoryNegativeResultEvent += Instance_OnInventoryNegativeResultEvent;
        Inventory.Instance.OnInventoryNavigateEvent += Instance_OnInventoryNavigateEvent;
        OOCManager.Instance.OnOOCNavigateEvent += Instance_OnOOCNavigateEvent;
        OOCManager.Instance.OnOOCPressButtonEvent += Instance_OnOOCPressButtonEvent;
        ItemRequestObject.ItemRequestPositiveEvent += ItemRequestObject_ItemRequestPositiveEvent;
        NumberSystem.Instance.OnNumberSystemEnterAnswerEvent += Instance_OnNumberSystemEnterAnswerEvent;
        NumberSystem.Instance.OnNumberSystemSwitchEvent += Instance_OnNumberSystemSwitchEvent;
    }

   
    private void UnSubscribeEvents() {
        OpenFence.OnOpenFenceEvent -= OpenFence_OnOpenFenceEvent;
        ChoiceAndResult.OnChoiceEvent -= ChoiceAndResult_OnChoiceEvent;
        Title.OnTitleButtonPressedEvent -= Title_OnButtonClickEvent;
        PlayerStat.OnPlayerDamagedEvent -= PlayerStat_OnPlayerDamagedEvent;
        WeatherManager.Instance.OnWeatherChangedEvent -= Instance_OnWeatherChangedEvent;
        TypeEffect.OnCharacterTypeEvent -= TypeEffect_OnCharacterTypeEvent;
        ChoiceManager.Instance.OnPressEnterEvent -= Instance_OnPressEnterEvent;
        ChoiceManager.Instance.OnKeyEnterEvent -= Instance_OnKeyEnterEvent;
        Inventory.Instance.OnItemPickUpEvent -= Instance_OnItemPickUpEvent;
        Inventory.Instance.OnInventoryStateChangedEvent -= Instance_OnInventoryStateChangedEvent;
        Inventory.Instance.OnInventoryNegativeResultEvent -= Instance_OnInventoryNegativeResultEvent;
        Inventory.Instance.OnInventoryNavigateEvent -= Instance_OnInventoryNavigateEvent;
        OOCManager.Instance.OnOOCPressButtonEvent -= Instance_OnOOCPressButtonEvent;
        OOCManager.Instance.OnOOCNavigateEvent -= Instance_OnOOCNavigateEvent;
        ItemRequestObject.ItemRequestPositiveEvent -= ItemRequestObject_ItemRequestPositiveEvent;
        NumberSystem.Instance.OnNumberSystemEnterAnswerEvent -= Instance_OnNumberSystemEnterAnswerEvent;
        NumberSystem.Instance.OnNumberSystemSwitchEvent -= Instance_OnNumberSystemSwitchEvent;
    }

    private void Instance_OnNumberSystemSwitchEvent(object sender, System.EventArgs e) {
        Play("Switch");
    }

    private void Instance_OnNumberSystemEnterAnswerEvent(object sender, NumberSystem.OnNumberSystemEnterAnswerEventArgs e) {
        if(e.isCorrect)
            Play("Button_Positive");
        else
            Play("Beep");
    }


    private void ItemRequestObject_ItemRequestPositiveEvent(object sender, System.EventArgs e) {
        Play("Button_Positive");
    }

    private void Instance_OnInventoryNavigateEvent(object sender, System.EventArgs e) {
        Play("Popup");
    }


    private void Instance_OnOOCPressButtonEvent(object sender, OOCManager.OnOOCPressButtonEventArgs e) {
        if (e.result)
            Play("Button_Positive");
        else
            Play("Beep");
    }

    private void Instance_OnOOCNavigateEvent(object sender, System.EventArgs e) {
        Play("Switch");
    }

    private void Instance_OnInventoryNegativeResultEvent(object sender, System.EventArgs e) {
        Play("Beep");
    }

    private void Instance_OnInventoryStateChangedEvent(object sender, Inventory.OnInventoryStateChangedEventArgs e) {
        Play("Popup");
    }

    private void Instance_OnItemPickUpEvent(object sender, System.EventArgs e) {
        Play("Button_Positive");
    }

    private void Instance_OnKeyEnterEvent(object sender, System.EventArgs e) {
        Play("Switch");
    }

    private void Instance_OnPressEnterEvent(object sender, System.EventArgs e) {
        Play("Button");
    }

    private void TypeEffect_OnCharacterTypeEvent(object sender, System.EventArgs e) {
        Play("Switch");
    }


    private void Instance_OnWeatherChangedEvent(object sender, WeatherManager.OnWeatherChangedEventArgs e) {
        //if (e.weather != "")
        //    Play("rain_sound");
        //else
        //    Stop("rain_sound");
    }

    private void PlayerStat_OnPlayerDamagedEvent(object sender, System.EventArgs e) {
        Play("Beep");
    }

    private void Title_OnButtonClickEvent(object sender, System.EventArgs e) {
        Play("Button_Positive");
    }

    private void ChoiceAndResult_OnChoiceEvent(object sender, System.EventArgs e) {
        Play("Button_Negative");
    }

    private void OpenFence_OnOpenFenceEvent(object sender, System.EventArgs e) {
        Play("Beep");
    }

    private void SetUp()
    {
        for (int i = 0; i < sounds.Count; i++)
        {
            soundDictionary.Add(sounds[i].name, sounds[i]);
        }
    }

    public void Play(string sfx) {
        //get string as arguement and load correct audio 
        if (!soundDictionary.ContainsKey(sfx))
            return;

        sfxPlayer.clip = soundDictionary[sfx];
        sfxPlayer.Play();
    }


}
