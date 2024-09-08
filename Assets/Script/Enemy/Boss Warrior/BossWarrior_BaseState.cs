using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BossWarrior_BaseState : BaseState<BossWarriorEnemy>
{
    private int remainingDamageThreshold;
    private float TimeCount;
    private int remainingTimeThreshold;
    private float NextAttackDelay;
    public override void OnEnter(BossWarriorEnemy Enemy)
    {
        remainingDamageThreshold = (int)Math.Ceiling(5*(currentEnemy.healthPoint/currentEnemy.maxHealth));
        if(currentEnemy.firstStage){
            remainingTimeThreshold = 5;
        }else remainingTimeThreshold = 3;
        currentEnemy = Enemy;
        currentEnemy.anim.SetBool("Idle", true);
        currentEnemy.wasHitedTimesCountInThisState = 0;
        TimeCount = 0;
    }
    public override void LogicUpdate()
    {
        StateTimeCount();
        currentEnemy.ChaseEnemy();
        if(currentEnemy.wasHitedTimesCountInThisState<=remainingDamageThreshold||TimeCount >remainingTimeThreshold){
            currentEnemy.DelaySwitchState(NextAttackDelay,StateChoose());
        }
    }


    public override void PhysicUpdate()
    {

    }

    public override void OnExit()
    {
        currentEnemy.wasHitedTimesCountInThisState = 0;
        currentEnemy.anim.SetBool("Idle", false);
    }
    private void StateTimeCount(){
        TimeCount += Time.deltaTime;
    }
    private WarriorBossstate StateChoose(){
        if(SlideStateChoose()&&currentEnemy.wasHitedTimesCountInThisState<2){
            NextAttackDelay = 0.5f;
            return WarriorBossstate.SlideState;
        }
        if(currentEnemy.lastStage&&SlideStateChoose()){
            NextAttackDelay = 1f;
            return WarriorBossstate.SlideAndAttackState;
        }
        if(DashStateChoose()&&currentEnemy.wasHitedTimesCountInThisState == remainingDamageThreshold){
            NextAttackDelay = 0.7f;
            return WarriorBossstate.DashState;
        }
        if(DashAndDashAttackState()&& currentEnemy.lastStage || currentEnemy.wasHitedTimesCountInThisState< remainingDamageThreshold*0.4f){
            NextAttackDelay = 0.5f;
            return WarriorBossstate.DashAndDashAttackState;
        }
        return WarriorBossstate.BaseState;
    }
    private bool SlideStateChoose(){
        if(currentEnemy.playerDistance_y< 1){
            if(currentEnemy.playerDistance_x <10 && currentEnemy.playerDistance_x > 7)
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
