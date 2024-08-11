using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillAttack : MonoBehaviour
{
    [Header("攻擊力")]
    public float attack;
    public float attackMultiplier;

    //public float attackRate; //暫時沒用
    public Vector2 attackDisplaces;

    private void OnEnable(){
        var Summon = GetComponentInParent<SkillSummonAndDamage>();
        var atk = Summon.attack;
        attackDisplaces = Summon.attackDisplaces;
        if(atk>0)
        attack=(float)atk*attackMultiplier;
    }
    private void OnTriggerEnter2D(Collider2D other) {
            var enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(transform,attack,attackDisplaces);
            }
        
    }
}
