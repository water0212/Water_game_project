using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class SkillManager : MonoBehaviour
{
    // Start is called before the first frame updatezz
    public GameObject Player;
    public Skill testSkillLoad;
    public Skill currentSkill_E;
    public Skill currentSkill_Q;
    [Header("測試用")]
    public SkillDatabase skillDatabase;
    public SkillUIManager skillUIManager;
    [Header("監聽")]
    public FloatEventSO GainSkillEvent;
    
    private void OnEnable() {
        currentSkill_E = null;
        currentSkill_Q = null;
        GainSkillEvent.OnEventRaised += AddSkill;
    }
    private void OnDisable() {
        GainSkillEvent.OnEventRaised -= AddSkill;
    }
    private void Update() {
        if(currentSkill_E!=null)currentSkill_E.Update();
        if(currentSkill_Q!=null)currentSkill_Q.Update();
    }
    public void AddSkill(float num){
        skillUIManager.SkillAdd(skillDatabase.GetSkillByID((int)num));
        //LoadSkill_Q(testSkillLoad);
    }
    public void LoadSkill_TEST(InputAction.CallbackContext context)
    {
        Debug.Log("skill_Load");
        if(context.started)
        LoadSkill_E(testSkillLoad);
        
    }

    public void ActiveSkill_Q(InputAction.CallbackContext context)
    {
        
        if(currentSkill_Q == null){
            Debug.Log("qwq");
            return;
        }
        if(context.started)
        ActiveSkill(currentSkill_Q,Player);
    }

    public void ActiveSkill_E(InputAction.CallbackContext context)
    {
        if(currentSkill_E == null){
            Debug.Log("qeq");
            return;
        }
        if(context.started)
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
