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
    [Header("數值")]
    public float maxHealth;
    public float healthPoint;
    public float maxMana;
    public float manaPoint;
    public float maxHitCD;
    public float hitCD;
    public float attackPower;
    public float CounterStunTime;
    public int MaxRollTimes;
    public int RollTimes;
    public float Rollrecovery;
    public float MaxRollrecovery;
    public float ExperiencePoint;
    public float MaxExperience;
    [Header("狀態")]
    public bool wasHited;
    public bool isDead;
    public bool isInvincible;
    public bool isBlock,wasBlocking;
    public bool isPerfectBlock;
    private PlayerControler playerController;
    private Rigidbody2D rb;
    [Header("廣播")]
    public VoidEventSO DeadEvent;
    public CharacterEventSO HealthChangeEvent;
    private void Awake() {
        playerController = GetComponent<PlayerControler>();    
        rb = GetComponent<Rigidbody2D>();
    }
    private void Start() {
        healthPoint= maxHealth;
        manaPoint = maxMana;
        Rollrecovery = MaxRollrecovery;
        onHealthChange?.Invoke(this);
        ExperienceProgress(0);
    }
    private void OnEnable() {
        ExperienceGived.OnEventRaised += ExperienceProgress;
    }
    private void OnDisable() {
        ExperienceGived.OnEventRaised -= ExperienceProgress;
    }
    private void NewGame() {
        healthPoint= maxHealth;
        manaPoint = maxMana;
    }
    private void Update() {
        if(wasHited){
            hitCD-=Time.deltaTime;
            if(hitCD<0){
                wasHited=false;
            }
        }
        if(RollTimes<MaxRollTimes){       
            Rollrecovery-=Time.deltaTime;
            if(Rollrecovery<=0){
                RollTimes++;
                Rollrecovery = MaxRollrecovery;
            }
            playerController.RollingChangeEvent.OnEventRaised(this);
        }
    }
    #region 受傷與死亡
    public void TakeDamage(Transform transform,float attack,Vector2 attackDisplaces,int AttackStrength){
            if(wasHited|| isInvincible)
        return;
            if(playerController.isHanging){
            rb.gravityScale = 2.3f;
            playerController.isHanging = false;
        }
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
    public void ReflectEffect(Enemy attacker, Vector2 attackDisplaces){
        var revise = attackDisplaces/2;
        attacker.healthPoint-=1;
        if(!isPerfectBlock){
        Vector2 vir = new Vector2(rb.transform.position.x - attacker.transform.position.x,1).normalized;
        var AttackStrength = vir*revise;
        rb.AddForce(AttackStrength,ForceMode2D.Impulse); 
        CamaeraControl.GetInstance().CameraShake(AttackStrength);    
        }else{
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
