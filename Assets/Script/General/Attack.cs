using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using UnityEngine;
using UnityEngine.Animations;

public class Attack : MonoBehaviour
{
    [Header("攻擊力")]
    public float attack;
    public float attackMultiplier;
    
    //public float attackRate; //暫時沒用
    public float attackDisplaces;
    private void OnEnable() {
        var atk = GetComponentInParent<Character>()?.attackPower;
        if(atk.HasValue)
        attack=(float)atk*attackMultiplier;
        else{
         atk = GetComponentInParent<Enemy>()?.attackPower;
         attack=(float)atk*attackMultiplier;
        }
    }
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D other) {
        var character = other.GetComponent<Character>();
        if (character != null)
        {
            
            if(character.isPerfectBlock)
                PerfectReflectAttack(character, attackDisplaces);
            else if(character.isBlock)
                ReflectAttack(character, attackDisplaces);
            else character.TakeDamage(this,attackDisplaces);
        }
        else
        {
            var enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(this,attackDisplaces);
            }
        }
    }
    public void ReflectAttack(Character character,float attackDisplaces){
        var attacker = GetComponentInParent<Enemy>();
        character.ReflectEffect(attacker,attackDisplaces);
    }
    public void PerfectReflectAttack(Character character, float attackDisplaces){
         var attacker = GetComponentInParent<Enemy>();
         character.ReflectEffect(attacker, attackDisplaces);
         attacker.Blocked(character.CounterStunTime);
    }
}
