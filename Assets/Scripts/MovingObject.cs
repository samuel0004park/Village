using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObject : MonoBehaviour
{
    [Header("Shared Variables")]
    public string characterName;
    public float speed;
    public int walkCount;
    public Animator anim;
    
    public int currentWalkCount;
    [HideInInspector]
    public BoxCollider2D boxCollider;

    public Queue<string> queue;
    private bool notCoroutine = false;

    [Header("Protected Variables(no need to be seen)")]
    public GameObject scanObject;
    protected Vector2 vector;
    protected Vector3 dirVec;

   
    public void Move(string _dir, int _frequency=5)
    {
        queue.Enqueue(_dir);
        //if not Coroutine, which is the beginning, set coroutine to true and start a new coroutine
        if (!notCoroutine)
        {
            notCoroutine = true;
            StartCoroutine(MoveCoroutine(_dir, _frequency));
        }
    } //use queue to process move movable objects
    
    IEnumerator MoveCoroutine(string _dir, int _frequency)
    {
        
        //while there is an order in queue
        while (queue.Count!=0)
        {
            //wait time is taken after every movement frequency
            switch (_frequency)
            {
                case 1:
                    yield return new WaitForSeconds(4f);
                    break;
                case 2:
                    yield return new WaitForSeconds(3f);
                    break;
                case 3:
                    yield return new WaitForSeconds(2f);
                    break;
                case 4:
                    yield return new WaitForSeconds(1f);
                    break;
                case 5:
                    break;
            }

            //pop the order in queue
            string direction = queue.Dequeue();

            //set vector to zero so vectors do not overlap
            vector.Set(0, 0);

            //set vector with coresponding direction 
            switch (direction)
            {
                case "UP":
                    vector.y = 1;
                    break;
                case "DOWN":
                    vector.y = -1;
                    break;
                case "RIGHT":
                    vector.x = 1;
                    break;
                case "LEFT":
                    vector.x = -1;
                    break;
            }

            //set animation values
            anim.SetFloat("DirX", vector.x);
            anim.SetFloat("DirY", vector.y);

            //check for collision, if there is collision ahead, wait 
            while (true)
            {
                bool checkCollisionFlag = CheckCollision();
                if (checkCollisionFlag)
                {
                    anim.SetBool("isWalking", false);
                    yield return new WaitForSeconds(1f);
                    if (characterName == "Slime") { 
                        notCoroutine = false;
                        StopAllCoroutines();
                    }
                }
                else
                    break;
            }

            //enlarge box collider so other objects do not pass 
            EnlargeCollider();
            anim.SetBool("isWalking", true);

            //actual translation
            while (currentWalkCount < walkCount)
            {
                //translate horizontal or vertical 
                if (vector.x != 0)
                    transform.Translate((vector.x * (speed )) / 200, 0, 0); //divided by 200 b/c sprite is too small 
                else if (vector.y != 0)
                    transform.Translate(0, (vector.y * (speed )) / 200, 0);

                currentWalkCount++;

                if (currentWalkCount == walkCount)
                    boxCollider.size = new Vector2(1f, 1f);

                yield return new WaitForSeconds(0.01f);
            }

            //end walk animation and reset
            if (_frequency != 5)
                anim.SetBool("isWalking", false);
            currentWalkCount = 0;
        }
        //reset so next action is available
        anim.SetBool("isWalking", false);
        notCoroutine = false;
    } //start coroutine to actually move objects

    protected bool CheckCollision()
    {
        //get vectors for raycast
        //start from currrent pos to destination
        Vector2 start = transform.position;
        Vector2 end = start + new Vector2(vector.x * speed * walkCount, vector.y * speed * walkCount);

        //disable box collider so it does not catch its own
        boxCollider.enabled = false;
        RaycastHit2D hit = Physics2D.Raycast(start, end, 1f, LayerMask.GetMask("NoPass") | LayerMask.GetMask("Object") | LayerMask.GetMask("Player"));
        boxCollider.enabled = true;

        //check direction 
        CheckDirection();

        if (hit.collider != null)
            return true;
       
        return false;
    }
    protected void CheckDirection()
    {
        //save a direction vector to know where the gameObejct is currently facing
        if (vector.x != 0)
            dirVec = vector.x == 1 ? Vector3.right : Vector3.left;
        else if (vector.y != 0)
            dirVec = vector.y == 1 ? Vector3.up : Vector3.down;
    }

    public void EnlargeCollider()
    {
        if (vector.x != 0)
            boxCollider.size = new Vector2(2.1f, 1f);
        else
            boxCollider.size = new Vector2(1f, 2.1f);
    }
}

