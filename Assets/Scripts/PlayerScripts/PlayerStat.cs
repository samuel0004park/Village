using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStat : MonoBehaviour {
    
    private const int RECOVER_STAMINA = 1;
    public const int MAX_STAMINA = 100;
    public const int MAX_HP = 1;

    public int currentHp { get; private set; }
    public int currentStamina { get; private set; }
    public bool isDead { get; private set; }


    [Header("ETC")]
    private float recover_delaytime = 0.07f;
    private float current_time;

    [Header("References")]
    [SerializeField] private SpriteRenderer emotionSprite;

    public event EventHandler<OnPlayerHeartBeatEventArgs> OnPlayerHeartBeatEvent;
    public static event EventHandler OnPlayerDamagedEvent;
    public static event EventHandler OnPlayerKilledEvent;
    public static event EventHandler<OnPlayerEmotionChangedEventArgs> OnPlayerEmotionChangedEvent;

    public class OnPlayerEmotionChangedEventArgs : EventArgs {
        public EmotionManager.Emotion emotion;
        public SpriteRenderer sprite;
    }
    public class OnPlayerHeartBeatEventArgs : EventArgs {
        public int currentStamina;
        public int currentLifeCount;
    }

    void Start()
    {
        current_time = recover_delaytime;
    }

    public bool CheckStamina(int _deducAmount) {
        if (currentStamina - _deducAmount <= 0) {
            return false;
        }
        else
            return true;
    }



    public void LoadStat(int playerCurrentHP, int playerCurrentMP) {
        currentHp = playerCurrentHP;
        currentStamina = playerCurrentMP;

        UpdateBar();
    }


    public void Heal_HP(int _heal)
    {
        if (currentHp + _heal >= MAX_HP)
            currentHp = MAX_HP;
        else
            currentHp += _heal;
     
        UpdateBar();
    }

    public void Heal_Stamina(int _heal)
    {
        if (currentStamina + _heal >= MAX_STAMINA)
            currentStamina = MAX_STAMINA;
        else
            currentStamina += _heal;

        UpdateBar();
    }

    public void StaminaBuff(int _time)
    {
        if (isDead)
            return;

        StartCoroutine(BuffCoroutine(_time));
    }

    IEnumerator BuffCoroutine(int _time)
    {
        //increase player stamina regen for given time
        float temp = recover_delaytime;
        recover_delaytime = recover_delaytime / 5f;

        yield return new WaitForSeconds(_time);

        recover_delaytime = temp;
    }

    public void Hit()
    {
        if (isDead)
            return;

        currentHp--;

        UpdateBar();
        OnPlayerDamagedEvent?.Invoke(this, EventArgs.Empty);

        if (currentHp<1)
        {
            Die();
        }

        StopAllCoroutines();
        StartCoroutine(HitEffectCoroutine());
    }

    public void Die()
    {
        isDead = true;
        Collider2D[] colliders = gameObject.GetComponents<Collider2D>();
        for(int i = 0; i < colliders.Length; i++)
        {
            colliders[i].enabled = false;
        }

        OnPlayerKilledEvent?.Invoke(this, EventArgs.Empty);
        OnPlayerHeartBeatEvent?.Invoke(this, new OnPlayerHeartBeatEventArgs { currentStamina = 0, currentLifeCount = 0 });
    }

    IEnumerator HitEffectCoroutine()
    {
        Color color = GetComponent<SpriteRenderer>().color;
        color.a = 0;
        GetComponent<SpriteRenderer>().color = color;
        yield return new WaitForSeconds(0.1f);
        color.a = 1;
        GetComponent<SpriteRenderer>().color = color;
        yield return new WaitForSeconds(0.1f);
        color.a = 0;
        GetComponent<SpriteRenderer>().color = color;
        yield return new WaitForSeconds(0.1f);
        color.a = 1;
        GetComponent<SpriteRenderer>().color = color;
        yield return new WaitForSeconds(0.1f);
        color.a = 0;
        GetComponent<SpriteRenderer>().color = color;
        yield return new WaitForSeconds(0.1f);
        color.a = 1;
        GetComponent<SpriteRenderer>().color = color;
    }

    public void UpdateBar()
    {
        OnPlayerHeartBeatEvent?.Invoke(this, new OnPlayerHeartBeatEventArgs { currentStamina = currentStamina, currentLifeCount = currentHp });
    } 

    private void RecoverStamina(int _default, int additional = 0)
    {
        if (isDead)
            return;

        int total_Recovery = _default + additional;

        if (currentStamina + total_Recovery >= MAX_STAMINA)
            currentStamina = MAX_STAMINA;
        else
            currentStamina += total_Recovery;
    }

    public void DecreaseStamina(int decrementOffset) {
        if (currentStamina - decrementOffset < 0)
            currentStamina = 0;
        else
            currentStamina -= decrementOffset;
        UpdateBar();
    }

    private void HandleStamina() {
        if (currentStamina <= 0) {
            OnPlayerEmotionChangedEvent?.Invoke(this, new OnPlayerEmotionChangedEventArgs { emotion = EmotionManager.Emotion.DEATH, sprite = emotionSprite });
            Die();
        }
        else if (currentStamina <= 30) {
            OnPlayerEmotionChangedEvent?.Invoke(this, new OnPlayerEmotionChangedEventArgs { emotion = EmotionManager.Emotion.TEAR, sprite = emotionSprite });
        }
        else {
            if (emotionSprite.color.a != 0f)
                OnPlayerEmotionChangedEvent?.Invoke(this, new OnPlayerEmotionChangedEventArgs { emotion = EmotionManager.Emotion.NONE, sprite = emotionSprite });
        }
    }

    private void Update()
    {
        if (!GameManager.Instance.isLive || isDead)
            return;

        HandleStamina();

        current_time -= Time.deltaTime;
        if(current_time <= 0)
        {
            RecoverStamina(RECOVER_STAMINA);
            current_time = recover_delaytime;
        }
    }


}

