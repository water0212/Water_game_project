using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Info : MonoBehaviour
{
    // Start is called before the first frame update
    private static Player_Info instance;
    public GameObject Player;
    private PlayerControler playerControler;
    private void Awake(){
        if(instance != null){
            Debug.LogError("Found more than one Player_Info in the scene.");
            Destroy(this);
        }
        instance = this;
    }
    private void OnEnable() {
        playerControler = Player.GetComponent<PlayerControler>();
    }
    public static Player_Info GetInstance(){
    return instance;
    }
    public static int GetPlayerFaceon(){
        return instance.playerControler.faceOn;
    }
}

