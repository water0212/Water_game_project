using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

using UnityEngine.Accessibility;
using UnityEngine.InputSystem.iOS;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.Events;

public class Enemy : MonoBehaviour
{

 //   public ParticleSystem HurtEffect;
 //   public ParticleSystem DeadEffect;
    [HideInInspector]public Rigidbody2D rb;
    [HideInInspector]public PhysicCheck physicCheck;
    [HideInInspector]public Animator anim;
    public Vector2 faceOn;
    public UnityEvent<Transform> onTakeDamage;
    [Header("辨別敵友")]
    public Vector2 checkSize;
    public Vector2 Offset;
    public float checkDistance;
    public LayerMask enemyLayer;
    public Transform enemyTransform;
    public Vector2 enemyPosition;
    public LayerMask teamLayer;
    [Header("身體數值")]  
    [Header("最大血量")]
    public float maxHealth;
    public float healthPoint;
    [Header("受擊冷卻")]
    public float maxHitCD;
    public float hitCD;
    [Header("攻擊力")]
    public float attackPower;
    [Header("防禦力")]
    public float defense;
    [Header("攻擊延遲")]
    public float attackDelay;
    [Header("經驗值給予")]
    public int ExperiencePoint;
    [Header("移動力")]
    public float MoveforceMultplier;
    [Header("暈眩時間")]
    public float stunTime;
        [Header("氣力")]
    public float maxTenacity;
    public float tenacityPoint;
    [Header("計時器")]
    [HideInInspector]public float moveRecovery;
    public float maxMoveRecovery;
    [Tooltip("是否正在回復移動")]
    public bool isMoveRecovery;
    [Header("追擊時間")]
    public float chaseTime;
    [HideInInspector]public float chasingTimeCount;
    [HideInInspector]public float stuningTimeCount;
    [HideInInspector]public float attackDelayCount;

    [Header("狀態")]
    [Tooltip("被打")]
    public bool wasHited;
    [Tooltip("死亡")]
    public bool isDead;
    [Tooltip("玩家掛了")]
    public bool canMove_playerDead;
    [Tooltip("正在攻擊")]
    public bool attacking;
    [Tooltip("戰鬥中(偵查到敵人了)")]
    public bool inCombat;
    [Tooltip("暈眩")]
    public bool Stuning;
    [Tooltip("打到人了")]
    public bool ishit;
    [Tooltip("準備攻擊")]
    public bool readyToattack;
    [Tooltip("被打硬直了")]
    public bool isknockback;

    [Header("廣播")]
    public PositionEventSO HurtEffect;
    public PositionEventSO DeadEffect;
    public FloatEventSO ExperienceGive;
    [Header("接收")]
    public VoidEventSO PlayerDead;

    protected virtual void Awake() {
        rb = GetComponent<Rigidbody2D> ();
        physicCheck = GetComponent<PhysicCheck> ();
        anim = GetComponent<Animator> ();
        
    }
    protected virtual void Start() {
        //currentSpeed = normalSpeed;                    //敵人_初始化目前速度
        
    }
    protected virtual void OnEnable() {
        canMove_playerDead = true; 
        healthPoint= maxHealth;
        tenacityPoint = maxTenacity;               
                   //敵人_觸發進入代碼
    }
    protected virtual void Update() {
        
        faceOn = new Vector2((int)transform.localScale.x,transform.localScale.y);           //敵人_面向

    }
    protected virtual void FixedUpdate() {
        
    }
    protected virtual void OnDisable() {
        
    }
    #region 受傷
    public virtual void TakeDamage(Transform transform,float attack,Vector2 attackDisplaces,int AttackStrength,float TenacityDamage,float TenacityDamageRate){
    }   
    public virtual void TakeTenacityDamage(float TenacityDamage,float TenacityDamageRateBoost){
    }
    public virtual void HurtDisplacement(Transform attackTransform, Vector2 attackDisplaces){//受擊偏移
        rb.velocity = Vector2.zero;
        Vector2 vir = new Vector2(rb.transform.position.x - attackTransform.position.x,1).normalized;
        rb.AddForce(vir*attackDisplaces,ForceMode2D.Impulse);
    } 
    public virtual void Blocked(float stunTimeCount){
        anim.SetBool("Stuning", true);
        attacking = false;
        readyToattack = false;
        ishit = false;
        Stuning = true;
        stuningTimeCount = stunTimeCount;
    }
    #endregion
    public virtual void Dead()
    {   
    }
    public void DestoryGB(){
        Destroy(gameObject);
    }  
        public virtual bool FoundEnemy(){
            return false;
        }
    public void CheckKnockback(){
        if(wasHited&&!physicCheck.isGround){
            isknockback = true;
        }
    }
    public void CancelKnockback(){
        isknockback = false;
    }
        public virtual void EnemyOnTakeDamage(Transform transform){}
   
}
