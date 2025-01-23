using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using JetBrains.Annotations;
using TMPro;
using Unity.VisualScripting;

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
    private Skill currentSkill;
    public Button[] activeButtonE = new Button[3];
    public Button[] activeButtonQ = new Button[3];
    public Image[] lockE = new Image[3];
    public Image[] lockQ = new Image[3];
    public Sprite lockItem;
    [Header("技能冷卻UI")]

    [Header("右方技能列")]
    public Image[] skill_E_CD_Icon = new Image[3];
    public Image[] skill_E_CD_BackIcon = new Image[3];
    public TextMeshProUGUI[] canUse_E = new TextMeshProUGUI[3];
    [Header("左方技能列")]
    public Image[] skill_Q_CD_Icon = new Image[3];
    public Image[] skill_Q_CD_BackIcon = new Image[3];
    public TextMeshProUGUI[] canUse_Q = new TextMeshProUGUI[3];
    [Header("獲取技能UI通知")]
    private Queue<Skill> SkillQueue;
    public GameObject getUIFrame;
    private Animator getUIAnimation;
    public TextMeshProUGUI getSkill_Name;
    public Image getSkill_Image;
    private Coroutine QueueChecker;

    private void Awake() {
        getUIAnimation = getUIFrame.GetComponent<Animator>();
        SkillQueue = new Queue<Skill>();
    }
    private void Start() {
        UpdateSkillSlots();
        CloseSkillChooseUI();
    }
    private void OnEnable() {
        skillDescriptionUpdateEvent.OnEventRaised += UpdateSkillDescription; 
    }
    public void SkillAdd( Skill skill){
            for(int i = 0; i < skillUISlots.Length; i++ ){
                if(skillUISlots[i].skill==null){
                skillUISlots[i].UpdateSkillUI(skill);   
                Debug.Log("解鎖新第" + i + "技能" + skill.name);   
                SetSkillNotifyQueue(skill);
                break;
                }
                if(i==skillUISlots.Length-1){
                    //TODO當技能滿額時，給予選擇刪除或擴充的資格
                }
            } 
    }
    public void UpdateSkillSlots(){
        skillUISlots = SkillSlotsParent.GetComponentsInChildren<SkillUISlots>();
    }
    public void OpenSkillChooseUI()
    {
        if(Skill_Canvas.gameObject.activeSelf == true){
            CloseSkillChooseUI();
            return;
        }
        Skill_Canvas.gameObject.SetActive(true);
        EnterSkillUI_icon.gameObject.SetActive(false);
    }
    public void CloseSkillChooseUI(){
        Skill_Canvas.gameObject.SetActive(false);
        EnterSkillUI_icon.gameObject.SetActive(true);
    }
        private void UpdateSkillDescription(string TEXT)
    {
        SkillDescription.text = TEXT;
    }
    public void OpenSkillChoose(Skill skill){
        if(skill == null) return;
        currentSkill = skill;
        for (int i = 0; i < 3 ; i++){
            if(lockE[i].sprite != lockItem||lockE[i].gameObject.activeSelf == false){
                activeButtonE[i].gameObject.SetActive(true);
            }
            if(lockQ[i].sprite != lockItem||lockQ[i].gameObject.activeSelf == false){
                activeButtonQ[i].gameObject.SetActive(true);
            }
        } 
        //TODO:把skill搬移到Check中 然後再從check呼叫skillManager的Load
    }
    public void CloseSkillChoose(){
        foreach(Button activeButton in activeButtonE)
        if(activeButton.gameObject.activeSelf == true){
            activeButton.gameObject.SetActive(false);
        }
        foreach(Button activeButton in activeButtonQ)
            activeButton.gameObject.SetActive(false);
        }
    }
    public void ChooseE(int index){ //TODO:需要變成三個格子的CHECK
        //if(lockE[index-1].gameObject.activeSelf) lockE[index-1].gameObject.SetActive(false);
        DeleteActiveButton();
        lockE[index-1].sprite = currentSkill.skillImage;
        skill_E_CD_Icon[index-1].sprite = currentSkill.skillImage;
        skill_E_CD_BackIcon[index-1].sprite = currentSkill.skillImage;
        skillManager.LoadSkill_E(currentSkill,index);
    }
    public void ChooseQ(int index){//TODO:需要變成三個格子的CHECK
        //if(lockQ[index-1].gameObject.activeSelf) lockQ[index-1].gameObject.SetActive(false);
        DeleteActiveButton();
        lockQ[index-1].sprite = currentSkill.skillImage;
        skill_Q_CD_Icon[index-1].sprite = currentSkill.skillImage;
        skill_Q_CD_BackIcon[index-1].sprite = currentSkill.skillImage;
        skillManager.LoadSkill_Q(currentSkill,index);
        //拖移至
    }
    private void DeleteActiveButton(){
        for (int i = 0; i < 3 ; i++){
                activeButtonE[i].gameObject.SetActive(false);
                activeButtonQ[i].gameObject.SetActive(false);
            
        }
    }
    public void UpdateSkillIcon_E(float persentage,int i){
        skill_E_CD_Icon[i].fillAmount = persentage;
    }
    public void UpdateSkillIcon_Q(float persentage,int i){
        skill_Q_CD_Icon[i].fillAmount = persentage;
    }
    public void ChangeTimeOfUse_E(int Times,int i){
        canUse_E[i].text = Times.ToString();
    }
    public void ChangeTimeOfUse_Q(int Times,int i){
        canUse_Q[i].text= Times.ToString();
    }
    public void ChangeEquipSkillIcon_E(int i){//更改目前裝備技能
        
    }
    public void ChangeEquipSkillIcon_Q(int i){//更改目前裝備技能
        
    }
    private void SetSkillNotifyQueue(Skill skill){
        SkillQueue.Enqueue(skill);
        if( QueueChecker == null ){
            QueueChecker = StartCoroutine(CheckNotifyQueue());
        }
    }
    private void ShowSkillGet(Skill skill){
        getUIFrame.SetActive(true);
        getUIAnimation.Play("GetSkillNotify");
        getSkill_Name.text = skill.name;
        getSkill_Image.sprite = skill.skillImage;
        
    }
    private IEnumerator CheckNotifyQueue(){
        do {
            ShowSkillGet(SkillQueue.Dequeue());
            do {
                yield return null;
            } while (!getUIAnimation.GetCurrentAnimatorStateInfo(0).IsTag("Idle")); 
        } while (SkillQueue.Count > 0 );
        getUIFrame.SetActive(false);
        QueueChecker = null;
    }
}
