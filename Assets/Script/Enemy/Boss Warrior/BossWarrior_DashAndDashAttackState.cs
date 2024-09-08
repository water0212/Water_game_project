using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossWarrior_DashAndDashAttackState : BaseState<BossWarriorEnemy>
{
    private float attackDelay;
    private float dashForce ;
    private bool ActiveDash;
    private bool isAttack;
    public override void OnEnter(BossWarriorEnemy Enemy)
    {
        currentEnemy = Enemy;
        currentEnemy.ChaseEnemy();
        currentEnemy.anim.SetTrigger("DashAndDashAttack");
        if(currentEnemy.firstStage){
            dashForce = 25+(currentEnemy.playerDistance_x)*0.5f;
        }else dashForce = 40+(currentEnemy.playerDistance_x)*0.6f;
        Debug.Log(dashForce+"衝刺");
        currentEnemy.wasHitedTimesCountInThisState = 0;
        attackDelay = currentEnemy.attackDelay;
        
    }
    public override void LogicUpdate()
    {
        if(!ActiveDash){
            attackDelay-= Time.deltaTime;
            if(attackDelay <= 0){
                ActiveStage();
            }    
        }
        
    }
    public override void PhysicUpdate()
    {   
        if(ActiveDash&&!isAttack){
        currentEnemy.ChaseEnemy();
        if(currentEnemy.playerDistance_x<=2){
            currentEnemy.rb.velocity = Vector2.zero;
            currentEnemy.anim.Play("Dash-Attack");
            isAttack = true;
        }
        }
        
    }
        public override void OnExit()
    {
        
    }
    private void ActiveStage(){
        ActiveDash = true;
        currentEnemy.anim.SetTrigger("Attack!");
        currentEnemy.rb.AddForce(new Vector2(dashForce*currentEnemy.faceOn.x, 0),ForceMode2D.Impulse);
    }
}
