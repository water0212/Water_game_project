using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightPatrolState : BaseState<knightEnemy>
{
    knightEnemy currentknightEnemy;
    public override void LogicUpdate()
    {
        if(currentEnemy.FoundEnemy()){
            currentEnemy.inCombat = true;
            currentEnemy.chasingTimeCount = currentEnemy.chaseTime ;
            currentEnemy.SwitchState(NPCstate.Chase);
        }
    }

    public override void OnEnter(knightEnemy enemy)
    {
        currentEnemy = enemy;
        currentEnemy.currentSpeed = currentEnemy.patrolSpeed;
        currentEnemy.anim.SetBool("Runing", true); //武士_動畫狀態_跑步
    }

    public override void OnExit()
    {
        currentEnemy.anim.SetBool("Runing", false);
    }

    public override void PhysicUpdate()
    {
        if(currentEnemy.canMove_playerDead&&!currentEnemy.isDead)PatrolMove();
    }
    virtual public void PatrolMove() {
        if(currentEnemy.physicCheck.isGround&&!currentEnemy.physicCheck.isOnTheFloor||currentEnemy.physicCheck.touchWall){
            currentEnemy.transform.localScale = new Vector3 (-currentEnemy.transform.localScale.x, 1,1); //武士_當碰到牆壁或懸崖時回頭
            currentEnemy.DontRotateObj.RotateLock((int)currentEnemy.transform.localScale.x);
        }
        if(currentEnemy.physicCheck.isGround) {
        currentEnemy.rb.velocity = new Vector2(currentEnemy.currentSpeed*currentEnemy.faceOn.x*Time.deltaTime,currentEnemy.rb.velocity.y) ; //武士_在地上時移動
        }
        
    }
}
