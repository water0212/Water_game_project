using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class SkillAttackAndTakeOnDamage : MonoBehaviour
{
    private IDamageProvider Summon;
    public SkillCustomAttackAndTakeOnDamage AATD;
    private void OnEnable(){
        Summon = GetComponentInParent<IDamageProvider>();
        if(!AATD.CustomAttackDamage)
        AATD.attackDamage = Summon.attackDamage;
        if(!AATD.CustomAttackMultiplier)
        AATD.attackMultiplier = Summon.attackMultiplier;
        if(!AATD.CustomAttackStrength)
        AATD.AttackStrength = Summon.attackStrength;
        if(!AATD.CustomTenacityDamage)
        AATD.TenacityDamage = Summon.TenacityDamage;
        if(!AATD.CustomTenacityDamageRate)
        AATD.TenacityDamageRate = Summon.TenacityDamageRate;
        if(!AATD.CustomAttackDisplaces)
        AATD.attackDisplaces = Summon.attackDisplaces;
        
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if(!AATD.Attacker) return;
            var enemy = other.GetComponent<AttackAndTakeOnDamage>();
            if(enemy.tag == "Player") Debug.Log("打到玩家拉");
            AATD.TenacityDamageRate = Summon.TenacityDamageRate;
            if (enemy != null && enemy.tag != "Player")
            {
                enemy.OnTakeDamage(transform,AATD.attackDamage*AATD.attackMultiplier,AATD.attackDisplaces,AATD.AttackStrength,AATD.TenacityDamage,AATD.TenacityDamageRate);
            }
        
    }
}
[System.Serializable]
    public class SkillCustomAttackAndTakeOnDamage{
        public bool Attacker = true;
        public bool TakeOnDamage = false;
        public bool CustomDamageInfo = false;
        [HideInInspector] public float attackDamage;
        [HideInInspector] public float attackMultiplier;
        [HideInInspector] public int AttackStrength;
        [HideInInspector] public float TenacityDamage;
        [HideInInspector] public float TenacityDamageRate;
        [HideInInspector] public Vector2 attackDisplaces;
        [HideInInspector] public bool CustomAttackDamage;
        [HideInInspector] public bool CustomAttackMultiplier;
        [HideInInspector] public bool CustomAttackStrength;
        [HideInInspector] public bool CustomTenacityDamage;
        [HideInInspector] public bool CustomTenacityDamageRate;
        [HideInInspector] public bool CustomAttackDisplaces;
    }
    [CustomEditor(typeof(SkillAttackAndTakeOnDamage))]
    public class SkillAttackAndTakeOnDamageEditor : Editor{
        SkillAttackAndTakeOnDamage attackAndTakeOnDamage;
        private void OnEnable(){
            attackAndTakeOnDamage = (SkillAttackAndTakeOnDamage)target;
        }
        public override void OnInspectorGUI(){
            DrawDefaultInspector();
            var AATDInfo = attackAndTakeOnDamage.AATD;
            if(AATDInfo.Attacker && AATDInfo.CustomDamageInfo){
                AATDInfo.CustomAttackDamage = EditorGUILayout.Toggle("自訂義攻擊傷害",AATDInfo.CustomAttackDamage);
                AATDInfo.CustomAttackMultiplier = EditorGUILayout.Toggle("自訂義攻擊傷害倍率",AATDInfo.CustomAttackMultiplier);
                AATDInfo.CustomAttackStrength = EditorGUILayout.Toggle("自訂義攻擊力道",AATDInfo.CustomAttackStrength);
                AATDInfo.CustomTenacityDamage = EditorGUILayout.Toggle("自訂義氣力傷害",AATDInfo.CustomTenacityDamage);
                AATDInfo.CustomTenacityDamageRate = EditorGUILayout.Toggle("自訂義氣力傷害倍率",AATDInfo.CustomTenacityDamageRate);
                AATDInfo.CustomAttackDisplaces = EditorGUILayout.Toggle("自訂義擊退位移",AATDInfo.CustomAttackDisplaces);
                if(AATDInfo.CustomAttackDamage)
                AATDInfo.attackDamage = EditorGUILayout.FloatField("攻擊傷害(自訂義)",AATDInfo.attackDamage);
                if(AATDInfo.CustomAttackMultiplier)
                AATDInfo.attackMultiplier = EditorGUILayout.FloatField("攻擊倍數(自訂義)",AATDInfo.attackMultiplier);
                if(AATDInfo.CustomAttackStrength)
                AATDInfo.AttackStrength = EditorGUILayout.IntField("攻擊力道(自訂義)",AATDInfo.AttackStrength);
                if(AATDInfo.CustomTenacityDamage)
                AATDInfo.TenacityDamage = EditorGUILayout.FloatField("氣力傷害(自訂義)",AATDInfo.TenacityDamage);
                if(AATDInfo.CustomTenacityDamageRate)
                AATDInfo.TenacityDamageRate = EditorGUILayout.FloatField("氣力倍率(自訂義)",AATDInfo.TenacityDamageRate);
                if(AATDInfo.CustomAttackDisplaces)         
                AATDInfo.attackDisplaces = EditorGUILayout.Vector2Field("擊退位移(自訂義)",AATDInfo.attackDisplaces);
            }
        }
    }
