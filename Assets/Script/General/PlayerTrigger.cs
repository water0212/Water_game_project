using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    protected void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.CompareTag("Player")){
            IsFindPlayer(other.gameObject);
        }
    }

    protected virtual void IsFindPlayer(GameObject player){

    }
    protected virtual void DestoryGB(){
        Destroy(this);
    }

}
