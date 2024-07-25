using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public PlayerStateBar playerStateBar;
    [Header("監聽")]
    public CharacterEventSO HealthChangeEvent;
    public CharacterEventSO RollingChangeEvent;
    private void OnEnable() {
        HealthChangeEvent.OnEventRaised += OnHealthEvent;
        RollingChangeEvent.OnEventRaised += RollingEvent;
        
    }
    private void OnDisable() {
        HealthChangeEvent.OnEventRaised -= OnHealthEvent;
        RollingChangeEvent.OnEventRaised -= RollingEvent;
    }

    private void RollingEvent(Character character)
    {
        float persentage = character.RollTimes/character.MaxRollTimes;
        playerStateBar.OnHealthChange(persentage);
    }

    private void OnHealthEvent(Character character)
    {
        float persentage = character.healthPoint/character.maxHealth;
        playerStateBar.OnHealthChange(persentage);
    }
}
