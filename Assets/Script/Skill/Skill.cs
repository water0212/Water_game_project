//using System.Drawing;
using UnityEngine;
using UnityEngine.UI;
using System;
public abstract class Skill : ScriptableObject
{
    
    [SerializeField]protected string skillName;
    [SerializeField]protected string skillDescription;
    [SerializeField]protected int id;
    public float Maxcooldown;
    [SerializeField]protected float cooldownCount;
    [SerializeField]protected int useCount;
    public int MaxUseCount;
    public float duration;
    public Sprite skillImage;
    [HideInInspector]public bool isUpdate;
    [HideInInspector] public Animator animator;
    [Tooltip("要播放的技能片段")]
    public AnimationClip clip;
    public event Action<float> onCooldownChanged;
    public event Action<int> onUseCountChanged;
    public abstract void OnLoad(GameObject user);
    public abstract void OnEquip();
    public abstract void Update();
    public abstract bool Activate(GameObject user);
    public abstract void UnEquip();
    public abstract void UnLoad();
    public virtual bool BackGroundUpdate(){
        ColdDownUpdate();
        return useCount > 0;
    }
    public virtual void ColdDownUpdate(){
        if(isUpdate) return;
        if(useCount < MaxUseCount){
            cooldownCount += Time.deltaTime;
            onCooldownChanged?.Invoke(cooldownCount / Maxcooldown);
            if(cooldownCount > Maxcooldown){
                useCount ++;
                cooldownCount = 0;
                onUseCountChanged?.Invoke(useCount);
            }
        }
        isUpdate = true;
    }
    public virtual void ColdDownChange(float num){
        if(useCount >= MaxUseCount) return;
        cooldownCount += num;
        if(cooldownCount > Maxcooldown) {
            useCount ++;
            cooldownCount = 0;
            onUseCountChanged?.Invoke(useCount);
        }
        
    } 
    public void NotifyActivated(Skill skill)
    {
        Debug.Log("使用技能 " + skillName);
        SkillManager.GetSkillManager().ActiveSkill(skill);
    }
    public float GetCooldown() => cooldownCount;
    public int GetUseCount() => useCount;
    public string GetSkill_Name() => skillName;
    public string GetSkill_Description() => skillDescription;
    public int GetId() => id;
}