using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class BossSummonItemAttackAndOnTakeDamage : AttackAndTakeOnDamage
{
    public BossSummonItemCustomAttackAndTakeOnDamage AATD;
    private bool wasHited;
    private float HitTimeCount;
    public float MaxHitTime;
    public Enemy master;
    public void Initialize(Enemy enemy){
        master = enemy; 
    }
    public override void OnTakeDamage(Transform transform, float attack, Vector2 attackDisplaces, int AttackStrength, float TenacityDamage, float TenacityDamageRate)
    {
        if(!AATD.TakeOnDamage) return;
        base.OnTakeDamage(transform, attack, attackDisplaces, AttackStrength, TenacityDamage, TenacityDamageRate);
        if(wasHited)return;
        if(AATD.HP - attack > 0 && AATD.canAttackCounts >0){
            AATD.HP -= attack;
            wasHited = true;
            AATD.canAttackCounts -=1;
        }else{
            AATD.canAttackCounts = 0;
            Dead();
            //特效
        }
        //enemy.HealthUIChange();
    }
    public void Dead(){
        if(AATD.canAttackCounts == 0){
            Reward();
        }
        Debug.Log("GhostDead");
        Destroy(this.gameObject);
    }
    private void Reward(){
        master.tenacityPoint -= master.maxTenacity*0.1f;
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
        var attacker = master;
        character.ReflectEffect(attacker,attackDisplaces,TenacityDamage);
    }
    public void PerfectReflectAttack(Character character,Vector2 attackDisplaces, float TenacityDamage){
         var attacker = master;
         character.ReflectEffect(attacker, attackDisplaces,TenacityDamage);
        // attacker.Blocked(character.CounterStunTime);
    }
}
[System.Serializable]
    public class BossSummonItemCustomAttackAndTakeOnDamage{
        public bool Attacker = false;
        public bool TakeOnDamage = false;
        public bool Unblockable = false;
        [HideInInspector] public float HP;
        [HideInInspector] public int canAttackCounts;
        [HideInInspector] public float attackDamage;
        [HideInInspector] public int AttackStrength;
        [HideInInspector] public float TenacityDamage;
        [HideInInspector] public float TenacityDamageRate;
        [HideInInspector] public Vector2 attackDisplaces;

    }
    [CustomEditor(typeof(BossSummonItemAttackAndOnTakeDamage))]
    public class BossSummonItemAttackAndTakeOnDamageEditor : Editor{
        BossSummonItemAttackAndOnTakeDamage attackAndTakeOnDamage;
        private void OnEnable(){
            attackAndTakeOnDamage = (BossSummonItemAttackAndOnTakeDamage)target;
        }
        public override void OnInspectorGUI(){
            DrawDefaultInspector();

            if(attackAndTakeOnDamage.AATD.Attacker){
                attackAndTakeOnDamage.AATD.attackDamage = EditorGUILayout.FloatField("攻擊傷害",attackAndTakeOnDamage.AATD.attackDamage);
                attackAndTakeOnDamage.AATD.AttackStrength = EditorGUILayout.IntField("攻擊力道",attackAndTakeOnDamage.AATD.AttackStrength);
                attackAndTakeOnDamage.AATD.TenacityDamage = EditorGUILayout.FloatField("氣力傷害",attackAndTakeOnDamage.AATD.TenacityDamage);
                attackAndTakeOnDamage.AATD.TenacityDamageRate = EditorGUILayout.FloatField("氣力倍率",attackAndTakeOnDamage.AATD.TenacityDamageRate);
                attackAndTakeOnDamage.AATD.attackDisplaces = EditorGUILayout.Vector2Field("擊退位移",attackAndTakeOnDamage.AATD.attackDisplaces);
            }
            if(attackAndTakeOnDamage.AATD.TakeOnDamage){
                attackAndTakeOnDamage.AATD.HP = EditorGUILayout.FloatField("物體血量",attackAndTakeOnDamage.AATD.HP);
                attackAndTakeOnDamage.AATD.canAttackCounts = EditorGUILayout.IntField("最大可被攻擊次數",attackAndTakeOnDamage.AATD.canAttackCounts);
            }
        }
    }