using System.Drawing;
using UnityEngine;

public abstract class Skill : ScriptableObject
{
    
    public string skillName;
    public int id;
    public float cooldown;
    public float duration;
    public Image image;
    public AnimationClip animator;
    public abstract void OnLoad();
    public abstract void Activate(GameObject user);
    public abstract void OnExit();
}