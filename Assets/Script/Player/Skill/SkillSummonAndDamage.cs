using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillSummonAndDamage : MonoBehaviour, ISummonedAndDamageObject
{
    private Material material;
    public GameObject user;
    public float attack;
    public Vector2 attackDisplaces;
    public bool isSummoned;
    public float duration;
    public float durationTimeCount;
    public bool isDissolving;
    public float fade;
    public int attackStrength;
    [Header("氣力傷害")]
    public float TenacityDamage;
    [HideInInspector]public float TenacityDamageRate;
    
    public void Initialize(GameObject user, float damage,Vector2 attackDisplaces, float duration,int attackStrength,float TenacityDamageRate,float TenacityDamage)
    {
        this.user = user;
        this.attack = damage;
        this.attackDisplaces = attackDisplaces;
        this.duration = duration;
        this.attackStrength = attackStrength;
        this.TenacityDamageRate = TenacityDamageRate;
        this.TenacityDamage = TenacityDamage;
    }
    private void OnEnable() {
        durationTimeCount = duration;
        fade = 1f;
        material = GetComponent<SpriteRenderer>().material;
    }
    private void Update() {
        if (isSummoned){
            durationTimeCount -= Time.deltaTime;
            if(durationTimeCount <=0){
                isSummoned = false;
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
