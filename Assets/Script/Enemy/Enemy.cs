using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using UnityEngine.UI;
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
    [HideInInspector]public Vector2 faceOn;
    public UnityEvent<Transform> onTakeDamage;
    [Header("辨別敵友")]
    public Vector2 checkSize;
    public Vector2 Offset;
    public float checkDistance;
    public LayerMask enemyLayer;
    public Transform enemyTransform;
    public Vector2 enemyPosition;
    public LayerMask teamLayer;
    [Header("速度數值")]
    public float currentSpeed;                      //敵人_目前速度
    public float chaseSpeed;                        //敵人_追擊速度
    public float patrolSpeed;                        //敵人_巡邏速度
    public float normalSpeed;                       //敵人_一般速度
    [Header("身體數值")]  
    [Header("最大血量")]
    public float maxHealth;
    public float healthPoint;
    [Header("受擊冷卻")]
    public float maxHitCD;
    private float hitCD;
    [Header("攻擊力")]
    public float attackPower;
    [Header("防禦力")]
    private float defense;
    [Header("追擊時間")]
    public float chaseTime;
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
    public float ChasingTime;
    [HideInInspector]public float StuningTimeCount;
    [HideInInspector]public float attackDelayCount;

    [Header("狀態")]
    public bool wasHited;
    public bool isDead;
    public bool canMove;
    public bool attacking;
    public bool inCombat;
    public bool Stuning;
    public bool ishit;
    public bool readyToattack;
    public bool isknockback;

    [Header("階段狀態")]
    protected BaseState patrolState;                 //敵人_巡邏狀態
    protected BaseState currentState;                //敵人_目前狀態
    protected BaseState chaseState;                  //敵人_追擊狀態
    [Header("廣播")]
    public PositionEventSO HurtEffect;
    public PositionEventSO DeadEffect;
    public FloatEventSO ExperienceGive;
    [Header("接收")]
    public VoidEventSO PlayerDead;
    [Header("狀態欄")]
    [SerializeField]private GameObject StateBar;
    [SerializeField]private RectTransform StateBarRectTransform;
    [SerializeField]private Image healthBar;
    [SerializeField]private Image tenacityBar;
    protected virtual void Awake() {
        rb = GetComponent<Rigidbody2D> ();
        physicCheck = GetComponent<PhysicCheck> ();
        anim = GetComponent<Animator> ();
        StateBar = transform.Find ("Health bar").gameObject;
        StateBarRectTransform = StateBar.GetComponent<RectTransform>();
    }
    private void Start() {
        //currentSpeed = normalSpeed;                    //敵人_初始化目前速度
        GameObject healthBarGO = transform.Find ("Health bar/HealthBAR").gameObject;
        if(healthBarGO == null) {Debug.Log("沒找到血量條"); return; }
        healthBar = healthBarGO.GetComponent<Image>(); 
        if(healthBar == null) {Debug.Log("沒找到血量image"); return; }
        GameObject TenacityBarGO = transform.Find ("Health bar/TenacityBAR").gameObject;
        tenacityBar = TenacityBarGO.GetComponent<Image>();
        HealthUIChange();
        TenacityUIChange();
    }
    private void OnEnable() {
        PlayerDead.OnEventRaised += OnPlayerDeadEvent;
        canMove = true; 
        healthPoint= maxHealth;
        tenacityPoint = maxTenacity;
        
        currentState = patrolState;                     //敵人_初始化狀態->巡邏
        currentState.OnEnter(this);                     //敵人_觸發進入代碼
    }
    private void Update() {
        #region 計時器
        if(!isDead){
        HitTimeCount();
        MoveTumeCount();   
        ChaseEnemyTimeCount(); 
        StunRecover();
        }    
        #endregion
        if(inCombat){
            StateBar.SetActive(true);
        }else StateBar.SetActive(false);
        
        faceOn = new Vector2((int)transform.localScale.x,transform.localScale.y);           //敵人_面向
        currentState.LogicUpdate();                     //敵人_觸發邏輯持續代碼
    }
    private void FixedUpdate() {
        currentState.PhysicUpdate();                    //敵人_觸發物理持續代碼
    }
    private void OnDisable() {
        currentState.OnExit();                          //敵人_觸發離開代碼
    }
    #region 受傷
    public void TakeDamage(Transform transform,float attack,Vector2 attackDisplaces,int AttackStrength,float TenacityDamage,float TenacityDamageRate){
        if(wasHited)return;
        TakeTenacityDamage(TenacityDamage,TenacityDamageRate);
        if(healthPoint-attack>0){
            HurtEffect.RaiseEvent(this.transform.position+new Vector3(0,1.5f,0));
            AttackScene.GetInstance().HitPause(AttackStrength);
            CamaeraControl.GetInstance().CameraShake(attackDisplaces);
            healthPoint-=attack;
            wasHited=true;
            isMoveRecovery = true;
            attacking= false;
            //canMove=false;
            moveRecovery = maxMoveRecovery;
            hitCD = maxHitCD;
            
            if(attack>0){
               onTakeDamage?.Invoke(transform); 
               HurtDisplacement(transform,attackDisplaces);
            }
            

        }else{
            healthPoint = 0;
            CamaeraControl.GetInstance().CameraShake(attackDisplaces);
            Dead();
        //    AttackScene.GetInstance().HitPause(AttackStrength+10f);
        }
        HealthUIChange();
    }   
    public void TakeTenacityDamage(float TenacityDamage,float TenacityDamageRateBoost){
        if(tenacityPoint - TenacityDamage >0 ){
            tenacityPoint -= TenacityDamage;
            
        }else {
            StartCoroutine(StateBarShake(0.3f , 0.2f));
            tenacityPoint = 0; 
            var Damage = TenacityDamage*TenacityDamageRateBoost;
            Blocked(stunTime);
            //TODO:減去內功防禦
            healthPoint -=Damage;
        }
        TenacityUIChange();
    }
    public void HurtDisplacement(Transform attackTransform, Vector2 attackDisplaces){//受擊偏移
        rb.velocity = Vector2.zero;
        Vector2 vir = new Vector2(rb.transform.position.x - attackTransform.position.x,1).normalized;
        rb.AddForce(vir*attackDisplaces,ForceMode2D.Impulse);
    } 
    public void Blocked(float stunTimeCount){
        anim.SetBool("Stuning", true);
        attacking = false;
        readyToattack = false;
        ishit = false;
        Stuning = true;
        StuningTimeCount = stunTimeCount;
    }
    #endregion
    #region 切換狀態
    public void SwitchState(NPCstate state) 
    {
        var newState = state 
        switch 
        {
            NPCstate.Patrol => patrolState,
            NPCstate.Chase => chaseState,
            _=> null
        }; 
        currentState.OnExit();
        currentState = newState;
        currentState.OnEnter(this);
        
    }    
    #endregion
    
