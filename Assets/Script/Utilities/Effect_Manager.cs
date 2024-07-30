using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Messaging;
using UnityEngine;
using Unity.Mathematics;
public class Effect_Manager : MonoBehaviour
{
    // Start is called before the first frame update
    

    [Header("怪物效果")]
    public ParticleSystem MonsterHurtEffect;
    public ParticleSystem MonsterAttackEffect;
    public ParticleSystem MonsterDeadEffect;
    [Header("怪物接收")]
    public PositionEventSO MonsterHurtEvent;
    public PositionEventSO MonsterDeadEvent;
    [Header("玩家效果")]
    public ParticleSystem PlayerHurtEffect;
    public ParticleSystem PlayerAttackEffect;
    private void OnEnable() {
        MonsterHurtEvent.OnEventRaised += MonsterHurtEffectStart;
        MonsterDeadEvent.OnEventRaised += MonsterDeadEffectStart;
    }
    private void OnDisable() {
        
    }
    public void MonsterHurtEffectStart(Vector3 Targetposition){
        Instantiate(MonsterHurtEffect,Targetposition,quaternion.identity);
    }
    public void MonsterDeadEffectStart(Vector3 Targetposition){
        Instantiate(MonsterDeadEffect,Targetposition,quaternion.identity);
    }
}
