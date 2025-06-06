using System;
using System.Collections;
using System.Collections.Generic;
using Ink.Runtime;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerControler : MonoBehaviour
{
    [Header("廣播")]
    public CharacterEventSO RollingChangeEvent;
    public VoidEventSO SkillActive_E;
    public VoidEventSO SkillActive_Q;
    public VoidEventSO SkilllLoad;
    [Header("取得")]
    public Rigidbody2D rb2D;
    private CapsuleCollider2D capCo2D;
    public PlayerInputAction inputAction;
    private PhysicCheck physicCheck;
    public Vector2 inputDirection;
    public PhysicsMaterial2D jumpPhysicsState;
    public PhysicsMaterial2D normalPhysicsState;
    private Animator anim;
    private Character character;
    [Header("接收")]
    public SkillEventSO ActiveSkill;
    public VoidEventSO FinishSkill;
    [Header("基本參數")]
    public float maxSpeedX;
    //public float forceMultiplier;
    public float aceleration;
    public float deaceleration;
    public float verPower;
    //public float jumpForce;  
    [Header("連擊持續時間")]
    public float maxAttackTime;
    public float attackTimecount;
    [Header("受傷無敵")]
    //public float hurtForce;
    public float hurtTime;
    public float maxHurtTime;
    [Header("滾動力量(距離)")]
    public float RollForce;
    [Header("跳躍有關")]
    public int canJumpTimes;
    public int MaxJumpTimes;
    public float initialJumpDelay;
    public float maxJumpDelay;
    public float initialJumpForce;
    public float jumpCutForce;
    public float JumpFallGravity;
    private bool isCancelJump;
    //[SerializeField]private float jumpTimeCount;
    [SerializeField]private bool isJumping;
    public int jump;
   // public float MaxJumpRecoverTime;
   // public float JumpRecoverTime;
   [Header("原本重力")]
    public float originGravity;
    [Header("狀態")]
    public bool isRolling;
    private bool isUsingSkill;
    private bool canJump = true;
    [Tooltip("攻擊動畫開始至結束")]
    public bool isAttacking;
    public bool isHurt;
    public bool isHanging;
    public bool isFailing;
    [Header("鏡頭控制")]
    [SerializeField] private CameraFollowObject _cameraConrol;
    [SerializeField] private GameObject CameraFloowObject;
    private float _fallSpeedYDampingChangeThreshold;
    [Header("除錯")]
    public float lineDownStartOffset;
    public float lineDownEndOffset;
    public float hangOffsetx;
    public float hangOffsety;
    public int  faceOn;
    //[Header("廣播")]
    //public CharacterEventSO AttackValueEvent;
    
    void Awake() {
        inputAction = new PlayerInputAction();
        rb2D = GetComponent<Rigidbody2D>();
        physicCheck =GetComponent<PhysicCheck> ();
        capCo2D = GetComponent<CapsuleCollider2D> ();
        character =  GetComponent<Character> ();
        inputAction.GamePlayer.Block.started += Block;
        inputAction.GamePlayer.Block.canceled += Unblock;
        inputAction.GamePlayer.Jump.started += JumpCheck;
        //inputAction.GamePlayer.Jump.canceled += JumpCancel;
        inputAction.GamePlayer.NormalAttack.started += NormalAttack;
        inputAction.GamePlayer.Roll.started += Roll;
        // inputAction.GamePlayer.Skill_E.started += ActiveSkill_E;
        // inputAction.GamePlayer.Skill_Q.started += ActiveSkill_Q;
        // inputAction.GamePlayer.Skill_Load.started += LoadSkill_TEST;
        anim = GetComponent<Animator> ();
        ActiveSkill.OnEventRaised += ActiveSkillF;
        FinishSkill.OnEventRaised += FinishSkillF;
        
    }

    private void OnEnable() {
        inputAction.Enable();
        attackTimecount = maxAttackTime;
        faceOn = 1;
        canJump = true;
    }
    private void Start(){
        //inputAction.GamePlayer.Enable();
        _fallSpeedYDampingChangeThreshold = CameraManager.instence._fallSpeedYDampingChangeThreshold;
        _cameraConrol = CameraFollowObject.GetInstance();
        originGravity = rb2D.gravityScale;
    }
    private void OnDisable() {
        inputAction.Disable();
    }

    private void OnTriggerStay2D(Collider2D other) {
    //    Debug.Log(other.name + "");
    }
    void Update()
    {
        if(rb2D.velocity.y < _fallSpeedYDampingChangeThreshold && ! CameraManager.instence.IsLerpingYDamping && !CameraManager.instence.LerpedFromPlayerFalling)
        {
            CameraManager.instence.LerpYDamping(true);
        }
        if(rb2D.velocity.y >=0 && !CameraManager.instence.IsLerpingYDamping && CameraManager.instence.LerpedFromPlayerFalling){
            CameraManager.instence.LerpedFromPlayerFalling = false;
            CameraManager.instence.LerpYDamping(false);
        }
        if(DialogManager.GetInstance().dialogueIsPlaying) return;
       inputDirection = inputAction.GamePlayer.Move.ReadValue<Vector2>();
       CheckState();
       TimeCount();
       CheckPreviousIsBlock();
       LedgeCrab();
       //JumpHoldControl();
       anim.SetBool("EdgeCarb",isHanging);
      // Debug.DrawLine(Vector3.zero, Vector3.right * 10, Color.red);
    }
    #region 轉向
    private void FaceOnCheck()
    {
        if(inputDirection.x >0&& faceOn != 1){
        Turn();
        }else if(inputDirection.x <0 && faceOn == 1) {
            Turn();
        }
    }
    public void Turn(){
        if(faceOn ==1 ){
            Vector3 rotator = new Vector3(transform.rotation.x, 180f , transform.rotation.z);
            transform.rotation = Quaternion.Euler(rotator);
            faceOn = -1;
            _cameraConrol.CallTurn();
        }else{
            Vector3 rotator = new Vector3(transform.rotation.x, 0 , transform.rotation.z);
            transform.rotation = Quaternion.Euler(rotator);
            faceOn = 1;
            _cameraConrol.CallTurn();
        }
    }
    #endregion
    private void FixedUpdate() {
        if(!isHurt&&!character.isBlock&&!character.isDead&&!isRolling&&!isHanging) {
        Move();    
        }
        if(!isHanging&&!isAttacking)
        FaceOnCheck();
        JumpInitialControl();
        if(isJumping && !isCancelJump)JumpCancel();
        if(physicCheck.isGround&&rb2D.velocity.y<= 0.1 && !isHanging){
        canJumpTimes=MaxJumpTimes;   
        isJumping = false; // 停止跳躍
        
        rb2D.gravityScale = originGravity;
        }
        if(isJumping && rb2D.velocity.y<= 0.5 &&  !isHanging){
            rb2D.gravityScale = JumpFallGravity;
        }
        
        
    }
    #region 計時器

    private void TimeCount(){
        if(anim.GetBool("isAttack")){
            if(attackTimecount>0){
                attackTimecount -=Time.deltaTime;

            }else{
                attackTimecount = maxAttackTime;
                anim.SetBool("isAttack",false);
                anim.SetInteger("Combo",0);
            }
        }
        if(isHurt){
            if(hurtTime>0)
            hurtTime-=Time.deltaTime;
            else {
            hurtTime=maxHurtTime;
            anim.SetBool("isHurt",false);
            isHurt = false;
            }
        }
        if(jump >0){
            initialJumpDelay-=Time.deltaTime;
            if(initialJumpDelay < 0){jump=0;
            Debug.Log("JumpCancel");
            }
        }
    }    
    #endregion
    #region 移動
    private void Move(){
        //學習
        float currentSpeedX = rb2D.velocity.x;
        float desiredSpeedX = inputDirection.x * maxSpeedX;
        float speedDif = desiredSpeedX - currentSpeedX;
        float accelRate = (Math.Abs(desiredSpeedX) > 0.01f) ? aceleration : deaceleration ;
        float forceX = (float)Math.Pow(Math.Abs(speedDif)* accelRate, verPower) * Math.Sign(speedDif);
        float atkdeaceleration = isAttacking ? 0.05f : 1;
        rb2D.AddForce(new Vector2(forceX*atkdeaceleration, 0));
        
        
    }
    #endregion
    #region 跳躍相關
    private void JumpCheck(InputAction.CallbackContext context)
    {
        if(!canJump) return;
        if(isHanging){
            rb2D.gravityScale = originGravity;
            isHanging = false;
            canJumpTimes=MaxJumpTimes;
        }
        jump++;
        initialJumpDelay = maxJumpDelay;
        
    }
    private void JumpInitialControl(){
        if(jump>0){
            if(canJumpTimes>0&&!isRolling&&!isHanging){
                    rb2D.gravityScale=originGravity;
                    isJumping = true;
                    rb2D.velocity = new Vector2(rb2D.velocity.x, 0.2f);
                    rb2D.AddForce(transform.up*initialJumpForce, ForceMode2D.Impulse);
                    isCancelJump = false;
                    canJumpTimes--;
                    jump--;
            } 
        }
    }
    private void JumpCancel(){
        if (inputAction.GamePlayer.Jump.phase == InputActionPhase.Canceled || inputAction.GamePlayer.Jump.phase == InputActionPhase.Waiting){
        rb2D.AddForce(Vector2.down* rb2D.velocity.y*(1- jumpCutForce), ForceMode2D.Impulse);
        isCancelJump = true;
        }
    }
    #endregion
    #region "攻擊"
        
    private void NormalAttack(InputAction.CallbackContext context)
    {
        if(isRolling||DialogManager.GetInstance().dialogueIsPlaying&&isUsingSkill)
        return;
        //transform.localScale=new Vector3(1,1,1);
        anim.SetBool("isAttack", true);
        anim.SetTrigger("Attack");
        attackTimecount = maxAttackTime;
        
    }
    #endregion
    #region 翻滾
    private void Roll(InputAction.CallbackContext context)
    {
        if(isRolling||!physicCheck.isGround||character.RollTimes<=0)
        return;
        //isRolling = true;
        //character.isInvincible = true;
        RollingChangeEvent.RaiseEvent(character);
        character.RollTimes--;
        anim.SetTrigger("RollActive");
        if(inputDirection.x !=0)
        rb2D.velocity = new Vector2(inputDirection.x * RollForce, rb2D.velocity.y);
        else
        rb2D.velocity = new Vector2(faceOn * RollForce, rb2D.velocity.y);
    }
    #endregion
    private void CheckState(){
        rb2D.sharedMaterial = physicCheck.isGround?normalPhysicsState:jumpPhysicsState;
    }
    #region 攻擊時間計算
    public void AttackCombo(){
        if(anim.GetInteger("Combo") < 2){
            anim.SetInteger("Combo",anim.GetInteger("Combo")+1);
        }else{
            anim.SetInteger("Combo",0);
        }
    }    
    #endregion
    
    #region 受擊偏移
    public void HurtDisplacement(Transform attackTransform, Vector2 attackDisplaces){
        isHurt = true;
        rb2D.velocity = Vector2.zero;
        int HurtDirection;
        if(rb2D.transform.position.x - attackTransform.position.x > 0){
            HurtDirection = 1;
        }else{
            HurtDirection = -1;
        }
        Debug.Log("被打了"+HurtDirection);
        Vector2 vir = new Vector2(HurtDirection,1).normalized;
        Vector2 knockbackForce = new Vector2(vir.x * attackDisplaces.x, vir.y * attackDisplaces.y);
        rb2D.AddForce(knockbackForce,ForceMode2D.Impulse);
    }    
    #endregion
    
    #region 格檔
    private void Unblock(InputAction.CallbackContext context)
    {
        character.isBlock = false;
    }

    private void Block(InputAction.CallbackContext context)
    {   
        if(isRolling||isHanging)
        return;
        character.isBlock = true;
    }
    private void CheckPreviousIsBlock(){
        if (!character.wasBlocking && character.isBlock)
        {
            anim.SetTrigger("BlockActive");
        }
        // 更新 previousIsBlock
        character.wasBlocking = character.isBlock;
    }    
    #endregion
    
    #region 邊緣攀爬
        
        private void LedgeCrab(){
        if(rb2D.velocity.y<0&&!isHanging/*true*/){
            
            Vector2 lineDownStart = new Vector2(transform.position.x,transform.position.y) + Vector2.up*lineDownStartOffset+new Vector2(faceOn*0.35f,0);
            Vector2 lineEndStart = new Vector2(transform.position.x,transform.position.y) + Vector2.up*lineDownEndOffset+new Vector2(faceOn*0.35f,0);
            RaycastHit2D downHit2D = Physics2D.Linecast(lineDownStart, lineEndStart,LayerMask.GetMask("Ground"));
            Debug.DrawLine(lineDownStart,lineEndStart, Color.red);
            if(downHit2D.collider !=null){
                Vector2 linefwdStart = new Vector2(transform.position.x,downHit2D.point.y-0.5f);
                Vector2 lineEndfwd = new Vector2(transform.position.x,downHit2D.point.y-0.5f)+new Vector2(faceOn,0);
                RaycastHit2D fwdHit2D = Physics2D.Linecast(linefwdStart, lineEndfwd,LayerMask.GetMask("Ground"));
                  Debug.DrawLine(linefwdStart,lineEndfwd, Color.blue);
                if(fwdHit2D.collider!=null){
                    rb2D.gravityScale = 0;
                    rb2D.velocity = Vector2.zero;
                    isHanging = true;
                    Vector2 hangPos = new Vector2(fwdHit2D.point.x,fwdHit2D.point.y);
                    Vector2 hangOffest = new Vector2(hangOffsetx*-faceOn,hangOffsety);
                    hangPos+=hangOffest;
                    transform.position = hangPos;
                }
            }
        }
        
    }
    #endregion
    #region 關於技能
    private void ActiveSkillF(Skill currentSkill){
        isUsingSkill = true;
        maxSpeedX*=0.2f;
        canJump = false;

    }
    private void FinishSkillF(){
        isUsingSkill = false;
        maxSpeedX*=5f;
        canJump = true;
    }
    #endregion
    #region 事件函式
    public void EnableRolling(){
        isRolling = true;
    }
    public void DisabledRolling(){
        isRolling = false;
    }
    #endregion
}
