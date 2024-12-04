using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class Character : MonoBehaviour
{
    // Start is called before the first frame update
    public FloatEventSO ExperienceGived;
    public FloatEventSO ExperienceChange;
    public UnityEvent<Transform,Vector2> onTakeDamage;
    public UnityEvent<Character> onHealthChange;
    public UnityEvent onperfectBlock;
    private PlayerAttackAndTakeOnDamage playerATKCompoment;
    [Header("數值")]
    [Header("血量")]
    public float maxHealth;
    public float healthPoint;
    [Header("氣力")]
    public float maxTenacity;
    public float tenacityPoint;
    public float TenacityBlockRate;
    [Header("氣力倍率")]
    public float TenacityDamageRate;
    [Header("破氣後氣力被攻擊倍率")]
    public float TenacityWasDamageRate;
    [Header("被攻擊後CD")]
    public float maxHitCD;
    public float hitCD;
    [Header("攻擊力")]
    public float attackPower;
    [Header("敵人暈眩時間(韌性)")]
    public float CounterStunTime;

    public int MaxRollTimes;
    [Header("可滾動次數")]
    public int RollTimes;
    private float Rollrecovery;
    [Header("滾動冷卻時間")]
    public float MaxRollrecovery;
    [Header("被擊殺後掉落經驗值")]
    public float ExperiencePoint;
    [Header("最大經驗值")]
    public float MaxExperience;
    [Header("狀態")]
    public bool wasHited;
    public bool isDead;
    public bool isInvincible;
    public bool isBlock,wasBlocking;
    public bool isPerfectBlock;
    [HideInInspector]public PlayerControler playerController;
    [HideInInspector]public Rigidbody2D rb;
    [Header("廣播")]
    public VoidEventSO DeadEvent;
    public CharacterEventSO HealthChangeEvent;
    public FloatFloatEventSO TenacityChangeEvent;
    private void Awake() {
        playerController = GetComponent<PlayerControler>();    
        rb = GetComponent<Rigidbody2D>();
        playerATKCompoment = GetComponent<PlayerAttackAndTakeOnDamage>();
    }
    private void Start() {
        healthPoint= maxHealth;
        Rollrecovery = MaxRollrecovery;
        onHealthChange?.Invoke(this);
        ExperienceProgress(0);
        playerATKCompoment.TakeTenacityDamage(0,0,0);
    }
    private void OnEnable() {
        ExperienceGived.OnEventRaised += ExperienceProgress;
    }
    private void OnDisable() {
        ExperienceGived.OnEventRaised -= ExperienceProgress;
    }
    private void NewGame() {
        healthPoint= maxHealth;
    }
    private void Update() {
        if(wasHited){
            hitCD-=Time.deltaTime;
            if(hitCD<0){
                wasHited=false;
            }
        }
        if(tenacityPoint!= maxTenacity){
            TenacityChangeEvent.RaiseEvent(tenacityPoint,maxTenacity);
        }
        if(RollTimes<MaxRollTimes){       
            Rollrecovery-=Time.deltaTime;
            if(Rollrecovery<=0){
                RollTimes++;
                Rollrecovery = MaxRollrecovery;
            }
            playerController.RollingChangeEvent.RaiseEvent(this);
        }
    }
    #region 受傷與死亡
    /*public void TakeDamage(Transform transform,float attack,Vector2 attackDisplaces,int AttackStrength,float TenacityDamage){
            if(wasHited|| isInvincible)
        return;
            if(playerController.isHanging){
            rb.gravityScale = 2.3f;
            playerController.isHanging = false;
        }
            TakeTenacityDamage(TenacityDamage);
            if(healthPoint-attack>0){
            healthPoint-=attack;
            wasHited=true;
            hitCD = maxHitCD;
            Debug.Log("www");
            AttackScene.GetInstance().HitPause(AttackStrength);
            CamaeraControl.GetInstance().CameraShake(attackDisplaces);
            //受傷
            if(attack>0)
            onTakeDamage?.Invoke(transform,attackDisplaces);
        }else{
            healthPoint = 0;
            DeadEvent.RaiseEvent();
            isDead = true;   
            gameObject.layer = 2; 
        }
        onHealthChange?.Invoke(this);
    }
    public void TakeTenacityDamage(float TenacityDamage){
        if(tenacityPoint - TenacityDamage >0 ){
            tenacityPoint -= TenacityDamage;
        }else {
            tenacityPoint = 0; 
            var Damage = TenacityDamage*TenacityWasDamageRate;
            //TODO:減去內功防禦
            healthPoint -=Damage;
        }
    }*/
    public void ReflectEffect(Enemy attacker, Vector2 attackDisplaces, float TenacityDamage){
        var revise = attackDisplaces/2;
        attacker.healthPoint-=1;
        if(!isPerfectBlock){
        Vector2 vir = new Vector2(rb.transform.position.x - attacker.transform.position.x,1).normalized;
        var AttackStrength = vir*revise;
        playerATKCompoment.TakeTenacityDamage(0,TenacityDamage*TenacityBlockRate,0);
        rb.AddForce(AttackStrength,ForceMode2D.Impulse); 
        CamaeraControl.GetInstance().CameraShake(AttackStrength);    
        }else{
            playerATKCompoment.TakeTenacityDamage(0,TenacityDamage*0.1f,0);
            attacker.TakeTenacityDamage(TenacityDamage*(1-TenacityBlockRate),1);    
            AttackScene.GetInstance().HitPause(6);
            onperfectBlock?.Invoke();
        }
        
        Debug.Log("isblock!");
    }  
    #endregion
        private void EnableInvincible() {
        isInvincible = true;
        Debug.Log("Invincible Enabled");
    }

    private void DisableInvincible() {
        isInvincible = false;
        Debug.Log("Invincible Disabled");
    }
    private void ExperienceProgress(float ExperiencePointGived)
    {
        ExperiencePoint +=ExperiencePointGived;
        float persentage = ExperiencePoint/MaxExperience;
        ExperienceChange.RaiseEvent(persentage);
    }
}
