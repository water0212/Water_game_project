using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossWarrior_JumpAndDashAttackState : BaseState<BossWarriorEnemy>
{
    private bool isExitThisState;
    private float attackDelay;
    private float JumpForce;
    private bool ActiveJump;
    private bool ActiveJumpToAttack;
    private bool ActiveAttack;
    private float beginingGravityScale;
    public override void OnEnter(BossWarriorEnemy Enemy)
    {
       Debug.Log("進入BossWarrior_JumpAndDashAttackState");
       currentEnemy = Enemy;
       currentEnemy.ChaseEnemy();
       beginingGravityScale = currentEnemy.rb.gravityScale;
       currentEnemy.anim.SetTrigger("DashAndDashAttack");
       if(currentEnemy.firstStage){
            JumpForce = 50+(currentEnemy.playerDistance_x)*0.5f;
        }else{
         JumpForce = 70+(currentEnemy.playerDistance_x)*0.5f;
        }   
        currentEnemy.wasHitedTimesCountInThisState = 0;
        attackDelay = currentEnemy.attackDelay;
        isExitThisState = false ;
        ActiveJumpToAttack = false ;
        ActiveAttack = false ;
        ActiveJump = false ;
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
        }else if(ActiveJump&&!ActiveJumpToAttack){
            if(currentEnemy.rb.velocity.y<-1){
                currentEnemy.anim.SetTrigger("Fall");
            }
            else if(currentEnemy.rb.velocity.y<2){
                currentEnemy.anim.SetTrigger("JumpToFall");
            }
        }
    }


    public override void PhysicUpdate()
    {
        if(isExitThisState) return;
        currentEnemy.ChaseEnemy();
        if(ActiveJump&&!ActiveJumpToAttack){
            if(currentEnemy.playerDistance_x< 2){
                Debug.Log("準備Jump攻擊");
                currentEnemy.anim.SetTrigger("JumpToFall");
                currentEnemy.rb.velocity = Vector2.zero;
                currentEnemy.rb.AddForce(new Vector2(0,10f),ForceMode2D.Impulse);
                ActiveJumpToAttack = true;
                currentEnemy.anim.SetBool("JumpToAttack", true);
            }
            
        }else if (ActiveJump&&ActiveJumpToAttack&&!ActiveAttack&&currentEnemy.rb.velocity.y<0){
            if(currentEnemy.firstStage){
                currentEnemy.rb.gravityScale = 25f;
                if(currentEnemy.playerDistance_y< 2){
                    currentEnemy.anim.SetTrigger("JumpAttack");
                    ActiveAttack = true;
                    isExitThisState = true;
                    currentEnemy.anim.SetBool("JumpToAttack", false);
                }
            }else {
                currentEnemy.rb.gravityScale = 45f;
                if(currentEnemy.playerDistance_y< 2){
                    currentEnemy.anim.SetTrigger("JumpAttack");
                    ActiveAttack = true;
                    isExitThisState = true;
                    currentEnemy.anim.SetBool("JumpToAttack", false);
                }
            }
        }
    }
    public override void OnExit()
    {
        currentEnemy.wasHitedTimesCountInThisState = 0;
        currentEnemy.rb.gravityScale = beginingGravityScale;
        Debug.Log("離開BossWarrior_JumpAndDashAttackState");
    }
    private void ActiveStage(){
        ActiveJump = true;
        currentEnemy.ChaseEnemy();
        currentEnemy.anim.SetTrigger("Jump!");
        Debug.Log(JumpForce+"跳躍");
        currentEnemy.rb.AddForce(new Vector2(JumpForce*currentEnemy.faceOn.x, JumpForce),ForceMode2D.Impulse);
    }
    private WarriorBossstate StateChoose(){
        currentEnemy.ChaseEnemy();
        if(DashAttackChoose()||currentEnemy.lastStage){
            attackDelay = 0;
            return WarriorBossstate.DashAndDashAttackState;
        }
        if(FollowPlayerAndAttackStateChoose()){
            currentEnemy.attackDelay = 1;
            return WarriorBossstate.FollowPlayerAndAttackState;
        }
        return WarriorBossstate.Jump;
    }

    private bool FollowPlayerAndAttackStateChoose()
    {
        if(currentEnemy.playerDistance_x<10&&currentEnemy.playerDistance_x>4){
            return true;
        }
        return false;
    }

    private bool DashAttackChoose()
    {
        if(currentEnemy.playerDistance_x>10){
            return true;
        }
        return false ;
    }
}
