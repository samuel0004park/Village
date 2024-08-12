using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TestMove
{
    public string name;
    public string direction;
}

public class OrderTest : MonoBehaviour
{
    [SerializeField]
    public TestMove[] move;

    private OrderManager theOrder;

    public string direction;
    void Start()
    {
        theOrder = FindObjectOfType<OrderManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //when collision, load all characters in game and give predefined order
        if(collision.gameObject.name == "Player")
        {
            theOrder.PreLoadCharacter();
            //for (int i = 0; i < direction.Length; i++)
            //{theOrder.Move(move[i].name,move[i].direction);}W
         
            theOrder.Turn("Luna",direction);
        }
    }
}
