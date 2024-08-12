using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeController : MovingObject
{
    [Header("Delay")]
    [SerializeField]private float inter_MoveWaitTime; // wait time in between move
    [SerializeField] private float attackDelay; // wait time after attack
    private float current_interMWT;
    private string attack_sound = "beep";
    [Header("References")]
    private Vector2 playerPos; //position of player
    private int random_int;
    private string direction;
    private SpriteRenderer sprite;
    private LayerMask player_layerMask;

    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
        queue = new Queue<string>();
        current_interMWT = inter_MoveWaitTime;
        player_layerMask = LayerMask.GetMask("Player");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //do countdown
        current_interMWT -= Time.deltaTime;
        //if coutdown reaches 0, reset counter and move slime
        if (current_interMWT <= 0)
        {
            current_interMWT = inter_MoveWaitTime + Random.Range(0,3);

            if (NearPlayer())
            {
                Flip();
                return; 
            }

            RandomDirection();
            ChangeSprite();
            //if there is collision ahead, do not move 
            if (base.CheckCollision())
            {
                return;
            }
            base.Move(direction);
        }
    }


    private void Flip()
    {

        //if player is behind, flip
        if (playerPos.x < this.transform.position.x)
        {
            sprite.flipX = true;
        }
        //if player is infront, do not flip
        else if (playerPos.x > this.transform.position.x)
        {
            sprite.flipX = false;
        }

        //trigger animation 
        anim.SetTrigger("Attack"); 
        StartCoroutine(WaitCoroutine());
    }
    IEnumerator WaitCoroutine()
    {
        //wait for delay and do damage after
        yield return new WaitForSeconds(attackDelay);
        AudioManager.instance.Play(attack_sound);
        if (NearPlayer())
            PlayerStat.instance.Hit();

    } 
    private bool NearPlayer()
    {
        playerPos = PlayerManager.instance.transform.position;
        //1.1f for a block distance
        //0.5f for no distance
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, 1f, Vector2.zero, 0, player_layerMask);

        return (hits.Length != 0);
    }
    private void ChangeSprite()
    {
        //change sprite only if looking at opposite direction, up down does not change
        if (direction == "LEFT")
            sprite.flipX = true;
        else if(direction == "RIGHT")
            sprite.flipX = false;
    }

    private void RandomDirection()
    {
        //get a random direction
        vector.Set(0, 0);
        random_int = Random.Range(0, 4);
        switch (random_int)
        {
            case 0:
                vector.x = 1f;
                direction = "UP";
                break;
            case 1:
                vector.x = -1f;
                direction = "DOWN";
                break;
            case 2:
                vector.y = 1f;
                direction = "RIGHT";
                break;
            case 3:
                vector.y = -1f;
                direction = "LEFT";
                break;
        }

    }
}
