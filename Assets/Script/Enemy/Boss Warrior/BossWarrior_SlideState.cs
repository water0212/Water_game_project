using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossWarrior_SlideState : BaseState<BossWarriorEnemy>
{
    private bool isExitThisState;
    private float attackDelay;
    private float SwitchStateDelay;
    private float SlideForce ;
    private float MoveForce ;
    private float MoveSpeed ;
    private bool ActiveSlide;
    private bool isMoving;
    private float EndSlideCount;
    private float beginingGravityScale;
    public override void OnEnter(BossWarriorEnemy Enemy)
    {
        Debug.Log("進入BossWarrior_SlideState");
        currentEnemy = Enemy;
        currentEnemy.ChaseEnemy();
        if(currentEnemy.firstStage){
            attackDelay = 0.7f;
            SwitchStateDelay = 3;
            SlideForce = 20+(currentEnemy.playerDistance_x)*0.3f;
            MoveForce = 10;
            MoveSpeed = 10;
        }else{
        attackDelay = 0.3f;
        SwitchStateDelay = 0.0f;
         SlideForce = 25+(currentEnemy.playerDistance_x)*0.4f;
         MoveForce = 10;
        MoveSpeed = 15;
        }   
        beginingGravityScale = currentEnemy.rb.gravityScale;
        Debug.Log(SlideForce+"鏟擊");
         currentEnemy.wasHitedTimesCountInThisState = 0;
        isExitThisState = false;
        ActiveSlide = false;
        isMoving = false;
    }
    public override void LogicUpdate()
    {
        if(currentEnemy.canChangeState&&isExitThisState){
            currentEnemy.canChangeState = false; 
            currentEnemy.SwitchState(StateChoose());
        }
        if(isExitThisState) return;
        if(!ActiveSlide){
            attackDelay-= Time.deltaTime;
            if(attackDelay <= 0){
                ActiveStage();
            }    
        }else{
            if(Mathf.Abs(currentEnemy.rb.velocity.x) < 2){
                currentEnemy.rb.velocity = Vector2.zero;
                currentEnemy.anim.SetTrigger("SlideEnd!");
                isExitThisState = true;
            }
        }
    }

    public override void PhysicUpdate()
    {
        if(isExitThisState) return;
        if(!ActiveSlide){
            Move();
            if(!isMoving){
            currentEnemy.anim.SetTrigger("Move!");
            isMoving = true;    
            }
            
        }else{
            currentEnemy.ChaseEnemy();
            if(currentEnemy.playerDistance_x<2){
                currentEnemy.rb.gravityScale = 25;
            }
        }
    }
    private void Move(){
        currentEnemy.ChaseEnemy();
        float currentSpeedX = currentEnemy.rb.velocity.x;
        float desiredSpeedX = currentEnemy.transform.localScale.x * MoveSpeed;
        float forceX = (desiredSpeedX - currentSpeedX) * MoveForce;
        currentEnemy.rb.AddForce(new Vector2(forceX , 0)); 
    }

    public override void OnExit()
    {
        currentEnemy.wasHitedTimesCountInThisState = 0;
        currentEnemy.rb.gravityScale = beginingGravityScale;
    }
    private void ActiveStage(){
        ActiveSlide = true;
        currentEnemy.ChaseEnemy();
        currentEnemy.anim.SetTrigger("Slide!");
        currentEnemy.rb.AddForce(new Vector2(SlideForce*currentEnemy.faceOn.x, 0),ForceMode2D.Impulse);
    }
    private WarriorBossstate StateChoose(){
        if(true/*SlideAndAttackState()||currentEnemy.lastStage*/){
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
