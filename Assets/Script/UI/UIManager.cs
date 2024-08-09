using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public PlayerStateBar playerStateBar;
    public SkillUIManager skillUIManager;
    [Header("監聽")]
    public CharacterEventSO HealthChangeEvent;
    public CharacterEventSO RollingChangeEvent;
    public FloatEventSO ExperienceChangeEvent;
    
    private void OnEnable() {
        HealthChangeEvent.OnEventRaised += OnHealthEvent;
        RollingChangeEvent.OnEventRaised += RollingEvent;
        ExperienceChangeEvent.OnEventRaised += ExperienceEvent;
        
    }
    private void OnDisable() {
        HealthChangeEvent.OnEventRaised -= OnHealthEvent;
        RollingChangeEvent.OnEventRaised -= RollingEvent;
        ExperienceChangeEvent.OnEventRaised -= ExperienceEvent;
    }

    private void ExperienceEvent(float ExperiencePoint)
    {
        playerStateBar.OnExperienceChange(ExperiencePoint);
    }

    private void RollingEvent(Character character)//客製化UI
    {
        float persentage = (float)character.RollTimes/(float) character.MaxRollTimes;
        if(persentage == 0.5f)
        playerStateBar.OnRollingTimesChange(0.8f);
        else if(persentage == 1f){
            playerStateBar.OnRollingTimesChange(1f);
        } else {
            playerStateBar.OnRollingTimesChange(0);
        }
    }

    private void OnHealthEvent(Character character)
    {
        float persentage = character.healthPoint/character.maxHealth;
        playerStateBar.OnHealthChange(persentage);
    }
    
}
