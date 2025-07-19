using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

using UnityEngine.Accessibility;
using UnityEngine.InputSystem.iOS;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.Events;
using System.Runtime.Remoting.Messaging;

public class Enemy : MonoBehaviour
{

    [Header("基本屬性")]
    [Space(5)]
    [Header("血量")]
    [SerializeField]private float maxHealth;
    [SerializeField]private float healthPoint;
    public float HealthPoint { get => healthPoint; set => healthPoint = Mathf.Clamp(value, 0, MaxHealth); }
    public float MaxHealth { 
        get => maxHealth; 
        set {
        maxHealth = Mathf.Max(0, value);  
        if (healthPoint > maxHealth)
            healthPoint = maxHealth;   
        } 
    }


    [Header("面向")]
    [SerializeField]private Vector2 faceOn;
    public Vector2 FaceOn { get => faceOn; set => faceOn = value; }

    [Header("受擊冷卻")]
    [SerializeField]private float maxHitCD;
    [SerializeField]private float hitCD;
    public float MaxHitCD { get => maxHitCD; set => maxHitCD = value; }
    public float HitCD { get => hitCD; set => hitCD = value; }

    [Header("攻擊力")]
    [SerializeField]private float attackPower;
    public float AttackPower { get => attackPower; set => attackPower = value; }

    [Header("防禦力")]
    [SerializeField]private float defense;
    public float Defense { get => defense; set => defense = value; }

    [Header("攻擊延遲")]
    [SerializeField]private float attackDelay;
    public float AttackDelay { get => attackDelay; set => attackDelay = value; }

    [Header("經驗值給予")]
    [SerializeField]private int experiencePoint;
    public int ExperiencePoint { get => experiencePoint; set => experiencePoint = value; }
    [Header("移動力")]
    public float MoveforceMultplier;
    [Header("暈眩時間")]
    public float stunTime;
        [Header("氣力")]
    public float maxTenacity;
    public float tenacityPoint;
    [Header("辨別敵友")]
    public Vector2 checkSize;
    public Vector2 Offset;
    public float checkDistance;
    public LayerMask enemyLayer;
    public Transform enemyTransform;
    public Vector2 enemyPosition;
    public LayerMask teamLayer;
    [Header("恢復移動")]
    public float maxMoveRecovery;
    [HideInInspector]public float moveRecovery;
    public bool isMoveRecovery;
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
    [Header("額外設定")]
    public bool canHurtDisplacement;

    [Header("廣播")]
    public PositionEventSO HurtEffect;
    public PositionEventSO DeadEffect;
    public FloatEventSO ExperienceGive;
    [Header("接收")]
    public VoidEventSO PlayerDead;
    [HideInInspector]public Rigidbody2D rb;
    [HideInInspector]public PhysicCheck physicCheck;
    [HideInInspector]public Animator anim;
    [Header("計時器")]
    public float stuningTimeCount;
    public float chasingTimeCount;
    public float attackDelayCount;
    public UnityEvent<Transform> onTakeDamage;

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
        HealthPoint= MaxHealth;
        tenacityPoint = maxTenacity;               
                   //敵人_觸發進入代碼
    }
    protected virtual void Update() {
        
        FaceOn = new Vector2((int)transform.localScale.x,transform.localScale.y);           //敵人_面向

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
        if(!canHurtDisplacement) return;
        rb.velocity = Vector2.zero;
        int HurtDirection;
        if(rb.transform.position.x - attackTransform.position.x > 0){
            HurtDirection = 1;
        }else{
            HurtDirection = -1;
        }
        Vector2 vir = new Vector2(HurtDirection,1).normalized;
        Vector2 knockbackForce = new Vector2(vir.x * attackDisplaces.x, vir.y * attackDisplaces.y);
        rb.AddForce(knockbackForce,ForceMode2D.Impulse);
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
        if(wasHited&&!physicCheck.IsGround){
            isknockback = true;
        }
    }
    public void CancelKnockback(){
        isknockback = false;
    }
        public virtual void EnemyOnTakeDamage(Transform transform){}
   
}
