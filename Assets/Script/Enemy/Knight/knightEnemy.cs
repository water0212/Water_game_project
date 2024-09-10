using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;
public class knightEnemy : Enemy
{
    [Header("速度數值")]  
    public float currentSpeed;                      //敵人_目前速度
    public float chaseSpeed;                        //敵人_追擊速度
    public float patrolSpeed;                        //敵人_巡邏速度
    public float normalSpeed;                        //敵人_一般速度
    [Header("階段狀態")]
    protected BaseState<knightEnemy> patrolState;                 //敵人_巡邏狀態
    protected BaseState<knightEnemy> chaseState;                  //敵人_追擊狀態
    protected BaseState<knightEnemy> currentState;                //敵人_目前狀態
    [Header("狀態欄")]
    private GameObject StateBar;
    private RectTransform StateBarRectTransform;
    private Image healthBar;
    private Image tenacityBar;                      
    protected override void Awake() {
        base.Awake();
        patrolState = new KnightPatrolState();
        chaseState = new knightChaseState();
        StateBar = transform.Find ("Health bar").gameObject;
        StateBarRectTransform = StateBar.GetComponent<RectTransform>();
    }
    protected override void OnEnable() {
        base.OnEnable();
        PlayerDead.OnEventRaised += OnPlayerDeadEvent;
        currentState  = patrolState;      //敵人_初始化狀態->巡邏
        currentState.OnEnter(this);  
    }
    protected override void Start(){
        base.Start();
        GameObject healthBarGO = transform.Find ("Health bar/HealthBAR").gameObject;
        if(healthBarGO == null) {Debug.Log("沒找到血量條"); return; }
        healthBar = healthBarGO.GetComponent<Image>(); 
        if(healthBar == null) {Debug.Log("沒找到血量image"); return; }
        GameObject TenacityBarGO = transform.Find ("Health bar/TenacityBAR").gameObject;
        tenacityBar = TenacityBarGO.GetComponent<Image>();
        HealthUIChange();
        TenacityUIChange();
    }
    protected override void Update() {
        base.Update();
        currentState.LogicUpdate();                     //敵人_觸發邏輯持續代碼
        if(inCombat){
            StateBar.SetActive(true);
        }else StateBar.SetActive(false);
        #region 計時器
        if(!isDead){
        HitTimeCount();
        MoveTimeCount();   
        ChaseEnemyTimeCount(); 
        StunRecover();
        }    
        #endregion
    }
    protected override void FixedUpdate() {
        currentState.PhysicUpdate();                    //敵人_觸發物理持續代碼
    }
    protected override void OnDisable()
    {
        currentState.OnExit();                          //敵人_觸發離開代碼
    }
    #region 受擊轉向

    public override void EnemyOnTakeDamage(Transform Enemytransform){
        if(Enemytransform.position.x>transform.position.x){
            transform.localScale= new Vector3(1, 1,1);
        }else{
            transform.localScale= new Vector3(-1, 1,1);
        }
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
            /// <summary>
     /// 關於轉換型態的延遲時間與類型
     /// </summary>
     /// <param name="delay">給予延遲秒數</param>
     /// <param name="nPCstate">給予轉換類型</param>
     /// <returns></returns>
    public IEnumerator DelaySwitchState(float delay,NPCstate nPCstate){
        yield return new WaitForSeconds (delay);
        SwitchState(nPCstate);
     }
      private void OnPlayerDeadEvent()
    {
        StartCoroutine(DelaySwitchState(2.0f,NPCstate.Patrol));
        canMove_playerDead = false;
    }
    #endregion  
    #region 檢測敵人
        public override bool FoundEnemy(){
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
    public override void TakeDamage(Transform transform,float attack,Vector2 attackDisplaces,int AttackStrength,float TenacityDamage,float TenacityDamageRate){
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
    public override void TakeTenacityDamage(float TenacityDamage,float TenacityDamageRateBoost){
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
    #region 死亡與清除
          protected override void Dead()
    {   
        isDead = true;
        anim.SetTrigger("Die");
        this.gameObject.layer = 2;
        ExperienceGive.RaiseEvent(ExperiencePoint);
    }
        private void Deadeffect(){
        //在動畫中表現
        //ParticleSystem deadEffect = Instantiate(DeadEffect,transform.position,quaternion.identity);
        DeadEffect.RaiseEvent(transform.position+new Vector3(0,1.5f,0));
    }
    #endregion
    #region 計時器
    public void MoveTimeCount(){
        if(moveRecovery>0||isMoveRecovery) {
            moveRecovery-=Time.deltaTime;
            if(moveRecovery<0) {
                canMove_playerDead = true;
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
            chasingTimeCount-= Time.deltaTime;
            if(chasingTimeCount<0){
                inCombat= false;
            }
        }
    } 
    public void StunRecover(){
        if(Stuning){
            stuningTimeCount-= Time.deltaTime;
            if(stuningTimeCount<0){
                Stuning = false;
                anim.SetBool("Stuning",false);
            }
        }
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
    #region 狀態欄計算與更新
    private void   HealthUIChange(){
        var persentage = healthPoint/maxHealth;
        healthBar.fillAmount = persentage; 
    }
    private void TenacityUIChange(){
        var persentage = tenacityPoint/maxTenacity;
        tenacityBar.fillAmount = persentage;
    }
    private IEnumerator StateBarShake(float duration , float ShakePower){
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
}
