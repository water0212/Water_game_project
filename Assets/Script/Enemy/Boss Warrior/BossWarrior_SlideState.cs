using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossWarrior_SlideState : BaseState<BossWarriorEnemy>
{
    private float attackDelay;
    private float SwitchStateDelay;
    private float SlideForce ;
    private bool ActiveSlide;
    private bool isAttack;
    private Vector2 playerPosition;
    private float EndSlideCount;
    public override void OnEnter(BossWarriorEnemy Enemy)
    {
        currentEnemy = Enemy;
        currentEnemy.ChaseEnemy();
        if(currentEnemy.firstStage){
            SwitchStateDelay = 3;
            SlideForce = 30+(currentEnemy.playerDistance_x)*0.5f;
            EndSlideCount = 1f;
        }else{
        SwitchStateDelay = 0.0f;
         SlideForce = 45+(currentEnemy.playerDistance_x)*0.6f;
         EndSlideCount = 0.7f;
        }   
        Debug.Log(SlideForce+"鏟擊");
         currentEnemy.wasHitedTimesCountInThisState = 0;
        attackDelay = currentEnemy.attackDelay;
    }
    public override void LogicUpdate()
    {
        if(!ActiveSlide){
            attackDelay-= Time.deltaTime;
            if(attackDelay <= 0){
                ActiveStage();
            }    
        }else{
            EndSlideCount -= Time.deltaTime;
            if(EndSlideCount <= 0){
                currentEnemy.rb.velocity = Vector2.zero;
                currentEnemy.anim.SetTrigger("SlideEnd!");
                currentEnemy.DelaySwitchState(SwitchStateDelay,StateChoose());
            }
        }
    }

    public override void PhysicUpdate()
    {
        
    }

    public override void OnExit()
    {
        currentEnemy.wasHitedTimesCountInThisState = 0;
    }
    private void ActiveStage(){
        ActiveSlide = true;
        currentEnemy.ChaseEnemy();
        playerPosition =  currentEnemy.enemyPosition;
        currentEnemy.anim.SetTrigger("Slide!");
        currentEnemy.rb.AddForce(new Vector2(SlideForce*currentEnemy.faceOn.x, 0),ForceMode2D.Impulse);
    }
    private WarriorBossstate StateChoose(){
        if(SlideAndAttackState()||currentEnemy.lastStage){
            currentEnemy.attackDelay = 0.2f;
            return WarriorBossstate.SlideAndAttackState;
        }
        currentEnemy.attackDelay = 0;
        return WarriorBossstate.BaseState;
    }

    private bool SlideAndAttackState()
    {
        if(currentEnemy.playerDistance_x>5) return true;
        else return false;
    }
}
