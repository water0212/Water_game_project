//using System.Drawing;
using UnityEngine;
using UnityEngine.UI;
public abstract class Skill : ScriptableObject
{
    
    public string skillName;
    public string skillDescription;
    public int id;
    public float cooldown;
    public float cooldownCount;
    public int useCount;
    public int MaxUseCount;
    public float duration;
    public Sprite skillImage;
    [HideInInspector]public bool isUpdate;
    [HideInInspector] public Animator animator;
    [Tooltip("要播放的技能片段")]
    public AnimationClip clip;
    public abstract void OnLoad(GameObject user);
    public abstract void OnEquip();
    public abstract void Update();
    public abstract bool Activate(GameObject user);
    public abstract void UnEquip();
    public abstract void UnLoad();
    public virtual bool BackGroundUpdate(){
        ColdDownUpdate();
        if(useCount > 0) return true;
        else return false;
    }
    public virtual void ColdDownUpdate(){
        if(isUpdate) return;
        if(useCount < MaxUseCount){
            cooldownCount += Time.deltaTime;
            if(cooldownCount > cooldown){
                useCount ++;
                cooldownCount = 0;
            }
        }
    }
    public virtual void ColdDownChange(float num){
        cooldownCount += num;
        if(cooldownCount > cooldown) {
            useCount ++;
            cooldownCount = 0;
        }
    } 
}