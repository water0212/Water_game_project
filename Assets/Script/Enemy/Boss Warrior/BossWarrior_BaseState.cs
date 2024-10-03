using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BossWarrior_BaseState : BaseState<BossWarriorEnemy>
{
    private bool isExitThisState;
    private int remainingDamageThreshold;
    private float TimeCount;
    private int remainingTimeThreshold;
    public override void OnEnter(BossWarriorEnemy Enemy)
    {
        Debug.Log("進入BossWarrior_BaseState");
        currentEnemy = Enemy;
        remainingDamageThreshold = (int)Math.Ceiling(5*(currentEnemy.healthPoint/currentEnemy.maxHealth));
        if(currentEnemy.firstStage){
            remainingTimeThreshold = 3;
        }else remainingTimeThreshold = 1;
        currentEnemy.anim.SetBool("Idle", true);
        currentEnemy.wasHitedTimesCountInThisState = 0;
        TimeCount = 0;
        
    }
    public override void LogicUpdate()
    {   if(isExitThisState) return;
        StateTimeCount();
        //Debug.Log("TimeCount數道"+ TimeCount);
        currentEnemy.ChaseEnemy();
        if(currentEnemy.wasHitedTimesCountInThisState>=remainingDamageThreshold||TimeCount > remainingTimeThreshold){
            Debug.Log("TimeCount數道換不換?");
            isExitThisState = true;
            currentEnemy.StartCoroutine(currentEnemy.DelaySwitchState(0.5f,StateChoose()));
            
        }
    }


    public override void PhysicUpdate()
    {
        if(isExitThisState) return;
    }

    public override void OnExit()
    {
        currentEnemy.wasHitedTimesCountInThisState = 0;
        TimeCount = 0;
        isExitThisState = false;
        currentEnemy.anim.SetBool("Idle", false);
        Debug.Log("離開BossWarrior_BaseState");
    }
    private void StateTimeCount(){
        TimeCount += Time.deltaTime;
    }
    private WarriorBossstate StateChoose(){
        currentEnemy.ChaseEnemy();
        if(SlideStateChoose()&&currentEnemy.wasHitedTimesCountInThisState<2){
            currentEnemy.attackDelay = 1;
            return WarriorBossstate.SlideState;
        }
        if(DashAndDashAttackState()&& currentEnemy.lastStage || currentEnemy.wasHitedTimesCountInThisState< remainingDamageThreshold*0.4f){
            currentEnemy.attackDelay = 1;   
            return WarriorBossstate.DashAndDashAttackState;
        }
        if(currentEnemy.lastStage&&SlideStateChoose()){

            return WarriorBossstate.SlideAndAttackState;
        }
        if(DashStateChoose()&&currentEnemy.wasHitedTimesCountInThisState == remainingDamageThreshold){
            return WarriorBossstate.Jump;
        }
        return WarriorBossstate.BaseState;
    }
    private bool SlideStateChoose(){
        if(currentEnemy.playerDistance_y< 3){
            if(currentEnemy.playerDistance_x <35 && currentEnemy.playerDistance_x > 18)
            return true;
        }
        return false;
    }
    private bool DashStateChoose(){
        if(currentEnemy.playerDistance_x<5){
            return true;
        }
        return false;
    }
    private bool DashAndDashAttackState(){
        if(currentEnemy.playerDistance_x>10){
            return true;
        }
        return false ;
    }
}
