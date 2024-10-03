using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossWarrior_DashAndDashAttackState : BaseState<BossWarriorEnemy>
{
    private bool isExitThisState;
    private float attackDelay;

    private float dashForce ;
    private bool ActiveDash;
    private bool isAttack;
    private Vector2 playerPosition;
    private float beginingGravityScale;
    public override void OnEnter(BossWarriorEnemy Enemy)
    {
        Debug.Log("進入BossWarrior_DashAndDashAttackState");
        currentEnemy = Enemy;
        currentEnemy.ChaseEnemy();
        beginingGravityScale = currentEnemy.rb.gravityScale;
        currentEnemy.anim.SetTrigger("DashAndDashAttack");
        if(currentEnemy.firstStage){
            dashForce = 50+(currentEnemy.playerDistance_x)*0.5f;
        }else{
         dashForce = 70+(currentEnemy.playerDistance_x)*0.5f;
        }   
        currentEnemy.wasHitedTimesCountInThisState = 0;
        attackDelay = currentEnemy.attackDelay;
        isExitThisState = false ;
        isAttack = false ;
        ActiveDash = false ;
        
    }
    public override void LogicUpdate()
    {
        if(currentEnemy.canChangeState&&isExitThisState){
            currentEnemy.canChangeState = false; 
            currentEnemy.SwitchState(StateChoose());
        }
        if(isExitThisState) return;
        if(!ActiveDash){
            attackDelay-= Time.deltaTime;
            if(attackDelay <= 0){
                ActiveStage();
            }    
        }
        
    }
    public override void PhysicUpdate()
    {   
        if(isExitThisState) return;
        if(ActiveDash&&!isAttack){
            if(currentEnemy.firstStage){
                if(ChaseEnemy()<6){
                    Debug.Log("dash攻擊");
                    currentEnemy.rb.gravityScale = 25;
                    currentEnemy.anim.SetTrigger("DashAttack");
                    isAttack = true;
                    isExitThisState = true;
                }
            }else if (currentEnemy.lastStage){
                currentEnemy.ChaseEnemy();
                if(currentEnemy.playerDistance_x<7){
                    currentEnemy.rb.gravityScale = 45;
                    isAttack = true;
                    currentEnemy.anim.SetTrigger("DashAttack");
                    currentEnemy.ChaseEnemy();
                    isExitThisState = true;
                }
            }
        }
        
    }
        public override void OnExit()
    {
        attackDelay = 0;
        currentEnemy.wasHitedTimesCountInThisState = 0;
        currentEnemy.rb.gravityScale = beginingGravityScale;
         Debug.Log("離開BossWarrior_DashAndDashAttackState");
    }
    private void ActiveStage(){
        ActiveDash = true;
        playerPosition =  currentEnemy.enemyPosition;
        currentEnemy.anim.SetTrigger("Attack!");
        Debug.Log(dashForce+"衝刺");
        currentEnemy.rb.AddForce(new Vector2(dashForce*currentEnemy.faceOn.x, 0),ForceMode2D.Impulse);
    }
    private WarriorBossstate StateChoose(){
        if(FollowPlayerAndAttackStateChoose()&&currentEnemy.wasHitedTimesCountInThisState > 2){
            currentEnemy.attackDelay = 1;
            return WarriorBossstate.FollowPlayerAndAttackState;
        }
        if(JumpChoose()){
            return WarriorBossstate.Jump;
        }
        return WarriorBossstate.BaseState;
    }
    private float ChaseEnemy(){
        return Mathf.Abs(playerPosition.x - currentEnemy.transform.position.x);
    }
    private bool FollowPlayerAndAttackStateChoose(){
        if(currentEnemy.playerDistance_x<5){
            return true;
        }else return false;
    }
    private bool JumpChoose(){
        if(currentEnemy.lastStage && currentEnemy.playerDistance_x<3){
            return true;
        }else if(currentEnemy.firstStage && currentEnemy.playerDistance_y>1){
            return true;
        }else return false;
    }
}
