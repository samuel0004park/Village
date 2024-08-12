using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStat : MonoBehaviour
{
    #region Singleton
    public static PlayerStat instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    #endregion

    [Header("Player Stats")]
    private int recover_stamina = 3;
    public int hp;//total hp
    public int currentHp; //current hp
    public int stamina; //total stamina 
    public int currentStamina; //current stamina
    public float attack_Delay;
    public bool isDead = false;

    [Header("ETC")]
    public string dmg_sound = "beep";
    private float recover_delaytime = 0.07f;
    private float current_time;

    [Header("References")]
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private GameObject prefab_FloatingText;
    [SerializeField] private GameObject parent;
    [SerializeField] private Slider stamina_slider;
    [SerializeField] private Image[] UIhealth;

    void Start()
    {
        sprite = PlayerManager.instance.gameObject.transform.Find("Emotion").GetComponent<SpriteRenderer>();
        current_time = recover_delaytime;
        stamina_slider.maxValue = stamina;
        HeartBeat();
    }

    public void Refresh()
    {
        stamina_slider.maxValue = stamina;
        currentHp = hp;
        currentStamina = stamina;
    }
    public void HeartBeat()
    {
        stamina_slider.maxValue = stamina;
        for (int i = 0; i < currentHp; i++)
        {
            UIhealth[i].color = new Color(255, 255, 255, 255);
        }
        for (int i = currentHp; i < hp; i++)
        {
            UIhealth[i].color = new Color(1, 0, 0, 0.4f);
        }
    }
    public void Heal_HP(int _heal)
    {
        if (currentHp + _heal >= hp)
            currentHp = hp;
        else
            currentHp += _heal;
        HeartBeat();
    }
    public void Heal_Stamina(int _heal)
    {
        if (currentStamina + _heal >= stamina)
            currentStamina = stamina;
        else
            currentStamina += _heal;
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

        currentHp= currentHp-1;

        HeartBeat();


        if (currentHp<1)
        {
            Die();
        }

        AudioManager.instance.Play(dmg_sound);

        StopAllCoroutines();
        StartCoroutine(HitEffectCoroutine());
    }

    public void Die()
    {
        isDead = true;
        GameManager.instance.isLive = false;
        Collider2D[] colliders = gameObject.GetComponents<Collider2D>();
        for(int i = 0; i < colliders.Length; i++)
        {
            colliders[i].enabled = false;
        }
        PlayerManager.instance.anim.SetTrigger("isDead");
        PlayerManager.instance.StopAllCoroutines();
        UIController.instance.Die_Panel.gameObject.SetActive(true);
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
        stamina_slider.value = currentStamina;
    } //update stamina bar

    private void RecoverStamina(int _default, int additional = 0)
    {
        if (isDead)
            return;

        int total_Recovery = _default + additional;

        if (currentStamina + total_Recovery >= stamina)
            currentStamina = stamina;
        else
            currentStamina += total_Recovery;
    }
    private void Update()
    {
        if (!GameManager.instance.isLive || isDead)
            return;

        //show sweat emotion if stamina less than 100
        if (currentStamina <= 100)
            GameManager.instance.theEmo.ShowEmotion(sprite, "tear");
        else
            GameManager.instance.theEmo.HideEmotion(sprite);
        //if current stamina is less than 10, die
        if (currentStamina <= 10)
        {
            currentStamina = 0;
            GameManager.instance.theEmo.ShowEmotion(sprite, "death");
            Die();
        }

        UpdateBar();

        current_time -= Time.deltaTime;
        if(current_time <= 0)
        {
            RecoverStamina(recover_stamina);
            current_time = recover_delaytime;
        }
    }


}

