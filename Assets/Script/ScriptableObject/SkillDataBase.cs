using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SkillDatabase", menuName = "SkillSystem/SkillDatabase")]
public class SkillDatabase : ScriptableObject
{
    public List<Skill> skills;

    public Skill GetSkillByID(int id)
    {
        return skills.Find(skill => skill.id == id);
    }
}