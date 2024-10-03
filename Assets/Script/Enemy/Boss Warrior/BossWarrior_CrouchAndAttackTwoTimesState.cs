using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BossWarrior_CroushAndAttackTwoTimesState : BaseState<BossWarriorEnemy>
{
    private bool isExitThisState;
    private float attackDelay;
    private bool isGhostLive;

    GameObject summonedOB;
    private float dashForce ;
    private bool ActiveDash;
    private int DashTimes;
    private Vector3 orignalPos;
    private float beginingGravityScale;
    public override void OnEnter(BossWarriorEnemy enemy){
        Debug.Log("進入BossWarrior_CroushAndAttackTwoTimesState");
        currentEnemy = enemy;
        currentEnemy.ChaseEnemy();
        beginingGravityScale = currentEnemy.rb.gravityScale;
        currentEnemy.anim.SetTrigger("DashAndDashAttack");
        if(currentEnemy.firstStage){
            dashForce = 50+(currentEnemy.playerDistance_x)*0.5f;
            DashTimes = 1;
        }else{
            dashForce = 70+(currentEnemy.playerDistance_x)*0.5f;
            DashTimes = 3;
        }   
        currentEnemy.wasHitedTimesCountInThisState = 0;
        attackDelay = currentEnemy.attackDelay;
        isExitThisState = false ;
        ActiveDash = false ;
    }
    public override void LogicUpdate(){
        if(currentEnemy.canChangeState&&isExitThisState) {
            currentEnemy.ChaseEnemy();
            if (DashTimes>0 && currentEnemy.playerDistance_x >8){
                currentEnemy.canChangeState = false;
                currentEnemy.SwitchState(WarriorBossstate.FollowPlayerAndAttackState);
            }else{
            currentEnemy.canChangeState = false; 
            currentEnemy.SwitchState(StateChoose());
            }
        }
        
        if(isGhostLive){
        CheckGhost() ;
        }
        if(isExitThisState) return;
        if(!ActiveDash){
            attackDelay-= Time.deltaTime;
            if(attackDelay <= 0){
                ActiveStage();
            }    
        }
    }


    public override void PhysicUpdate(){
        if(isExitThisState) return;
        if(ActiveDash){
            currentEnemy.ChaseEnemy();
            if(currentEnemy.playerDistance_x <0.5){
                Debug.Log("停止");
                currentEnemy.rb.gravityScale = 45;
                currentEnemy.anim.SetTrigger("Stop!");
                SummonGhost();
                DashTimes--;
                isExitThisState = true;
                
            }
        } 
    }
    public override void  OnExit(){
        attackDelay = 0;
        currentEnemy.wasHitedTimesCountInThisState = 0;
        currentEnemy.rb.gravityScale = beginingGravityScale;
         Debug.Log("離開BossWarrior_DashAndDashAttackState");
    }
    private void ActiveStage(){
        orignalPos = currentEnemy.transform.position;
        ActiveDash = true;
        currentEnemy.anim.SetTrigger("Dash!");
        Debug.Log(dashForce+"衝刺");
        currentEnemy.rb.AddForce(new Vector2(dashForce*currentEnemy.faceOn.x, 0),ForceMode2D.Impulse);
    }
    private void SummonGhost(){
        isGhostLive = true;
        Debug.Log("召喚GHOST");
        summonedOB = UnityEngine.Object.Instantiate(currentEnemy.Ghost,orignalPos+new Vector3(0,0.5f,0), Quaternion.identity);
        summonedOB.GetComponent<Rigidbody2D>().AddForce(new Vector2(dashForce*currentEnemy.faceOn.x, 0),ForceMode2D.Impulse);

    }
    private void CheckGhost(){
        if (summonedOB != null && Mathf.Abs(summonedOB.transform.position.x - currentEnemy.transform.position.x) < 2){
        UnityEngine.Object.Destroy(summonedOB); 
        isGhostLive = false;
        }
    }
    private WarriorBossstate StateChoose()
    {
        return WarriorBossstate.FollowPlayerAndAttackState;
    }
}
