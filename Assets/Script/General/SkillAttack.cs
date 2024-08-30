using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillAttack : MonoBehaviour
{
    private SkillSummonAndDamage Summon;
    [Header("攻擊力")]
    public float attack;
    public float attackMultiplier;

    //public float attackRate; //暫時沒用
    public Vector2 attackDisplaces;
    [Header("力道")]
    public int AttackStrength;
    [Header("氣力傷害")]
    public float TenacityDamage;
    [Header("氣力倍率")]
    private float TenacityDamageRate;

    private void OnEnable(){
        Summon = GetComponentInParent<SkillSummonAndDamage>();
        AttackStrength = Summon.attackStrength;
        attackDisplaces = Summon.attackDisplaces;
        TenacityDamageRate = Summon.TenacityDamageRate;
        TenacityDamage = Summon.TenacityDamage;
    }
    private void OnTriggerEnter2D(Collider2D other) {
            var enemy = other.GetComponent<Enemy>();
            TenacityDamageRate = Summon.TenacityDamageRate;
            if (enemy != null)
            {
                enemy.TakeDamage(transform,Summon.attack*attackMultiplier,attackDisplaces,AttackStrength,TenacityDamage,TenacityDamageRate);
            }
        
    }
}
