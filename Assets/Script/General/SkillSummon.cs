using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillSummon : MonoBehaviour, ISummonedObject
{
    public Material material;
    public GameObject user;
    public float attack;
    public Vector2 attackDisplaces;
    public bool isSummon;
    public float duration;
    public float durationTimeCount;
    public bool isDissolving;
    public float fade;
    
    public void Initialize(GameObject user, float damage,Vector2 attackDisplaces, float duration)
    {
        this.user = user;
        this.attack = damage;
        this.attackDisplaces = attackDisplaces;
        this.duration = duration;
    }
    private void OnEnable() {
        durationTimeCount = duration;
        fade = 1f;
        material = GetComponent<SpriteRenderer>().material;
    }
    private void Update() {
        if (isSummon){
            durationTimeCount -= Time.deltaTime;
            if(durationTimeCount <=0){
                isSummon = false;
                isDissolving = true;
            }
        }
        if(isDissolving){
            fade -= Time.deltaTime*2;
            if(fade <=0){
                fade = 1;
                isDissolving = false;
                ExitInCombat();
            }
            material.SetFloat("_Fade", fade);
        }
    }
    public void ExitInCombat(){
        transform.position = Constants.SkillObjectPoolPosition;
        durationTimeCount = duration;
    }
    
}
