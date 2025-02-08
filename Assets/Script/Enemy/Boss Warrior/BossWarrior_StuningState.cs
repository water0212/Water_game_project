using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossWarrior_StuningState : BaseState<BossWarriorEnemy>
{
    // Start is called before the first frame update
    float currentDefense;
    public override void OnEnter(BossWarriorEnemy Enemy)
    {
        Debug.Log("Boss暈眩");
        currentEnemy = Enemy;
        currentEnemy.DebugLog.text = "BossWarrior_Stun";
        currentDefense = currentEnemy.defense;
        currentEnemy.defense = currentDefense*0.5f;
    }

    public override void PhysicUpdate()
    { 
    }
    public override void LogicUpdate()
    {
    }

    public override void OnExit()
    {
        currentEnemy.defense = currentDefense;
    }
}
