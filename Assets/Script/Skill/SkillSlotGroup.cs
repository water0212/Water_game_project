using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class SkillSlotGroup : MonoBehaviour
{
     public string controlKey; // like "q" or "e"
    public Skill[] skills = new Skill[3];
    public bool[] readyFlags = new bool[3];

    public int currentIndex = 0;
    public int minReadyIndex = 0;
    public float fadeCount = 0f;
    public float fadeTime = 5f;

    private Skill currentSkill;
    private GameObject player;
    private Action<Skill, int> onEquip;
    private Action<int, bool> onFadeVisualUpdate;
    public void Initialize(GameObject playerObj, float fadeTime, Action<Skill, int> onEquipCallback, Action<int, bool> onFadeUI)
    {
        player = playerObj;
        fadeCount = 0;
        this.fadeTime = fadeTime;
        onEquip = onEquipCallback;
        onFadeVisualUpdate = onFadeUI;
    }
    public void LoadSkill(Skill skill, int index)
    {
        if (skill == null || index < 1 || index > 3) return;
        skills[index - 1] = skill;
        skill.OnLoad(player);

        if (currentSkill == null)
            EquipSkill(index);
        else if (currentIndex > index)
            EquipSkill(index);
    }

    public void EquipSkill(int index)
    {
        if (index < 1 || index > skills.Length || skills[index - 1] == null) return;

        if (currentSkill != null) currentSkill.UnEquip();
        currentSkill = skills[index - 1];
        currentSkill.OnEquip();
        currentIndex = index;
        fadeCount = 0;
        onFadeVisualUpdate?.Invoke(index - 1, false);
        onEquip?.Invoke(currentSkill, index);
    }

    public void GroupUpdate()
    {
        minReadyIndex = 0;
        for (int i = 0; i < skills.Length; i++)
        {
            Skill s = skills[i];
            if (s == null) continue;
            currentSkill.Update();
            bool isReady = s.BackGroundUpdate();
            readyFlags[i] = isReady;
            if (isReady && (minReadyIndex == 0 || minReadyIndex > i + 1))
                minReadyIndex = i + 1;
        }

        HandleFade();
    }

    private void HandleFade()
    {
        if (currentSkill == null) return;

        if (currentIndex > minReadyIndex && minReadyIndex != 0 && fadeCount <= 0 && !SkillManager.isUsingSkill)
        {
            fadeCount = fadeTime;
        }

        if (fadeCount > 0)
        {
            fadeCount -= Time.deltaTime;
            onFadeVisualUpdate?.Invoke(currentIndex - 1, true);
            if (fadeCount <= 0)
            {
                onFadeVisualUpdate?.Invoke(currentIndex - 1, false);
                EquipSkill(minReadyIndex);
            }
        }
    }

    public void ActivateSkill()
    {
        if (currentSkill == null || currentSkill.GetUseCount() <= 0) return;
        fadeCount = 0;
        onFadeVisualUpdate?.Invoke(currentIndex - 1, false);
        currentSkill.Activate(player);
    }

    public void SkillFinish() => SwitchToNext();

    private void SwitchToNext()
    {
        if (minReadyIndex == 0) return;
        for (int i = 0; i < skills.Length; i++)
        {

            if (readyFlags[i] && currentIndex-1 != i)
            {
                EquipSkill(i + 1);
                return;
            }
        }
        EquipSkill(minReadyIndex);
    }

    public Skill GetCurrentSkill() => currentSkill;
    public int GetCurrentIndex() => currentIndex;
}
