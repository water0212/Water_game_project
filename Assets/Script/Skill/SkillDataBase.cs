using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SkillDatabase", menuName = "SkillSystem/SkillDatabase")]
public class SkillDatabase : ScriptableObject
{
    public List<Skill> skills;
    public int MaximumSkillCapacity;
    public void SkillAdd( Skill skill){
        if(skills.Count<MaximumSkillCapacity){
        skills.Add(skill);
        Debug.Log("解鎖新技能" + skill.name);    
        }else{
            Debug.Log("技能已滿" + skill.name + "無法容納");
        }
        
    }
    public Skill GetSkillByID(int id)
    {
        return skills.Find(skill => skill.GetId() == id);
    }
}