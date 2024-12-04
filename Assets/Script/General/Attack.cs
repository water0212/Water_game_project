using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.Animations;

public class Attack : MonoBehaviour
{
    [Header("攻擊力")]
    public float attack;
    [Header("攻擊倍數")]
    public float attackMultiplier;
    [Header("力道")]
    public int AttackStrength;
    [Header("氣力傷害")]
    public float TenacityDamage;
    private float TenacityDamageRate;
    
    //public float attackRate; //暫時沒用
    public Vector2 attackDisplaces;
    private void OnEnable() {
        Character cc = GetComponentInParent<Character>();
        
        if(cc!=null){
        var atk = cc.attackPower;
        TenacityDamageRate = cc.TenacityDamageRate;
        attack=(float)atk*attackMultiplier;
        }
        else{
        Enemy ee = GetComponentInParent<Enemy>();
        var atk = ee.attackPower;
        attack=(float)atk*attackMultiplier;
        }
    }
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D other) {
        var character = other.GetComponent<Character>();
        if (character != null)
        {
            
            if(character.isPerfectBlock)
                PerfectReflectAttack(character, attackDisplaces,TenacityDamage);
            else if(character.isBlock)
                ReflectAttack(character, attackDisplaces,TenacityDamage);
            else {
                //character.TakeDamage(transform,attack,attackDisplaces,AttackStrength,TenacityDamage);
            }
        }
        else
        {
            var enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(transform,attack,attackDisplaces,AttackStrength,TenacityDamage,TenacityDamageRate);
            }
        }
    }
    public void ReflectAttack(Character character,Vector2 attackDisplaces, float TenacityDamage){
        var attacker = GetComponentInParent<Enemy>();
        character.ReflectEffect(attacker,attackDisplaces,TenacityDamage);
    }
    public void PerfectReflectAttack(Character character,Vector2 attackDisplaces, float TenacityDamage){
         var attacker = GetComponentInParent<Enemy>();
         character.ReflectEffect(attacker, attackDisplaces,TenacityDamage);
        // attacker.Blocked(character.CounterStunTime);
    }
}
