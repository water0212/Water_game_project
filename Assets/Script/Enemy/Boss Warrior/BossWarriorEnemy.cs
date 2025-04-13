using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class BossWarriorEnemy : Enemy
{
    [Header("Boss特殊")]
    public int ChangeTimes;
    private bool bossStart;
    public GameObject Player;
    public GameObject Ghost;
    public float playerDistance_y;
    public float playerDistance_x;
    public bool canChangeState;
    public float lastDistance;
    public Transform leftJumpPos;
    public Transform rightJumpPos;
    public bool canDisslove;
    public TextMeshProUGUI DebugLog;
    public GameObject Soul;
    [Tooltip("被打的次數")]
    public int wasHitedTimesCountInCombat;
    public int wasHitedTimesCountInThisState;
    [Header("兩個階段")]
    public bool firstStage;
    public bool lastStage;
     [Header("速度數值")]  
    //  public float currnetSpeed;
    //  public float firstStageSpeed;
    //  public float lastStageSpeed;
     [Header("階段狀態")]
     public BaseState<BossWarriorEnemy> currentState; 
     public BaseState<BossWarriorEnemy> dashAndDashAttackState; 
     public BaseState<BossWarriorEnemy> dashState;
     public BaseState<BossWarriorEnemy> jumpAndDashAttackState;
     public BaseState<BossWarriorEnemy> croushAndAttackTwoTimesState;
     public BaseState<BossWarriorEnemy> followPlayerAndAttackState;
     public BaseState<BossWarriorEnemy> slideAndAttackState;
     public BaseState<BossWarriorEnemy> slideState;
     public BaseState<BossWarriorEnemy> baseState;
     public BaseState<BossWarriorEnemy> playerDeadState;
     public BaseState<BossWarriorEnemy> jumpState;
     public BaseState<BossWarriorEnemy> BossDeadState;
     public BaseState<BossWarriorEnemy> BossStuningState;
    //[Header("狀態欄")] TODO:boss血條

    [Header("接收")]
    public VoidEventSO BossStartEvent;
    [Header("廣播")]
    public FloatFloatEventSO BossHealthChange;
    public FloatFloatEventSO BossTenacityChange;
    public VoidEventSO BossDead;

    public void BossStart(){
        Debug.Log("關主啟動");
        currentState = baseState;
        enemyTransform = Player.transform;
        bossStart = true;
        currentState.OnEnter(this);
    }
    protected override void Awake() {
        base.Awake();
        baseState = new BossWarrior_BaseState();
        dashAndDashAttackState = new BossWarrior_DashAndDashAttackState();
        slideState = new BossWarrior_SlideState();
        followPlayerAndAttackState = new BossWarrior_FollowPlayerAndAttackState();
        slideAndAttackState = new BossWarrior_SlideAndAttackState();
        jumpAndDashAttackState = new BossWarrior_JumpAndDashAttackState();
        jumpState = new BossWarrior_Jump();
        croushAndAttackTwoTimesState = new BossWarrior_CroushAndAttackTwoTimesState();
        BossDeadState = new BossWarriorBossDeadState();
        BossStuningState = new BossWarrior_StuningState();
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        PlayerDead.OnEventRaised += OnPlayerDeadEvent;
        BossStartEvent.OnEventRaised += BossStart;
        
    }
    protected override void Update() {
        if(!bossStart)return;
        base.Update();
        currentState.LogicUpdate();
        if(!isDead){
             HitTimeCount();
             StunRecover(); 
        }
    }
    protected override void FixedUpdate(){ 
        if(!bossStart)return;
        base.FixedUpdate(); 
        currentState.PhysicUpdate();
    }
    protected override void OnDisable()
    {
        //currentState.OnExit();
        PlayerDead.OnEventRaised -= OnPlayerDeadEvent;
        BossStartEvent.OnEventRaised -= BossStart;
    }
    #region 切換狀態
    public void SwitchState(WarriorBossstate state) 
    {
        ChangeTimes++;
        var newState = state 
        switch 
        {
            WarriorBossstate.DashAndDashAttackState => dashAndDashAttackState,
            WarriorBossstate.JumpAndDashAttackState => jumpAndDashAttackState,
            WarriorBossstate.Jump => jumpState,
            WarriorBossstate.CroushAndAttackTwoTimesState => croushAndAttackTwoTimesState,
            WarriorBossstate.FollowPlayerAndAttackState => followPlayerAndAttackState,
            WarriorBossstate.SlideAndAttackState => slideAndAttackState,
            WarriorBossstate.SlideState => slideState,
            WarriorBossstate.BaseState => baseState,
            WarriorBossstate.PlayerDeadState => playerDeadState,
            WarriorBossstate.BossDeadState => BossDeadState,
            WarriorBossstate.BossStuningState => BossStuningState,
            _=> null
        }; 
        Debug.Log("離開");
        currentState.OnExit();
        currentState = newState;
        currentState.OnEnter(this);
        Debug.Log("進入"); 
    } 
    /// <summary>
     /// 關於轉換型態的延遲時間與類型
     /// </summary>
     /// <param name="delay">給予延遲秒數</param>
     /// <param name="nPCstate">給予轉換類型</param>
     /// <returns></returns>
    public IEnumerator DelaySwitchState(float delay,WarriorBossstate State){
        yield return new WaitForSeconds (delay);
        SwitchState(State);
     }
    #endregion
    #region 玩家死亡
    private void OnPlayerDeadEvent()
    {
        StartCoroutine(DelaySwitchState(2.0f,WarriorBossstate.PlayerDeadState));
        canMove_playerDead = false;
    }
    #endregion
    #region 受到傷害後是否轉變型態
        
    // public override void TakeDamage(Transform transform,float attack,Vector2 attackDisplaces,int AttackStrength,float TenacityDamage,float TenacityDamageRate){
    //     if(wasHited)return;
    //     TakeTenacityDamage(TenacityDamage,TenacityDamageRate);
    //     if(healthPoint-attack>0){
    //         HurtEffect.RaiseEvent(this.transform.position+new Vector3(0,1.5f,0));
    //         AttackScene.GetInstance().HitPause(AttackStrength);
    //         CamaeraControl.GetInstance().CameraShake(attackDisplaces);
    //         healthPoint-=attack;
    //         wasHitedTimesCountInCombat++;
    //         wasHitedTimesCountInThisState++;
    //         wasHited=true;
    //         hitCD = maxHitCD;
    //         BossHealthChange.RaiseEvent(maxHealth,healthPoint);
    //         if(attack>0){
    //            onTakeDamage?.Invoke(transform); 
    //            HurtDisplacement(transform,attackDisplaces);
    //         }
            

    //     }else{
    //         healthPoint = 0;
    //         CamaeraControl.GetInstance().CameraShake(attackDisplaces);
    //         Dead();
    //     //    AttackScene.GetInstance().HitPause(AttackStrength+10f);
    //     }
    // }
    // public override void TakeTenacityDamage(float TenacityDamage,float TenacityDamageRateBoost){
    //     if(tenacityPoint - TenacityDamage >0 ){
    //         tenacityPoint -= TenacityDamage;
            
    //     }else {
    //         tenacityPoint = 0; 
    //         var Damage = TenacityDamage*TenacityDamageRateBoost;
    //         Blocked(stunTime);
    //         //TODO:減去內功防禦
    //         healthPoint -=Damage;
    //     }
    // }
    public void ChangeStage(){
        firstStage = false;
        lastStage = true;
        
    }
    #endregion
    
    public override void Dead()
    {   
        isDead = true;
        SwitchState(WarriorBossstate.BossDeadState);
        this.gameObject.layer = 2;
        ExperienceGive.RaiseEvent(ExperiencePoint);
    }
    #region 暈眩
        public override void Blocked(float stunTimeCount){
        anim.SetBool("Stuning", true);
        attacking = false;
        SwitchState(WarriorBossstate.BossStuningState);
        ishit = false;
        Stuning = true;
        stunTime = stunTimeCount /*TODO:這邊可以乘上暈眩係數*/;
        stuningTimeCount = stunTime;
    }
    #endregion
    public void CanDisslove(){
        canDisslove = true;
    }
    #region 計時器
    public void HitTimeCount(){
        if(wasHited){
            hitCD-=Time.deltaTime;
            if(hitCD<0){
                wasHited=false;
            }
        }
    }  
    public void StunRecover(){
        if(Stuning){
            stuningTimeCount-= Time.deltaTime;
            BossTenacityChange.RaiseEvent(stunTime,stunTime - stuningTimeCount);
            if(stuningTimeCount<0){
                Stuning = false;
                anim.SetBool("Stuning",false);
                SwitchState(WarriorBossstate.BaseState);
                tenacityPoint = maxTenacity;
                BossTenacityChange.RaiseEvent(maxTenacity,tenacityPoint);
            }
        }
    }
    #endregion
    #region 追蹤敵人
    public void ChaseEnemy(bool faceOnLock = false)
    {
    enemyPosition = enemyTransform.position; // 获取敌人位置
    playerDistance_y = Mathf.Abs(enemyPosition.y-transform.position.y) ;
    playerDistance_x = Mathf.Abs(enemyPosition.x-transform.position.x) ;
    if(enemyPosition.x-transform.position.x >=0.2 && !faceOnLock){
        Debug.Log("轉頭");
        //Debug.Log("TargetFunction called", this);
        transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x),transform.localScale.y,transform.localScale.z);
    }else if(enemyPosition.x-transform.position.x<=0.2 && !faceOnLock){
        Debug.Log("轉頭");
        //Debug.Log("TargetFunction called", this);
        transform.localScale = new Vector3(-1*Mathf.Abs(transform.localScale.x),transform.localScale.y,transform.localScale.z);
    }
    }
    public bool IFAwayTarger()
{
    // 計算當前距離
    float currentDistance = Mathf.Abs(enemyPosition.x-transform.position.x);

    // 定義 Lambda 表達式來檢查方向
    int enemyInfront = (enemyPosition.x - transform.position.x) > 0? 2 : -2;

    // 檢查距離和方向
    if (currentDistance > lastDistance && faceOn.x != enemyInfront)
    {
        Debug.LogWarning("正在遠離");
        lastDistance = currentDistance;
        return true;
    }
    lastDistance = currentDistance;
    return false;
}
    #endregion
    public void CanChangeBossState(){
        canChangeState = true;
    }
}

public enum WarriorBossstate{
    DashAndDashAttackState,
    JumpAndDashAttackState,
    Jump,
    CroushAndAttackTwoTimesState,
    FollowPlayerAndAttackState,
    SlideAndAttackState,
    SlideState,
    BaseState,
    PlayerDeadState,
    BossDeadState,
    BossChangeState,
    BossStuningState
}
