using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class Character : MonoBehaviour
{
    // Start is called before the first frame update
    public UnityEvent<Transform,float> onTakeDamage;
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
        }
    }
    #region 受傷與死亡
    public void TakeDamage(Attack attacker,float attackDisplaces){
            if(wasHited&&isInvincible)
        return;
            if(healthPoint-attacker.attack>0){
            healthPoint-=attacker.attack;
            wasHited=true;
            hitCD = maxHitCD;
            //受傷
            if(attacker.attack>0)
            onTakeDamage?.Invoke(attacker.transform,attackDisplaces);
        }else{
            DeadEvent.RaiseEvent();
            isDead = true;   
            gameObject.layer = 2; 
        }
        onHealthChange?.Invoke(this);
        
    }
    public void ReflectEffect(Enemy attacker, float attackDisplaces){
        var revise = attackDisplaces/2;
        attacker.healthPoint-=1;
        if(!isPerfectBlock){
        Vector2 vir = new Vector2(rb.transform.position.x - attacker.transform.position.x,0).normalized;
        rb.AddForce(vir*revise,ForceMode2D.Impulse);    
        }else{
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
}