#region 死亡與清除
  private void Dead()
    {   
        isDead = true;
        anim.SetTrigger("Die");
        this.gameObject.layer = 2;
        canMove = false;
        ExperienceGive.RaiseEvent(ExperiencePoint);
    }
    public void DestoryGB(){
        Destroy(gameObject);
    }  
    private void Deadeffect(){
        //在動畫中表現
        //ParticleSystem deadEffect = Instantiate(DeadEffect,transform.position,quaternion.identity);
        DeadEffect.RaiseEvent(transform.position+new Vector3(0,1.5f,0));
    }
#endregion
#region 計時器
    public void MoveTumeCount(){
        if(moveRecovery>0||isMoveRecovery) {
            moveRecovery-=Time.deltaTime;
            if(moveRecovery<0) {
                canMove = true;
                isMoveRecovery = false;
            }
        }
    }
    public void HitTimeCount(){
        if(wasHited){
            hitCD-=Time.deltaTime;
            if(hitCD<0){
                wasHited=false;
            }
        }
    }   
    public void ChaseEnemyTimeCount(){
        if(inCombat){
            ChasingTime-= Time.deltaTime;
            if(ChasingTime<0){
                inCombat= false;
            }
        }
    } 
    public void StunRecover(){
        if(Stuning){
            StuningTimeCount-= Time.deltaTime;
            if(StuningTimeCount<0){
                Stuning = false;
                anim.SetBool("Stuning",false);
            }
        }
    }
    #endregion
    #region 狀態欄計算與更新
    private void   HealthUIChange(){
        var persentage = healthPoint/maxHealth;
        healthBar.fillAmount = persentage; 
    }
    private void TenacityUIChange(){
        var persentage = tenacityPoint/maxTenacity;
        tenacityBar.fillAmount = persentage;
    }
    IEnumerator StateBarShake(float duration , float ShakePower){
        Vector3 startPosition = StateBarRectTransform.anchoredPosition;
        while (duration > 0 )
        {
            StateBarRectTransform.anchoredPosition = UnityEngine.Random.insideUnitSphere* ShakePower + startPosition;
            duration-= Time.deltaTime;
            yield return null;
        }
        StateBarRectTransform.anchoredPosition = startPosition;
    }
    #endregion
    #region 檢測敵人
        public bool FoundEnemy(){
            var hit = Physics2D.BoxCast(transform.position + (Vector3)Offset,checkSize,0,new Vector2(faceOn.x,0),checkDistance,enemyLayer);
            if(hit.collider!=null&&hit.collider.CompareTag("Player")){
                enemyTransform = hit.transform;
                StartCoroutine(ChaseEnemy(chaseTime));
            }
            return hit;
        }
        private void OnDrawGizmos()//描繪圖案
    {
        Gizmos.color = UnityEngine.Color.red;
        Vector3 boxCastOrigin = transform.position + (Vector3)Offset;
        Vector3 boxCastEnd = boxCastOrigin + new Vector3(faceOn.x,0,0).normalized * checkDistance;
        Gizmos.DrawWireCube(boxCastOrigin, checkSize);
        Gizmos.DrawWireCube(boxCastEnd, checkSize);
    }
    #endregion
    #region 協程 我看不懂
        private IEnumerator ChaseEnemy(float duration)
    {
        float timer = 0f;

        while (timer < duration)
        {
            if (enemyPosition != null)
            {
                enemyPosition = enemyTransform.position; // 获取敌人位置
            }

            timer += Time.deltaTime; // 递增计时器
            yield return null; // 等待下一帧
        }
    }
    #endregion
     /// <summary>
     /// 關於轉換型態的延遲時間與類型
     /// </summary>
     /// <param name="delay">給予延遲秒數</param>
     /// <param name="nPCstate">給予轉換類型</param>
     /// <returns></returns>
     private IEnumerator DelaySwitchState(float delay,NPCstate nPCstate){
        yield return new WaitForSeconds (delay);
        SwitchState(nPCstate);
     }
     private void OnPlayerDeadEvent()
    {
        StartCoroutine(DelaySwitchState(2.0f,NPCstate.Patrol));
        canMove = false;
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
