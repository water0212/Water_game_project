using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;


public class Sign : MonoBehaviour
{
    // Start is called before the first frame update
    SignSpriteF _signSpriteF;
    private void Awake() {
        _signSpriteF = GetComponent<SignSpriteF>();
    }

    private void Update()
    {
        if (Player_Info.GetPlayerFaceon() == 1)
            transform.localRotation = Quaternion.Euler(new Vector3(0,0, 0));
        else
            transform.localRotation = Quaternion.Euler(new Vector3(0,180f, 0));
    }

    //public bool canPress;

    private void OnTriggerEnter2D(Collider2D other) {
        

        GameObject otherGameobject = other.gameObject;
        _signSpriteF.OnSignEvent(otherGameobject);
    }
    private void OnTriggerExit2D(Collider2D other) {
        

        GameObject otherGameobject = other.gameObject;
        _signSpriteF.OnSignEventEnd(otherGameobject);
    }
}
