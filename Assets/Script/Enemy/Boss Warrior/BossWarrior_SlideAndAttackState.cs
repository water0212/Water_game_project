using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossWarrior_SlideAndAttackState : BaseState<BossWarriorEnemy>
{
    private float attackDelay;
    private float SwitchStateDelay;
    private float SlideForce ;
    private bool ActiveSlide;
    private bool isAttack;
    private Vector2 playerPosition;
    private float beginingGravityScale;
    public override void OnEnter(BossWarriorEnemy Enemy)
    {
        currentEnemy = Enemy;
        currentEnemy.ChaseEnemy();
        if(currentEnemy.firstStage){
            SwitchStateDelay = 3;
            SlideForce = 30+(currentEnemy.playerDistance_x)*0.5f;
        }else{
        SwitchStateDelay = 0.0f;
         SlideForce = 45+(currentEnemy.playerDistance_x)*0.6f;
        }   
        beginingGravityScale = currentEnemy.rb.gravityScale;
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
        }
    }

    public override void PhysicUpdate()
    {
        if(ActiveSlide)
            if(ChaseEnemy()<1.5){
                currentEnemy.rb.gravityScale = 80;
                if(currentEnemy.rb.velocity.x <1){
                currentEnemy.anim.SetTrigger("Attack!");
                currentEnemy.DelaySwitchState(SwitchStateDelay,StateChoose());

                }
            }
        
    }

    public override void OnExit()
    {
        currentEnemy.wasHitedTimesCountInThisState = 0;
        currentEnemy.rb.gravityScale = beginingGravityScale;
    }
    private float ChaseEnemy(){
        Debug.Log(playerPosition.x - currentEnemy.transform.position.x);
        return Mathf.Abs(playerPosition.x - currentEnemy.transform.position.x);
    }
    private void ActiveStage(){
        ActiveSlide = true;
        currentEnemy.ChaseEnemy();
        playerPosition =  currentEnemy.enemyPosition;
        currentEnemy.anim.SetTrigger("Slide!");
        currentEnemy.rb.AddForce(new Vector2(SlideForce*currentEnemy.faceOn.x, 0),ForceMode2D.Impulse);
    }
    private WarriorBossstate StateChoose(){
        return WarriorBossstate.Jump;
    }
}
