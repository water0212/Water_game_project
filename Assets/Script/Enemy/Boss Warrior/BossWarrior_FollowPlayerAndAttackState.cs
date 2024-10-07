using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossWarrior_FollowPlayerAndAttackState : BaseState<BossWarriorEnemy>
{
    private bool isExitThisState;
    private float attackDelay;
    private float MoveForce ;
    private float MoveSpeed ;
    private bool ActiveAttack;
    private bool isMoving;
    private Vector2 playerPosition;
    public override void OnEnter(BossWarriorEnemy Enemy)
    {
        Debug.Log("進入BossWarrior_FollowPlayerAndAttackState");
        currentEnemy = Enemy;
        currentEnemy.ChaseEnemy();
        if(currentEnemy.firstStage){
            MoveForce = 10;
            MoveSpeed = 10;
        }else{
            MoveForce = 10;
            MoveSpeed = 15;
        }  
        Debug.Log("開扁");
        attackDelay = currentEnemy.attackDelay;
        isExitThisState = false;
        isMoving = false;
        ActiveAttack = false;
    }
    public override void LogicUpdate()
    {
        if(currentEnemy.canChangeState&&isExitThisState){
            currentEnemy.canChangeState = false;
            currentEnemy.SwitchState(StateChoose());
        }
        if(isExitThisState) return;
        if(!ActiveAttack&&!isMoving){
            attackDelay-= Time.deltaTime;
            if(attackDelay <= 0){
                isMoving = true;
            currentEnemy.anim.SetTrigger("Move!");
            }    
        }
    }

    private void ActiveStage()
    {
        currentEnemy.ChaseEnemy();
        float currentSpeedX = currentEnemy.rb.velocity.x;
        float desiredSpeedX = currentEnemy.transform.localScale.x * MoveSpeed;
        float forceX = (desiredSpeedX - currentSpeedX) * MoveForce;
        currentEnemy.rb.AddForce(new Vector2(forceX , 0)); 
    }
    private void Attack(){
        ActiveAttack = true;
        isMoving = false;
        currentEnemy.rb.velocity = Vector2.zero;
        if(currentEnemy.firstStage){
        currentEnemy.anim.SetTrigger("RunAndAttack1Times");
        }else{
            currentEnemy.anim.SetTrigger("RunAndAttack2Times");
        }
        isExitThisState = true;
    }

    public override void PhysicUpdate()
    {
        if(isExitThisState) return;
        if(isMoving){
            ActiveStage();
            if(currentEnemy.playerDistance_x <2&& currentEnemy.playerDistance_y < 3){
                Attack();
            }
        }
    }
    public override void OnExit()
    {
        currentEnemy.wasHitedTimesCountInThisState = 0;
        Debug.Log("離開BossWarrior_FollowPlayerAndAttackState");
    }
    private WarriorBossstate StateChoose(){
        
        if(currentEnemy.wasHitedTimesCountInThisState > 0 || currentEnemy.lastStage){
            currentEnemy.attackDelay = 0.2f;
            return WarriorBossstate.Jump;
        }
        currentEnemy.attackDelay = 1;
        return WarriorBossstate.BaseState;
    }
}
