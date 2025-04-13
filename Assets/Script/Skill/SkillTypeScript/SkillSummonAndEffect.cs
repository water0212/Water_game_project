using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillSummonAndEffect : MonoBehaviour ,ISummonedAndEffectObject
{
    // Start is called before the first frame update
    public Material Fade_material;
    public Material Effect_material;

    public GameObject user;
    public GameObject Target;
    public bool isSummoned;
    public float duration;
    public float durationTimeCount;
    public bool isDissolving;
    public float fade;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D myrigidbody2D;
    public void Initialize(GameObject user,float duration)
    {
        this.user = user;
        this.duration = duration;
    }
    private void OnEnable() {
        myrigidbody2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        Effect_material = spriteRenderer.material;
        durationTimeCount = duration;
        fade = 1f;
    }
    private void Start() {
        durationTimeCount = duration;
    }
    private void Update() {
        if (isSummoned){
            durationTimeCount -= Time.deltaTime;
            if(durationTimeCount <=0){
                isSummoned = false;
                isDissolving = true;
                spriteRenderer.material = Fade_material;
            }
        }
        if(isDissolving){
            fade -= Time.deltaTime*4;
            if(fade <=0){
                fade = 1;
                isDissolving = false;
                ExitInCombat();
            }
            Fade_material.SetFloat("_Fade", fade);
        }
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.layer == LayerMask.NameToLayer("Enemy")){
            isSummoned = false;
            Target = other.gameObject;
            spriteRenderer.material = Fade_material;
            isDissolving = true ;
            myrigidbody2D.velocity = Vector3.zero;
        }else if (other.gameObject.layer == LayerMask.NameToLayer("Ground")){
            isSummoned = false;
            myrigidbody2D.velocity = Vector3.zero;
            ExitInCombat();
        }
    }
    public void ExitInCombat(){
        myrigidbody2D.velocity = Vector3.zero ;
        spriteRenderer.material = Effect_material;
        transform.position = Constants.SkillObjectPoolPosition;
        durationTimeCount = duration;
    }
    //TODO:效果函式
}
