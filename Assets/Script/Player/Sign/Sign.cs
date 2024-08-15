using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;


public class Sign : MonoBehaviour
{
    // Start is called before the first frame update
    SignSpriteF signSpriteF;
    private void Awake() {
        signSpriteF = GetComponent<SignSpriteF>();
        
    }


    //public bool canPress;

    private void OnTriggerEnter2D(Collider2D other) {
        

        GameObject otherGameobject = other.gameObject;
        signSpriteF.OnSignEvent(otherGameobject);
    }
    private void OnTriggerExit2D(Collider2D other) {
        

        GameObject otherGameobject = other.gameObject;
        signSpriteF.OnSignEventEnd(otherGameobject);
    }
}
