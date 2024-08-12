using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MovingObject
{
    [Header("Input Values:")]
    private float h;
    private float v;
    
    [Header("Player Stats:")]
    public bool interact;
    [SerializeField] private int atkStamina;

    [Space]
    [Header("References:")]
    private Rigidbody2D rigid;
    private PlayerStat theStat;
   
    [Header("Location Info")]
    public Location.MapNames currentMapName; //saves current map name 
    public Location.SceneNames currentSceneName; //current scene name

    [Space]
    [Header("Audio")]
    private string walkSound_1= "footsteps 2";
    private string attack_sound= "slash_sound";

    [Header("Variables:")]
    public bool notMove = false;//var so player does not move during dialogue
    public bool inputDelay = true;
    public bool attacking = false;
    private float currentAttackDelay;
    public bool loading;

    #region Singleton
    public static PlayerManager instance;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }
    #endregion
    void Start()
    {
        queue = new Queue<string>();
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        theStat = FindObjectOfType<PlayerStat>();
    }
    public void FixedUpdate()
    {
        if (PlayerStat.instance.isDead)
            return;

        //draw ray
        Debug.DrawRay(rigid.position, dirVec * 0.7f, new Color(0, 1, 0));

        //use raycast to check colliders in front of player
        RaycastHit2D hit = Physics2D.Raycast(rigid.position, dirVec, 0.7f, LayerMask.GetMask("Object")| LayerMask.GetMask("Item"));

        if (hit.collider != null)
            scanObject = hit.collider.gameObject;
        else
            scanObject = null;
    }
    void Update()
    {
        if (PlayerStat.instance.isDead)
            return;

        PlayerInputLogic();
        AttackLogic();
        InteractionLogic();
    }

    private void PlayerInputLogic()
    {
        //get horizontal and vertical axis values of player
        h = Input.GetAxisRaw("Horizontal");
        v = Input.GetAxisRaw("Vertical");
        if (inputDelay && !notMove && !attacking)
        {
            if (h != 0 || v != 0)
            {
                inputDelay = false;
                StartCoroutine(MoveCoroutine());
            }
        }
        else
        {
            //set velocity to zero and stop all movement if there is no input 
            rigid.velocity = Vector2.zero;
            h = 0;
            v = 0;
        }
    }

    private void InteractionLogic()
    {
        //press z for interactable that allows event collider to start event
        if (Input.GetKey(KeyCode.Z) && scanObject != null && !UIController.instance.pannelUP && !ChoiceManager.instance.choiceIng)
        {
            interact = true;
            //check if scaned object has dialogue and play dialogue if true
            if (scanObject.gameObject.layer == 7)
            {
                ObjectDialogue temp = scanObject.GetComponent<ObjectDialogue>();
                if (temp != null)
                {
                    temp.Talk();
                }
            }
        }
          if (Input.GetKeyUp(KeyCode.Z))
            interact = false;
    }
    private void AttackLogic()
    {
        //spacebar to attack while not moving
        if (!notMove && !attacking)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                theStat.currentStamina -= atkStamina;
                currentAttackDelay = theStat.attack_Delay;
                attacking = true;
                anim.SetBool("isAttacking", true);
                AudioManager.instance.Play(attack_sound);
            }
        }
        //can attack again after delay
        if (attacking)
        {
            currentAttackDelay -= Time.deltaTime;
            if (currentAttackDelay <= 0)
            {
                anim.SetBool("isAttacking", false);
                attacking = false;
            }
        }
        if (Input.GetKeyUp(KeyCode.Z))
            interact = false;
    }
    
    public void ProcessInputs()
    {
        //if can move, (so there is only one coroutine active for player)
        //check if there has been any input from keyboard

        if (h != 0 || v != 0)
        {
            //start coroutine
            inputDelay = false;
            StartCoroutine(MoveCoroutine());
        }
        else
        {
            //set velocity to zero if there is no input 
            rigid.velocity = Vector2.zero;
        }
    }

    IEnumerator MoveCoroutine()
    {
        while (h != 0 || v != 0 && !notMove && !attacking)
        {
            //set a vector with x and y for horizontal and vertical movement 
            vector.Set(h, v);

            if (vector.y != 0)
                vector.x = 0;

            //set animation
            anim.SetFloat("DirX", vector.x);
            anim.SetFloat("DirY", vector.y);


            bool checkCollisionFlag = CheckCollision();
            if (checkCollisionFlag)
                break;

            anim.SetBool("isWalking", true);

            //enlarge box collider so other objects do not pass 
            EnlargeCollider();

            AudioManager.instance.Play(walkSound_1);

            //loop total of walkCount(number of byte steps taken)
            while (currentWalkCount < walkCount)
            {
                //translate horizontal and vertical and get direction vector for raycast
                if (vector.x != 0)
                    transform.Translate((vector.x * (speed)) / 200, 0, 0); //divided by 200 b/c sprite is too small 
                else if (vector.y != 0)
                    transform.Translate(0, (vector.y * (speed)) / 200, 0);

                currentWalkCount++;

                //return box collider to normal
                if(currentWalkCount == walkCount * 0.5f+4)
                    boxCollider.size = new Vector2(1f, 1f);

                yield return new WaitForSeconds(0.01f);
            }
            currentWalkCount = 0;
        }
        anim.SetBool("isWalking", false);
        inputDelay = true;
    }
    public bool CheckStamina(int _deducAmount)
    {
        if (theStat.currentStamina - _deducAmount <= 0)
        {
            return false;
        }
        else
            return true;
    }
 
    
}
