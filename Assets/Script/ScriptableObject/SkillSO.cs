using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
[CreateAssetMenu(menuName ="Event/SkillEvnetSO")]
public class SkillEventSO : ScriptableObject
{
    public UnityAction<Skill> OnEventRaised;
    
    public void RaiseEvent(Skill num ){
        OnEventRaised?.Invoke(num);   
    }
}
