using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossWarrior_Jump : BaseState<BossWarriorEnemy>
{
    private bool isExitThisState;
    private float attackDelay;
    private float JumpForce;
    private bool ActiveJump;
    private bool JumpEnd;
    private float beginingGravityScale;
    private Transform currentPos;
    public override void OnEnter(BossWarriorEnemy Enemy)
    {
       Debug.Log("進入BossWarrior_Jump");
       currentEnemy = Enemy;
       currentEnemy.ChaseEnemy();
       beginingGravityScale = currentEnemy.rb.gravityScale;
       if(currentEnemy.firstStage){
            JumpForce = 40;
        }else{
         JumpForce = 60;
        }   
        currentEnemy.wasHitedTimesCountInThisState = 0;
        attackDelay = currentEnemy.attackDelay;
        isExitThisState = false ;
        ActiveJump = false ;
        JumpEnd = false ;
    }
    public override void LogicUpdate()
    {
        if(currentEnemy.canChangeState&&isExitThisState){
            currentEnemy.canChangeState = false; 
            currentEnemy.SwitchState(StateChoose());
        }
         if(isExitThisState) return;
         if(!ActiveJump){
            attackDelay-= Time.deltaTime;
            if(attackDelay <= 0){
                ActiveStage();
            }    
        }else {
            currentEnemy.ChaseEnemy();
            if(JumpEnd&&currentEnemy.playerDistance_y <2){
                currentEnemy.canChangeState = true;
                currentEnemy.anim.SetTrigger("Fall");
                currentEnemy.anim.SetTrigger("JumpEnd!");
                isExitThisState = true;
            }
        }
    }
    public override void PhysicUpdate()
    {
        if(isExitThisState) return;
        if(ActiveJump&&!JumpEnd){
            if(Chase(currentPos)< 2){
                currentEnemy.anim.SetTrigger("Fall");
                currentEnemy.rb.velocity = Vector2.zero;
                currentEnemy.rb.gravityScale = 45f;
                JumpEnd = true;
            }
            if(currentEnemy.rb.velocity.y<-1){
                currentEnemy.anim.SetTrigger("Fall");
            }
            else if(currentEnemy.rb.velocity.y<2){
                currentEnemy.anim.SetTrigger("JumpToFall");
            }
        }
    }
    public override void OnExit()
    {
        currentEnemy.wasHitedTimesCountInThisState = 0;
        currentEnemy.rb.gravityScale = beginingGravityScale;
        Debug.Log("離開BossWarrior_Jump");
    }
    private WarriorBossstate StateChoose(){
        currentEnemy.ChaseEnemy();
        if(CroushAndAttackTwoTimesState()){
            currentEnemy.attackDelay = 1;
            return WarriorBossstate.CroushAndAttackTwoTimesState;
        }
        if(DashAttackChoose()||currentEnemy.lastStage){
            attackDelay = 0.5f;
            return WarriorBossstate.DashAndDashAttackState;
        }
        currentEnemy.attackDelay = 1;
        return WarriorBossstate.BaseState;
    }

    private bool DashAttackChoose()
    {
        if(currentEnemy.playerDistance_x >10){
            return true;
        }
        return false;
    }

    private bool CroushAndAttackTwoTimesState()
    {
        if(currentEnemy.playerDistance_x >10 && currentEnemy.lastStage)
        return true;
        return false;
    }

    private float Chase(Transform transform)
    {
        return Mathf.Abs(currentEnemy.transform.position.x - transform.position.x);
    }

    private void ActiveStage(){
        ActiveJump = true;
        if(Chase(currentEnemy.leftJumpPos) < Chase(currentEnemy.rightJumpPos)){
            currentEnemy.transform.localScale = new Vector3(Mathf.Abs(currentEnemy.transform.localScale.x),currentEnemy.transform.localScale.y,currentEnemy.transform.localScale.z);
            currentPos = currentEnemy.rightJumpPos;
            Debug.Log("往右跳" + Chase(currentEnemy.rightJumpPos));
        }else{
            currentEnemy.transform.localScale = new Vector3(-1*Mathf.Abs(currentEnemy.transform.localScale.x),currentEnemy.transform.localScale.y,currentEnemy.transform.localScale.z);
            currentPos = currentEnemy.leftJumpPos;
            Debug.Log("往左跳" + Chase(currentEnemy.leftJumpPos));
        }
        currentEnemy.anim.SetTrigger("QuickJump!");
        Debug.Log(JumpForce+"跳躍");
        currentEnemy.rb.AddForce(new Vector2(JumpForce*currentEnemy.transform.localScale.x, JumpForce),ForceMode2D.Impulse);
    }
}
