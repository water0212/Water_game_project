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
    [HideInInspector] public Animator animator;
    public AnimationClip animClip;
    public abstract void OnLoad(GameObject user);
    public abstract void Update();
    public abstract void Activate(GameObject user);
    public abstract void OnExit();
}