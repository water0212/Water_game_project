using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using JetBrains.Annotations;
using System;

public class DialogTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    private TextMeshPro textMeshPro;
    private SpriteRenderer textBox;
    private bool isTrigger;
    private Color Textcolor;
    private Color Boxcolor;
    public int ID;
    public float Duration ;
    private float DurationCount;
    public bool permanent;
    public bool isBoxFade;
    public float BoxFadeSpeed;
    public bool isTextFade;
    public bool closeCollider;
    public float TextFadeSpeed;
    [Header("連接其他對話框")]
    public bool isLink;
    public int LinkID;
    public FloatEventSO IDEvent;
    private void Start() {
        textMeshPro = GetComponentInChildren<TextMeshPro>();
        Textcolor = textMeshPro.color;
        Textcolor.a = 0;
        DurationCount = Duration;
        textMeshPro.color = Textcolor;
        textBox = GetComponentInChildren<SpriteRenderer>();
        Boxcolor = textBox.color;
        Boxcolor.a = 0;
        textBox.color = Boxcolor;
    }
    private void OnEnable() {
        IDEvent.OnEventRaised += OnEventRaised;
    }
    private void OnDisable() {
        IDEvent.OnEventRaised -= OnEventRaised;
    }
    private void OnEventRaised(float arg0)
    {
        if(arg0 == ID){
            isTrigger = true;
            FadeCheck();
            textMeshPro.GetComponent<TypingEffect3D>().PlayText();
            textBox.enabled = true;
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
        }
    }

    private void Update() {
        if(isTrigger&&Textcolor.a<=1&&isTextFade){
            Textcolor.a+=Time.deltaTime*TextFadeSpeed;
            textMeshPro.color = Textcolor;
        }
        if(isTrigger&&Boxcolor.a<=1&&isBoxFade){
            Boxcolor.a += Time.deltaTime*BoxFadeSpeed;
            textBox.color = Boxcolor;
        }
        if(!permanent&&isTrigger){
            DurationCount -= Time.deltaTime;
            if(DurationCount < 0){
                if(isLink) IDEvent.RaiseEvent(LinkID);
                Destroy(gameObject);
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if(closeCollider) {
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
            return;
        }
        isTrigger = true;
        FadeCheck();
        textMeshPro.GetComponent<TypingEffect3D>().PlayText();
        textBox.enabled = true;
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
    }
    private void FadeCheck(){
      if(!isTextFade){
            Textcolor.a = 1;
            textMeshPro.color = Textcolor;
        }
        if(!isBoxFade){
            Boxcolor.a = 1;
            textBox.color = Boxcolor;
        }  
    }
}
