using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillSummon : MonoBehaviour, ISummonedObject
{
    public GameObject user;
    public float attack;
    public Vector2 attackDisplaces;
    public float duration;
    
    public void Initialize(GameObject user, float attack,Vector2 attackDisplaces, float duration)
    {
        this.user = user;
        this.attack = attack;
        this.attackDisplaces = attackDisplaces;
        this.duration = duration;
    }
    
}
