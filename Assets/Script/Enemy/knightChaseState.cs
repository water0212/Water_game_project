using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using UnityEditor.Callbacks;
using UnityEngine;

public class knightChaseState : BaseState
{
    [Header("判斷敵人")]
    public Vector2 checkSize = new Vector2(0.5f,1f);
    public Vector2 Offset= new Vector2 (0.1f,0.76f);
    public float checkDistance = 1.2f;
    [Header("狀態")]
    public float chaseFaceOn;


    public override void OnEnter(Enemy Enemy)
    {
        currentEnemy = Enemy;
        currentEnemy.attackDelayCount = currentEnemy.attackDelay;
        currentEnemy.currentSpeed = currentEnemy.chaseSpeed;
        currentEnemy.anim.SetBool("Chasing",true);
        
    }
    public override void LogicUpdate()
    {
        if(currentEnemy.FoundEnemy()){
            currentEnemy.ChasingTime = currentEnemy.chaseTime ;
        }
        if(!currentEnemy.inCombat){
            currentEnemy.SwitchState(NPCstate.Patrol);
        }
        if(!currentEnemy.readyToattack&&FoundEnemyAndAttack()&&!currentEnemy.Stuning) currentEnemy.readyToattack = true;
        if(currentEnemy.readyToattack&&!currentEnemy.Stuning&&!currentEnemy.isDead){
            AttackDelayCount();
        }
    }
    public override void PhysicUpdate()
    {
       if(currentEnemy.physicCheck.isGround&&currentEnemy.physicCheck.isOnTheFloor&&currentEnemy.canMove&&!currentEnemy.attacking&&!currentEnemy.Stuning&&!currentEnemy.readyToattack&&!currentEnemy.wasHited) ChaseMove();
       else if(!currentEnemy.physicCheck.isOnTheFloor&&currentEnemy.physicCheck.isGround) currentEnemy.rb.velocity =Vector2.zero;
       //if(!currentEnemy.Stuning&&!currentEnemy.attacking)CheckAndFaceOn();
        if(currentEnemy.physicCheck.touchWall)Jump();
    }

    

    private void Jump()
    {
        //TODO:當遇到阻礙時可跳躍
    }

    private void ChaseMove()
    {
        if(!currentEnemy.attacking)CheckAndFaceOn();
        float currentSpeedX = currentEnemy.rb.velocity.x;
        float desiredSpeedX = chaseFaceOn * currentEnemy.currentSpeed;
        float forceX = (desiredSpeedX - currentSpeedX) * currentEnemy.MoveforceMultplier;
        currentEnemy.rb.AddForce(new Vector2(forceX , 0)); //武士_在地上時移動
        
    }
    public void CheckAndFaceOn(){
        if(currentEnemy.enemyPosition.x - currentEnemy.transform.position.x>0){
                chaseFaceOn=1;
            }else chaseFaceOn = -1;
            currentEnemy.transform.localScale = new Vector3 (chaseFaceOn, 1,1);
    }

    public override void OnExit()
    {
       currentEnemy.anim.SetBool("Chasing",false);
        currentEnemy.transform.localScale = new Vector3 (-chaseFaceOn, 1,1);
       currentEnemy.inCombat = false;
    }
    public bool FoundEnemyAndAttack(){
        
            var hit1 = Physics2D.BoxCast(currentEnemy.transform.position + (Vector3)Offset,checkSize,0,new Vector2(chaseFaceOn,0),checkDistance,currentEnemy.enemyLayer);
            
            if(hit1.collider!=null&&hit1.collider.CompareTag("Player")&&!currentEnemy.attacking){
                Debug.Log(hit1);
                if(currentEnemy.physicCheck.isGround)currentEnemy.rb.velocity = currentEnemy.rb.velocity * new Vector2(0,1);
               return true;
            }
            else return false;
            
            
        }
    public void AttackDelayCount(){
    if(currentEnemy.attackDelayCount>0){
            currentEnemy.attackDelayCount-= Time.deltaTime;
            
        }else if(currentEnemy.attackDelayCount<0){
                if(!currentEnemy.ishit){
                    currentEnemy.anim.Play("Attack1");
                    currentEnemy.attackDelayCount = currentEnemy.attackDelay;
                    currentEnemy.ishit = true;
                    currentEnemy.readyToattack = false;
                }else if(currentEnemy.ishit){
                    currentEnemy.anim.Play("Attack2");
                    currentEnemy.attackDelayCount = currentEnemy.attackDelay;
                    currentEnemy.ishit = false;
                    currentEnemy.readyToattack = false;
                } 
            }
    }
}
