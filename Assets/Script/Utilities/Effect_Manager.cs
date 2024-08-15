using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Messaging;
using UnityEngine;
using Unity.Mathematics;
using TMPro;
public class Effect_Manager : MonoBehaviour
{
    // Start is called before the first frame update
    

    [Header("怪物效果")]
    public ParticleSystem damageParticleEffect;
    public float Effectlifetime = 1.5f;
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
        MonsterHurtEvent.OnEventRaised -= MonsterHurtEffectStart;
        MonsterDeadEvent.OnEventRaised -= MonsterDeadEffectStart;

    }
    public void MonsterHurtEffectStart(Vector3 Targetposition){
        Instantiate(MonsterHurtEffect,Targetposition,quaternion.identity);
        Debug.Log("受傷");
    }
    public void MonsterDeadEffectStart(Vector3 Targetposition){
        Instantiate(MonsterDeadEffect,Targetposition,quaternion.identity);
    }
}
