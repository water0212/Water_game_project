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
    public Button activeButtonE;
    public Button activeButtonQ;
    public Image lockE;
    public Image lockQ;
    public Sprite lockItem;
    public Skill currentSkill;
    [Header("技能冷卻UI")]
    public Image skillE_CD_Icon;
    public Image skillE_CD_BackIcon;
    public Image skillQ_CD_Icon;
    public Image skillQ_CD_BackIcon;
    public TextMeshProUGUI E_TimeOfUse;
    public TextMeshProUGUI Q_TimeOfUse;
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
    public void OpenSkillChooseUI()
    {
        if(Skill_Canvas.gameObject.activeSelf == true){
            CloseSkillChooseUI();
            return;
        }
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
        if(skill == null) return;
        currentSkill = skill; 
        if(lockE.sprite != lockItem||lockE.gameObject.activeSelf == false){
            activeButtonE.gameObject.SetActive(true);
        }
        if(lockQ.sprite != lockItem||lockQ.gameObject.activeSelf == false){
           activeButtonQ.gameObject.SetActive(true);
        }
        if(lockE.sprite==lockItem&&lockQ.sprite==lockItem){
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
        if(lockQ.gameObject.activeSelf==false) lockQ.gameObject.SetActive(true);
        lockQ.sprite = currentSkill.skillImage;
        skillManager.LoadSkill_Q(currentSkill);
    }
    public void UpdateSkillIcon_E(float persentage){
        skillE_CD_Icon.fillAmount = persentage;
    }
    public void UpdateSkillIcon_Q(float persentage){
        skillQ_CD_Icon.fillAmount = persentage;
    }
    public void ChangeTimeOfUse_E(int Times){
        E_TimeOfUse.text = Times.ToString();
    }
    public void ChangeTimeOfUse_Q(int Times){
        Q_TimeOfUse.text= Times.ToString();
    }
    public void ChangeSkillIcon_E(Sprite sprite){
        skillE_CD_Icon.sprite = sprite;
        skillE_CD_BackIcon.sprite = sprite;
    }
    public void ChangeSkillIcon_Q(Sprite sprite){
        skillQ_CD_Icon.sprite = sprite;
        skillQ_CD_BackIcon.sprite = sprite;
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
