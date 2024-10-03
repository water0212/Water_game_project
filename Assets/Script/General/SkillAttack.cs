using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class SkillAttackAndTakeOnDamage : MonoBehaviour
{
    private SkillSummonAndDamage Summon;
    public SkillCustomAttackAndTakeOnDamage AATD;
    private void OnEnable(){
        Summon = GetComponentInParent<SkillSummonAndDamage>();
        var atk = Summon.attack;
        AATD.AttackStrength = Summon.attackStrength;
        AATD.attackDisplaces = Summon.attackDisplaces;
        AATD.TenacityDamageRate = Summon.TenacityDamageRate;
        AATD.TenacityDamage = Summon.TenacityDamage;
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if(!AATD.Attacker) return;
            var enemy = other.GetComponent<AttackAndTakeOnDamage>();
            AATD.TenacityDamageRate = Summon.TenacityDamageRate;
            if (enemy != null)
            {
                enemy.OnTakeDamage(transform,Summon.attack*AATD.attackMultiplier,AATD.attackDisplaces,AATD.AttackStrength,AATD.TenacityDamage,AATD.TenacityDamageRate);
            }
        
    }
}
[System.Serializable]
    public class SkillCustomAttackAndTakeOnDamage{
        public bool Attacker = false;
        public bool TakeOnDamage = false;
        [HideInInspector] public float attackDamage;
        [HideInInspector] public float attackMultiplier;
        [HideInInspector] public int AttackStrength;
        [HideInInspector] public float TenacityDamage;
        [HideInInspector] public float TenacityDamageRate;
        [HideInInspector] public Vector2 attackDisplaces;

    }
    [CustomEditor(typeof(SkillAttackAndTakeOnDamage))]
    public class SkillAttackAndTakeOnDamageEditor : Editor{
        SkillAttackAndTakeOnDamage attackAndTakeOnDamage;
        private void OnEnable(){
            attackAndTakeOnDamage = (SkillAttackAndTakeOnDamage)target;
        }
        public override void OnInspectorGUI(){
            DrawDefaultInspector();

            if(attackAndTakeOnDamage.AATD.Attacker){
                attackAndTakeOnDamage.AATD.attackDamage = EditorGUILayout.FloatField("攻擊傷害",attackAndTakeOnDamage.AATD.attackDamage);
                attackAndTakeOnDamage.AATD.attackMultiplier = EditorGUILayout.FloatField("攻擊倍數",attackAndTakeOnDamage.AATD.attackMultiplier);
                attackAndTakeOnDamage.AATD.AttackStrength = EditorGUILayout.IntField("攻擊力道",attackAndTakeOnDamage.AATD.AttackStrength);
                attackAndTakeOnDamage.AATD.TenacityDamage = EditorGUILayout.FloatField("氣力傷害",attackAndTakeOnDamage.AATD.TenacityDamage);
                attackAndTakeOnDamage.AATD.TenacityDamageRate = EditorGUILayout.FloatField("氣力倍率",attackAndTakeOnDamage.AATD.TenacityDamageRate);
                attackAndTakeOnDamage.AATD.attackDisplaces = EditorGUILayout.Vector2Field("擊退位移",attackAndTakeOnDamage.AATD.attackDisplaces);
            }
        }
    }
