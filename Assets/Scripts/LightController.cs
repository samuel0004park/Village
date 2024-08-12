using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightController : MonoBehaviour
{
    private PlayerManager thePlayer;
    private Vector2 vector;//vector to save player's looking direction
    private SpriteRenderer sprite;
    private Color alpha_255;
    private Color alpha_0;
    private Quaternion rotation; //rotation vector

    public SpriteRenderer black;
    private Color color;

    #region Singleton
    static public LightController instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
            Destroy(this.gameObject);
        DontDestroyOnLoad(this.gameObject);
    }
    #endregion 

    void Start()
    {
        thePlayer = FindObjectOfType<PlayerManager>();
        sprite = GetComponent<SpriteRenderer>();
        alpha_255.a = 255;
        alpha_0.a = 0;
    }

    public void LightsOut()
    {
        Color temp = black.color;
        temp.a = 255;
        black.color = temp;
    }

    public void LightsIn()
    {
        Color temp = black.color;
        temp.a = 0;
        black.color = temp;
    }
    public void TurnOnFlash()
    {

        StartCoroutine(FlashOnCoroutine());
        
    }

    IEnumerator FlashOnCoroutine()
    {
        LightsIn();
        sprite.color = alpha_255;
        yield return new WaitForSeconds(0.5f);
        LightsOut();
        sprite.color = alpha_0;
        yield return new WaitForSeconds(0.5f);
        LightsIn();
        sprite.color = alpha_255;
        yield return new WaitForSeconds(0.2f);
        LightsOut();
        sprite.color = alpha_0;
        yield return new WaitForSeconds(0.2f);
        LightsIn();
        sprite.color = alpha_255;
        yield return new WaitForSeconds(0.2f);
        LightsOut();
        sprite.color = alpha_0;
        yield return new WaitForSeconds(0.1f);
        LightsIn();
        sprite.color = alpha_255;
        yield return new WaitForSeconds(0.1f);
        LightsOut();
        sprite.color = alpha_0;
        yield return new WaitForSeconds(0.5f);
        LightsIn();
        sprite.color = alpha_255;
    }

    public void TurnOffFlash()
    {
        sprite.color = alpha_0;
    }
    // Update is called once per frame
    void Update()
    {
        this.transform.position = thePlayer.transform.position;

        vector.Set(thePlayer.anim.GetFloat("DirX"), thePlayer.anim.GetFloat("DirY"));
        
        if(vector.x == 1f)
        {
            rotation = Quaternion.Euler(0, 0, 90);
            this.transform.rotation = rotation;
        }
        else if (vector.x == -1f)
        {
            rotation = Quaternion.Euler(0, 0, -90);
            this.transform.rotation = rotation;
        }
        else if (vector.y == 1f)
        {
            rotation = Quaternion.Euler(0, 0, 180);
            this.transform.rotation = rotation;
        }
        else if (vector.y == -1f)
        {
            rotation = Quaternion.Euler(0, 0,  0);
            this.transform.rotation = rotation;
        }
    }
}
