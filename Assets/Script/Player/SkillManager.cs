using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Player;
    public Skill testSkillLoad;
    public Skill currentSkill_E;
    public Skill currentSkill_Q;
    public VoidEventSO E_SkillActiveEvent;
    public VoidEventSO Q_SkillActiveEvent;
    public VoidEventSO SkillLoadEvent;
    private void OnEnable() {
        currentSkill_E = null;
        currentSkill_Q = null;
        E_SkillActiveEvent.OnEventRaised += ActiveSkill_E;
        Q_SkillActiveEvent.OnEventRaised += ActiveSkill_Q;
        SkillLoadEvent.OnEventRaised += LoadSkill_TEST;
    }

    private void LoadSkill_TEST()
    {
        Debug.Log("skill_Load");
        LoadSkill_E(testSkillLoad);
        
    }

    private void ActiveSkill_Q()
    {
        if(currentSkill_Q == null){
            Debug.Log("qwq");
        }
       ActiveSkill(currentSkill_Q,Player);
    }

    private void ActiveSkill_E()
    {
        if(currentSkill_E == null){
            Debug.Log("qeq");
        }
        ActiveSkill(currentSkill_E,Player);
    }

    public void LoadSkill_E(Skill sklnum){
        
        currentSkill_E = sklnum;
        currentSkill_E.OnLoad(Player);
    }
    public void LoadSkill_Q(Skill sklnum){
        
        currentSkill_Q = sklnum;
        currentSkill_Q.OnLoad(Player);
    }
    public void ActiveSkill(Skill currrentSkilluse, GameObject Player){
        if(currrentSkilluse != null){
            
            currrentSkilluse.Activate(Player);
        }
    }
}
