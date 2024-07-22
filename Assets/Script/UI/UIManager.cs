using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public PlayerStateBar playerStateBar;
    [Header("監聽")]
    public CharacterEventSO HealthChangeEvent;
    private void OnEnable() {
        HealthChangeEvent.OnEventRaised += OnHealthEvent;
    }
    private void OnDisable() {
        HealthChangeEvent.OnEventRaised -= OnHealthEvent;
    }

    private void OnHealthEvent(Character character)
    {
        float persentage = character.healthPoint/character.maxHealth;
        playerStateBar.OnHealthChange(persentage);
    }
}
