using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class PlayerAttackAndTakeOnDamage : AttackAndTakeOnDamage
{
    // Start is called before the first frame update
    Character cc;
    public CustomAttackAndTakeOnDamage AATD;
    public void OnEnable() {
        cc = GetComponentInParent<Character>();
        var atk = cc.attackPower;
        AATD.attackDamage=(float)atk*AATD.attackMultiplier;
    }
    public override void OnTakeDamage(Transform transform, float attack, Vector2 attackDisplaces, int AttackStrength, float TenacityDamage, float TenacityDamageRate)
    {
        if(!AATD.TakeOnDamage) return;
        base.OnTakeDamage(transform, attack, attackDisplaces, AttackStrength, TenacityDamage, TenacityDamageRate);
        if(cc.wasHited|| cc.isInvincible)
        return;
            if(cc.playerController.isHanging){
            cc.rb.gravityScale = 2.3f;
            cc.playerController.isHanging = false;
        }
            cc.TakeTenacityDamage(TenacityDamage);
            if(cc.healthPoint-attack>0){
            cc.healthPoint-=attack;
            cc.wasHited=true;
            cc.hitCD = cc.maxHitCD;
            Debug.Log("www");
            AttackScene.GetInstance().HitPause(AttackStrength);
            CamaeraControl.GetInstance().CameraShake(attackDisplaces);
            //受傷
            if(attack>0)
            cc.onTakeDamage?.Invoke(transform,attackDisplaces);
        }else{
            cc.healthPoint = 0;
            cc.DeadEvent.RaiseEvent();
            cc.isDead = true;   
            cc.gameObject.layer = 2; 
        }
        cc.onHealthChange?.Invoke(cc);
    }
    public override void TakeTenacityDamage(float TenacityDamage, float TenacityDamageRateBoost)
    {
        if(!AATD.TakeOnDamage) return;
        base.TakeTenacityDamage(TenacityDamage, TenacityDamageRateBoost);
        if(cc.tenacityPoint - TenacityDamage >0 ){
            cc.tenacityPoint -= TenacityDamage;
        }else {
            cc.tenacityPoint = 0; 
            var Damage = TenacityDamage*cc.TenacityWasDamageRate;
            //TODO:減去內功防禦
            cc.healthPoint -=Damage;
        }
    }
    public override void OnTriggerEnter2D(Collider2D other)
    {
        if(!AATD.Attacker) return;
        base.OnTriggerEnter2D(other);
        var enemyATK = other.GetComponent<AttackAndTakeOnDamage>();
         if (enemyATK != null)
        {
                enemyATK.OnTakeDamage(transform,AATD.attackDamage,AATD.attackDisplaces,AATD.AttackStrength,AATD.TenacityDamage,AATD.TenacityDamageRate);
            Debug.Log("hithit");
        }else {
            Debug.Log("nohit");
        }
    }
}
    [System.Serializable]
    public class CustomAttackAndTakeOnDamage{
        public bool Attacker = false;
        public bool TakeOnDamage = false;
        [HideInInspector] public float attackDamage;
        [HideInInspector] public float attackMultiplier;
        [HideInInspector] public int AttackStrength;
        [HideInInspector] public float TenacityDamage;
        [HideInInspector] public float TenacityDamageRate;
        [HideInInspector] public Vector2 attackDisplaces;

    }
    [CustomEditor(typeof(PlayerAttackAndTakeOnDamage))]
    public class AttackAndTakeOnDamageEditor : Editor{
        PlayerAttackAndTakeOnDamage attackAndTakeOnDamage;
        private void OnEnable(){
            attackAndTakeOnDamage = (PlayerAttackAndTakeOnDamage)target;
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
