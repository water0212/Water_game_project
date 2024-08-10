using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using JetBrains.Annotations;
using TMPro;

public class SkillUIManager : MonoBehaviour
{
    // Start is called before the first frame update
    public Canvas Skill_Canvas;
    public Canvas EnterSkillUI_icon;
    public SkillUISlots[] skillUISlots;
    public Transform SkillSlotsParent;
    public SkillManager skillManager;
    public TextMeshProUGUI SkillDescription;
    public StringEventSO skillDescriptionUpdateEvent;
    public Button activeButtonE;
    public Button activeButtonQ;
    public Image lockE;
    public Image lockQ;
    public Sprite lockItem;
    public Skill currentSkill;
    private void Start() {
        UpdateSkillSlots();
    }
    private void OnEnable() {
        skillDescriptionUpdateEvent.OnEventRaised += UpdateSkillDescription; 
    }
    public void SkillAdd( Skill skill){
            for(int i = 0; i < skillUISlots.Length; i++ ){
                if(skillUISlots[i].skill==null){
                skillUISlots[i].UpdateSkillUI(skill);   
                Debug.Log("解鎖新第" + i + "技能" + skill.name);   
                break;
                }
                if(i==skillUISlots.Length-1){
                    //TODO當技能滿額時，給予選擇刪除或擴充的資格
                }
            } 
    }
    public void OpenSkillChooseUI()
    {
        Skill_Canvas.gameObject.SetActive(true);
        EnterSkillUI_icon.gameObject.SetActive(false);
        Debug.Log ("www");
    }
    public void CloseSkillChooseUI(){
        Skill_Canvas.gameObject.SetActive(false);
        EnterSkillUI_icon.gameObject.SetActive(true);
    }
    public void UpdateSkillSlots(){
        skillUISlots = SkillSlotsParent.GetComponentsInChildren<SkillUISlots>();
    }
        private void UpdateSkillDescription(string TEXT)
    {
        SkillDescription.text = TEXT;
    }
    public void OpenSkillChoose(Skill skill){
        currentSkill = skill;
        if(lockE.sprite != lockItem||lockE.gameObject.activeSelf == false){
            activeButtonE.gameObject.SetActive(true);
        }
        if(lockQ.sprite != lockItem||lockQ.gameObject.activeSelf == false){
           activeButtonQ.gameObject.SetActive(true);
        }
        if(lockE.sprite==lockItem||lockQ.sprite==lockItem){
            Debug.Log("你尚未學會施放技能");
        }
    }
    public void CloseSkillChoose(){
        if(activeButtonE.gameObject.activeSelf == true){
            activeButtonE.gameObject.SetActive(false);
        }
        if(activeButtonQ.gameObject.activeSelf == true){
            activeButtonQ.gameObject.SetActive(false);
        }
    }
    public void CheckE(){
        if(lockE.gameObject.activeSelf==false) lockE.gameObject.SetActive(true);
        lockE.sprite = currentSkill.skillImage;
        skillManager.LoadSkill_E(currentSkill);
    }
    public void CheckQ(){
        if(lockQ.gameObject.activeSelf==false) lockE.gameObject.SetActive(true);
        lockQ.sprite = currentSkill.skillImage;
        skillManager.LoadSkill_Q(currentSkill);
    }
}
