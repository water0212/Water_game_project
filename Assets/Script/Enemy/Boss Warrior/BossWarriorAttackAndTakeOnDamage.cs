using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Accessibility;
public class BossWarriorAttackAndTakeOnDamage : AttackAndTakeOnDamage
{
    // Start is called before the first frame update
    BossWarriorEnemy enemy;
    public BossWarriorCustomAttackAndTakeOnDamage AATD;

    public void OnEnable() {
        enemy = GetComponentInParent<BossWarriorEnemy>();
        var atk = enemy.attackPower;
        AATD.attackDamage=(float)atk*AATD.attackMultiplier;
    }
    public override void OnTakeDamage(Transform transform, float attack, Vector2 attackDisplaces, int AttackStrength, float TenacityDamage, float TenacityDamageRate)
    {
        if(!AATD.TakeOnDamage) return;
        base.OnTakeDamage(transform, attack, attackDisplaces, AttackStrength, TenacityDamage, TenacityDamageRate);
        if(enemy.wasHited)return;
        TakeTenacityDamage(attack,TenacityDamage,TenacityDamageRate);
        if(enemy.healthPoint-attack>0){
            enemy.HurtEffect.RaiseEvent(enemy.transform.position+new Vector3(0,0.5f,0));
            AttackScene.GetInstance().HitPause(AttackStrength);
            CamaeraControl.GetInstance().CameraShake(attackDisplaces);
            enemy.anim.SetTrigger("Hurt");
            enemy.healthPoint-=attack-enemy.defense;
            if(enemy.healthPoint < enemy.maxHealth/2) enemy.ChangeStage();
            enemy.wasHited=true;
            enemy.isMoveRecovery = true;
            enemy.attacking= false;
            //canMove=false;
            enemy.moveRecovery = enemy.maxMoveRecovery;
            enemy.hitCD = enemy.maxHitCD;
            enemy.BossHealthChange.RaiseEvent(enemy.maxHealth,enemy.healthPoint);
            if(attack>0){
               enemy.onTakeDamage?.Invoke(transform); 
               enemy.HurtDisplacement(transform,attackDisplaces);
            }
            

        }else{
            enemy.healthPoint = 0;
            CamaeraControl.GetInstance().CameraShake(attackDisplaces);
            enemy.BossHealthChange.RaiseEvent(enemy.maxHealth,enemy.healthPoint);
            enemy.Dead();
        //    AttackScene.GetInstance().HitPause(AttackStrength+10f);
        }
        //enemy.HealthUIChange();
    }
    public override void TakeTenacityDamage(float attack,float TenacityDamage, float TenacityDamageRateBoost)
    {
        if(!AATD.TakeOnDamage) return;
        base.TakeTenacityDamage(attack, TenacityDamage, TenacityDamageRateBoost);
        if(enemy.tenacityPoint - TenacityDamage >0 ){
            enemy.tenacityPoint -= TenacityDamage;
            enemy.BossTenacityChange.RaiseEvent(enemy.maxTenacity,enemy.tenacityPoint);
            
        }else {
            //enemy.StartCoroutine(enemy.StateBarShake(0.3f , 0.2f));
            
            if(!enemy.Stuning){
            enemy.anim.SetTrigger("Stun");
            enemy.Blocked(5+TenacityDamageRateBoost);
            enemy.BossTenacityChange.RaiseEvent(enemy.maxTenacity,enemy.tenacityPoint);}
            enemy.tenacityPoint = 0; 
            var Damage = attack*TenacityDamageRateBoost;
            //TODO:減去內功防禦
            enemy.healthPoint -=Damage;
        }
       // enemy.TenacityUIChange();
    }
    public override void OnTriggerEnter2D(Collider2D other)
    {
        if(!AATD.Attacker) return;
        base.OnTriggerEnter2D(other);
        var characterATK = other.GetComponent<AttackAndTakeOnDamage>();
        var character = other.GetComponent<Character>();
        if (character != null)
        {
            if(character.isPerfectBlock&&!AATD.Unblockable)
                PerfectReflectAttack(character, AATD.attackDisplaces,AATD.TenacityDamage);
            else if(character.isBlock&&!AATD.Unblockable)
                ReflectAttack(character, AATD.attackDisplaces,AATD.TenacityDamage);
            else {
                characterATK.OnTakeDamage(transform,AATD.attackDamage,AATD.attackDisplaces,AATD.AttackStrength,AATD.TenacityDamage,1f);
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
[System.Serializable]
    public class BossWarriorCustomAttackAndTakeOnDamage{
        public bool Attacker = false;
        public bool TakeOnDamage = false;
        public bool Unblockable = false;
        [HideInInspector] public float attackDamage;
        [HideInInspector] public float attackMultiplier;
        [HideInInspector] public int AttackStrength;
        [HideInInspector] public float TenacityDamage;
        [HideInInspector] public float TenacityDamageRate;
        [HideInInspector] public Vector2 attackDisplaces;

    }
    [CustomEditor(typeof(BossWarriorAttackAndTakeOnDamage))]
    public class BossWarriorAttackAndTakeOnDamageEditor : Editor{
        BossWarriorAttackAndTakeOnDamage attackAndTakeOnDamage;
        private void OnEnable(){
            attackAndTakeOnDamage = (BossWarriorAttackAndTakeOnDamage)target;
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

