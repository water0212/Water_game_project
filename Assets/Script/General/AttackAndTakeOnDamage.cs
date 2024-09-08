using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class AttackAndTakeOnDamage : MonoBehaviour
{
    public CustomAttackAndTakeOnDamage customAttackAndTakeOnDamage;
    private Collider2D _coll;
    public virtual void OnTakeDamage(Transform transform,float attack,Vector2 attackDisplaces,int AttackStrength,float TenacityDamage,float TenacityDamageRate){
        
    }
    public virtual void TakeTenacityDamage(float TenacityDamage,float TenacityDamageRateBoost){

    }
}
[System.Serializable]
 public class CustomAttackAndTakeOnDamage{
    public bool Attacker = false;
    public bool TakeOnDamage = false;
    [HideInInspector] public float attackDamage;
    [HideInInspector] public float attackMultiplier;
    [HideInInspector] public float AttackStrength;
    [HideInInspector] public float TenacityDamage;
    [HideInInspector] public float TenacityDamageRate;

 }
 [CustomEditor(typeof(AttackAndTakeOnDamage))]
 public class AttackAndTakeOnDamageEditor : Editor{
    AttackAndTakeOnDamage attackAndTakeOnDamage;
    private void OnEnable(){
        attackAndTakeOnDamage = (AttackAndTakeOnDamage)target;
    }
    public override void OnInspectorGUI(){
        DrawDefaultInspector();

        if(attackAndTakeOnDamage.customAttackAndTakeOnDamage.Attacker){
            attackAndTakeOnDamage.customAttackAndTakeOnDamage.attackDamage = EditorGUILayout.FloatField("攻擊傷害",attackAndTakeOnDamage.customAttackAndTakeOnDamage.attackDamage);
            attackAndTakeOnDamage.customAttackAndTakeOnDamage.attackMultiplier = EditorGUILayout.FloatField("攻擊倍數",attackAndTakeOnDamage.customAttackAndTakeOnDamage.attackMultiplier);
            attackAndTakeOnDamage.customAttackAndTakeOnDamage.AttackStrength = EditorGUILayout.FloatField("攻擊力道",attackAndTakeOnDamage.customAttackAndTakeOnDamage.AttackStrength);
            attackAndTakeOnDamage.customAttackAndTakeOnDamage.TenacityDamage = EditorGUILayout.FloatField("氣力傷害",attackAndTakeOnDamage.customAttackAndTakeOnDamage.TenacityDamage);
            attackAndTakeOnDamage.customAttackAndTakeOnDamage.TenacityDamageRate = EditorGUILayout.FloatField("氣力倍率",attackAndTakeOnDamage.customAttackAndTakeOnDamage.TenacityDamageRate);
        }
    }
 }

