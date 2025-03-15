using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;

    public PlayerStat playerStat { get; private set; }
    public PlayerMovement PlayerMovement { get; private set; }
    public PlayerInteraction PlayerInteraction { get; private set; }

    public Location.MapNames CurrentMapName { get; private set; } //saves current map name 
    public Location.SceneNames currentSceneName { get; private set; } //current scene name


    private void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);

        PlayerInteraction = GetComponent<PlayerInteraction>();
        PlayerMovement = GetComponent<PlayerMovement>();
        playerStat = GetComponent<PlayerStat>();
    }

   
    public void ShowVisuals() {
        gameObject.SetActive(true);
    }



    #region Getter and Setter
    public void SetLocationInfo(Location.SceneNames sceneName, Location.MapNames mapName) {
        currentSceneName = sceneName;
        CurrentMapName = mapName;
    }

    public bool CheckPlayerDead() {
        return playerStat.isDead;
    }


    #endregion
    public void UseItem(int _itemID) {
        switch (_itemID) {
            case 10001: //milk
                playerStat.StaminaBuff(100);
                break;
            case 10002: //red_potion
                playerStat.StaminaBuff(10);
                break;
            case 10003://strawberry
                playerStat.Heal_Stamina(200);
                break;
            case 10005://bread
                playerStat.Heal_Stamina(300);
                break;
            default:
                break;
        }
    }
}
