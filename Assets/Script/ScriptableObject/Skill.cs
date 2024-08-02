using System.Drawing;
using UnityEngine;

public abstract class Skill : ScriptableObject
{
    
    public string skillName;
    public string skillDescription;
    public int id;
    public float cooldown;
    public float duration;
    public Image image;
    [HideInInspector] public Animator animator;
    public AnimationClip animClip;
    public abstract void OnLoad(GameObject user);
    public abstract void Update();
    public abstract void Activate(GameObject user);
    public abstract void OnExit();
}