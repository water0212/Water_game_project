using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossWarriorBossDeadState : BaseState<BossWarriorEnemy>
{
    private bool isExitThisState;
    private float TineCount;
    private Material material;
    private bool isDissolving;
    private float fade = 1;
    public override void OnEnter(BossWarriorEnemy Enemy)
    {
        Debug.Log("進入BossWarrior_BossDead");
        currentEnemy = Enemy;
        currentEnemy.DebugLog.text = "BossWarrior_BossDead";
        currentEnemy.anim.Play("Death");
        material = currentEnemy.GetComponent<SpriteRenderer>().material;

    }
    public override void LogicUpdate()
    {
       if(currentEnemy.canDisslove){
            fade -= Time.deltaTime*0.3f;
            if(fade <=0){
                fade = 1;
                Debug.Log("開始腐爛");
                currentEnemy.BossDead.RaiseEvent();
                currentEnemy.DestoryGB();
            }
            material.SetFloat("_Fade", fade);
        }
    }
    public override void PhysicUpdate()
    {
       
    }
    public override void OnExit()
    {
        
    }
}
